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
// <copyright file="NavController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Attributes;
using DuPont.Global.ActionResults;
using DuPont.Global.Exceptions;
using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Controllers
{
    [CustomHandleErrorWithErrorJson]
    public class NavController : BaseController
    {
        private IMenu menuRepository;
        private IMenu_Role menuRoleRepository;
        private IUser_Role userRoleRepository;
        private IAdminUser userRepository;
        public NavController(IPermissionProvider permissionManage, IMenu menu,
            IMenu_Role menuRole, IAdminUser userRepository, IUser_Role userRole)
            : base(permissionManage)
        {
            this.menuRepository = menu;
            this.menuRoleRepository = menuRole;
            this.userRoleRepository = userRole;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public JsonResult Menu(Int64 userId)
        {
            using (ResponseResult<List<T_MENU>> result = new ResponseResult<List<T_MENU>>())
            {
                //获取用户信息
                var currentUser = userRepository.GetAll(user => user.Id == userId).FirstOrDefault();
                if (currentUser == null)
                    throw new CustomException("用户不存在!");

                IEnumerable<T_USER_ROLE_RELATION> user_RoleList = null;
                //非超级管理员，则获取用户拥有的角色信息
                if (!currentUser.IsSuperAdmin)
                {
                    //检查账号是否锁定
                    if (currentUser.IsLock)
                        throw new CustomException("用户已被锁定,禁止登录!");
                    user_RoleList = userRoleRepository.GetAll(userRole => userRole.UserID == userId && !userRole.MemberType);
                }

                if (currentUser.IsSuperAdmin || user_RoleList != null)
                {
                    IList<T_MENU> menuList = null;
                    if (currentUser.IsSuperAdmin)
                        menuList = menuRepository.GetAll(mnu=>mnu.Visible);
                    else
                    {
                        var roleIdList = user_RoleList.Select(userRole => userRole.RoleID).ToList<int>();
                        var role_MenuList = this.menuRoleRepository.GetAll(menuRole => roleIdList.Contains(menuRole.RoleId));
                        var menuIdList = role_MenuList.Select(menuRole => menuRole.MenuId).ToList<int>();
                        menuList = this.menuRepository.GetAll(menu => menuIdList.Contains(menu.Id) && menu.Visible);
                    }

                    result.IsSuccess = true;
                    result.Entity = menuList.ToList().OrderBy(mnu=>mnu.Order).ToList();
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.UserNotExist;
                }

                return new JsonResultEx(result);
            }
        }
    }
}