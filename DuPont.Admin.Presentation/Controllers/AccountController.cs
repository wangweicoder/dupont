


using DuPont.Admin.Presentation.Filters;
using DuPont.Interface;
using DuPont.Admin.Presentation.Models;
using DuPont.Utility;
using System;
using System.Web.Mvc;
using System.Configuration;
using System.Collections.Generic;
using DuPont.Utility.LogModule.Model;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Admin.Attributes;
using DuPont.Models.Models;
using DuPont.Models.Enum;

namespace DuPont.Admin.Presentation.Controllers
{
    [Validate]
    public class AccountController : BaseController
    {

        #region "管理账号登录/登出"
        //
        // GET: /Login/
        public ActionResult Login()
        {
            if (Request.Cookies[SessionCookieName] != null)
            {
                HttpContext.Items[DataKey.RemoveSessionCookie] = "yes";
            }

            return View();
        }

        //
        // POST /Login/
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            var parameters = ModelHelper.GetPropertyDictionary<LoginViewModel>(model);
            var responseObjResult = PostStandardWithSameControllerAction<AdminUserLoginInfo>(this, parameters);

            if (responseObjResult.IsSuccess)
            {
                var user = responseObjResult.Entity;
                Session[DataKey.UserInfo] = user;
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ModelState.AddModelError("", responseObjResult.Message);
            }

            return View();
        }


        [HttpGet]
        // POST /LoginOut/
        public new ActionResult LoginOut()
        {
            base.LoginOut();

            return RedirectToAction("Login");
        }
        #endregion

        #region "账号是否是登录状态"
        /// <summary>
        /// 账号是否是登录状态
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <param name="OldPwd"></param>
        /// <returns></returns>
        [HttpPost]
        public string CheckPassword(string PhoneNumber, string OldPwd)
        {
            var postParas = new Dictionary<string, string>() 
                { 
                    {"PhoneNumber",PhoneNumber},
                    {"OldPwd",OldPwd}
                };

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var response = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());
            var fresult = JsonHelper.FromJsonTo<ResponseResult<UpdateUserInput>>(response);
            if (fresult.IsSuccess)
            {
                return "true";
            }

            return "false";
        }
        #endregion

        #region "修改密码"
        /// <summary>
        /// 修改密码界面
        /// </summary>
        /// <returns></returns>
        public ActionResult SavePwd()
        {
            var user = GetLoginInfo();
            if (user == null)
                return RedirectToAction("Login");

            UpdateUserInput modelUser = new UpdateUserInput()
            {
                Id = user.User.Id,
                PhoneNumber = user.User.UserName
            };
            return View(modelUser);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="updateuserinfo"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]       
        public ActionResult SavePwd(UpdateUserInput updateuserinfo, Int64 Id)
        {
            var postParas = new Dictionary<string, string>() 
            { 
                {"Id",updateuserinfo.Id.ToString()},
                {"ID",Id.ToString()},
                {"NewPwd",updateuserinfo.NewPwd},
                {"OldPwd",updateuserinfo.OldPwd},
                {"PhoneNumber",updateuserinfo.PhoneNumber},
                {"SavePwd",updateuserinfo.SavePwd}
            };

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            var fresult = PostStandardWithSameControllerAction<UpdateUserInput>(this, postParas);
            if (!fresult.IsSuccess && string.IsNullOrEmpty(fresult.Message))
            {
                return View();
            }
            if (!fresult.IsSuccess)
            {
                TempData["Error"] = fresult.Message;
            }
            else
            {

                TempData["Message"] = fresult.Message;
                HttpContext.Items[DataKey.RemoveSessionCookie] = "yes";
                Session.RemoveAll();
            }
            return View();
        }
        #endregion

        #region "用户个人信息"
        /// <summary>
        /// 用户信息界面
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult UserInfo()
        {
            return View("~/Views/Shared/_UserInfo.cshtml", GetLoginInfo().User);
        }
        #endregion
    }
}