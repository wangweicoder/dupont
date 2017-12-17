// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 10-27-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-04-2015
// ***********************************************************************
// <copyright file="FormsAuthProvider.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************




using DuPont.Entity.Enum;
using DuPont.Global.Exceptions;
using DuPont.Interface;
using DuPont.Models;
using DuPont.Models.Dtos.Background;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DuPont.Infrastructure
{
    public class FormsAuthProvider : IAuthProvider
    {
        private IAdminUser adminUserRepository;
        private IUser userRepository;
        public FormsAuthProvider(IAdminUser adminUserRepository, IUser userRepository)
        {
            this.adminUserRepository = adminUserRepository;
            this.userRepository = userRepository;
        }

        public AdminUserLoginInfo Authenticate(LoginInputDto input)
        {
            if (string.IsNullOrEmpty(input.LoginUserName))
                throw new ArgumentNullException("LoginUserName");

            if (string.IsNullOrEmpty(input.Password))
                throw new ArgumentNullException("Password");

            //根据账号和密码取得用户信息
            var adminUserInfo = adminUserRepository.GetAll(u => u.UserName == input.LoginUserName).FirstOrDefault();

            /*检查用户登录
             * --是户名是否存在
             * --密码是否匹配
             * --用户是否被锁定
             */
            CheckLoginCondition(adminUserInfo, input);

            //获取用户拥有的角色列表
            var userRoleList = adminUserRepository.GetRoles(adminUserInfo.Id);
            if (!adminUserInfo.IsSuperAdmin && userRoleList.Count() == 0)
                throw new UnauthorizedAccessException();

            var adminUserLoginInfo = new AdminUserLoginInfo()
            {
                Roles = userRoleList,
                User = adminUserInfo
            };

            return adminUserLoginInfo;
        }

        /// <summary>
        /// 检查用户登录:用户名是否存在;密码是否正确;账号是否锁定
        /// </summary>
        /// <param name="user"></param>
        /// <param name="input"></param>
        private void CheckLoginCondition(T_ADMIN_USER user, LoginInputDto input)
        {
            //检查用户是否存在
            if (user == null)
                throw new CustomException("用户不存在!");

            //检查密码是否正确
            var pwdHashCode = Encrypt.MD5Encrypt(input.Password);
            if (pwdHashCode != user.Password)
                throw new CustomException("用户名或密码不正确!");

            //检查账号是否锁定
            if (user.IsLock)
                throw new CustomException("用户已被锁定,禁止登录!");
        }
    }
}