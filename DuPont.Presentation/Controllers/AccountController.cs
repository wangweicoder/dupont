// ***********************************************************************
// Assembly         : DuPont.Presentation
// Author           : 毛文君
// Created          : 10-27-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 10-27-2015
// ***********************************************************************
// <copyright file="AccountController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;


using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using DuPont.Presentation.Models.Dto.Account;
using DuPont.Presentation.Properties;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using System;
using System.Web.Mvc;
using DuPont.Models.Enum;
using DuPont.Models.Dtos.Foreground.Account;
using DuPont.Global.ActionResults;
using DuPont.Models.Models;
namespace DuPont.Presentation.Controllers
{
    [Validate]
    public class AccountController : BaseController
    {
        private static new string Url = ConfigHelper.GetAppSetting(DataKey.RemoteApiForRelease);

        #region "用户注册"
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="Password">密码</param>
        /// <param name="ValidateCode">手机验证码</param>
        /// <returns>返回结果</returns>
        [HttpPost]
        public string Register(RegisterInput model)
        {
            SetJsonHeader();
            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "用户登录"
        #region "普通用户登录"
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="Password">登录密码</param>
        /// <returns>返回结果</returns>
        [HttpPost]
        public string Login(Login model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);

            return result;
        } 
        #endregion

        #region "第三方用户登录"
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="Password">登录密码</param>
        /// <returns>返回结果</returns>
        [HttpPost]
        public string SocialLogin(SocialLogin model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);

            return result;
        } 
        #endregion
        #endregion

        #region "大农户角色认证"
        /// <summary>
        /// 大农户角色认证
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="RealName">真实姓名</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <param name="DoPuntOrderNumbers">杜帮订单号</param>
        /// <param name="Land">共有土地</param>
        /// <param name="PurchasedProductsQuantity">已购先锋亩数</param>
        /// <returns>System.Web.Mvc.string.</returns>
        [HttpPost]
        public string RoleFarmerRegister(RoleFarmerRegister model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);


            return result;
        }
        #endregion

        #region "农机手角色认证"
        /// <summary>
        /// 农机手角色认证
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="RealName">真实姓名</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <param name="Machinery">拥有的农机</param>
        /// <param name="OtherMachinery">其它农机</param>
        /// <param name="PicturesIds">图片编号列表</param>
        /// <returns>string.</returns>
        public string RoleOperatorRegister(RoleOperatorRegister model)
        {
            SetJsonHeader();
            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);

            return result;
        }
        #endregion

        #region "产业商角色认证"
        /// <summary>
        /// 产业商角色申请
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="RealName">真实姓名</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <param name="PurchaseType">收购类型</param>
        /// <param name="Description">备注</param>
        /// <param name="PicturesIds">图片编号列表</param>
        /// <returns>string.</returns>
        public string RoleBusinessRegister(RoleBusinessRegister model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "获取用户申请的待审核的角色编号列表"
        /// <summary>
        /// 获取用户申请的待审核的角色编号列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>string.</returns>
        public string RoleVerificaionInfo(RoleVerificaionInfo model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "用户登出"
        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="Token">用户登录Token值</param>
        /// <returns>返回结果</returns>
        public string Logout(Logout model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "我的发布"
        /// <summary>
        /// 获取发布列表
        /// </summary>
        /// <param name="UserId">发布者id</param>
        /// <param name="IsClosed">发布状态：0表示进行中，1表示已关闭</param>
        /// <param name="RoleTyp">角色类别（根据角色类别读取相应表的数据）</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">一页显示的条数</param>
        /// <returns>返回发布列表集合</returns>
        [HttpPost]
        public string PublishedRequirement(PublishedRequirement model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "校验验证码"
        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="smscode"></param>
        /// <param name="phonenumber"></param>
        /// <returns></returns>
        public string ValidCode(ValidCode model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "修改密码"
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newpas"></param>
        /// <param name="phonenumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdPas(UpdatePasswordInput input)
        {
            SetJsonHeader();

            var parameter = ModelHelper.GetPropertyDictionary<UpdatePasswordInput>(input);
            var result = PostStandardWithSameControllerAction<UpdatePasswordOutput>(this, parameter);

            return new JsonResultEx(result);
        }
        #endregion

        #region "获取用户的个人信息"
        /// <summary>
        /// 获取用户的个人信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="RoleId">角色编号</param>
        /// <returns>string.</returns>
        [HttpPost]
        public new string Profile(Profile model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();


            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "修改个人信息"
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Name">用户名称</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <returns>string.</returns>
        [HttpPost]
        public string SaveProfile(SaveProfileInput model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "获取已审核的角色列表"
        /// <summary>
        /// 获取用户已审核的角色列表
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <returns>string.</returns>
        [HttpPost]
        public string RoleList(RoleList model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region "获取用户信息"
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserId">用户编号</param>      
        /// <author>ww</returns>
        [HttpPost]
        public  string UserInfo(long UserId)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region 修改用户信息
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Name">用户名称</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <author>ww</author>
        [HttpPost]
        public string SaveUserInfo(SaveProfileInput model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion      

        #region E田登录返回Token
        public string GetToken(string Userid, string Password)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region 修改超期的订单状态（大农户）
        public string UpdateFarmerRequirementState()
        {
            SetJsonHeader();
            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region 修改超期的订单状态（产业商）
        public string UpdateBusinessRequirementState()
        {
            SetJsonHeader();
            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            string result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion
    }
}