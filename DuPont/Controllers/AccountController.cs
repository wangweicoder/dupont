using DuPont.Attributes;




using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using DuPont.Models;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Web.Mvc;
using System.Linq;
using DuPont.Models.Dtos.Background;
using DuPont.Models.Enum;
using DuPont.Models.Models;

namespace DuPont.Controllers
{
    [CustomHandleErrorWithErrorJson]
    [Validate]
    public class AccountController : BaseController
    {
        private IAuthProvider authProvider;
        private IAdminUser _adminUserService;
        private IUser_Password_History _userPasswordHistory;
        private ISysSetting _sysSetting;
        public AccountController(IPermissionProvider permissionManage,
            IAuthProvider authProvider, IAdminUser Iuser,
            IUser_Password_History userPasswordHistory, ISysSetting sysSetting)
            : base(permissionManage)
        {
            this.authProvider = authProvider;
            this._adminUserService = Iuser;
            this._userPasswordHistory = userPasswordHistory;
            this._sysSetting = sysSetting;
        }

        #region "登录后台"
        [HttpPost]
        public JsonResult Login(LoginInputDto input, string returnUrl)
        {
            using (ResponseResult<AdminUserLoginInfo> result = new ResponseResult<AdminUserLoginInfo>())
            {
                input.Password = Server.HtmlDecode(input.Password);
                var adminUser = this.authProvider.Authenticate(input);
                //消除敏感数据
                adminUser.User.Password = string.Empty;
                result.Entity = adminUser;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "修改密码"
        [HttpPost]
        public JsonResult SavePwd(DuPont.Models.UpdateUserInfo updateuserinfo, Int64 Id)
        {
            using (var result = new ResponseResult<UpdateUserInfo>())
            {
                //获取用户信息
                var user = _adminUserService.GetAll(m => m.UserName == updateuserinfo.PhoneNumber);
                if (user == null || user.Count() == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "用户不存在!";
                    return Json(result);
                }

                var adminUser = user.ElementAt(0);
                updateuserinfo.OldPwd = Encrypt.MD5Encrypt(updateuserinfo.OldPwd);

                //校验输入密码与原密码是否一致
                if (adminUser.Password != updateuserinfo.OldPwd)
                {
                    result.IsSuccess = false;
                    result.Message = "输入的密码与原密码不一致!";
                }
                //密码复杂度校验
                else if (!PageValidate.IsSafePassword(updateuserinfo.NewPwd))
                {
                    result.IsSuccess = false;
                    result.Message = "密码必须包含字母、数字、特殊符号,且字母包含大小写长度在(7-18)";
                }
                else
                {
                    adminUser.Password = Encrypt.MD5Encrypt(updateuserinfo.NewPwd);
                    var isSuccess = _adminUserService.Update(adminUser) > 0;
                    if (isSuccess)
                    {
                        result.Message = "密码修改成功!";
                    }
                }
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "检查原密码"
        public JsonResult CheckPassword(string PhoneNumber, string OldPwd)
        {
            using (var result = new ResponseResult<object>())
            {
                //获取用户信息
                var user = _adminUserService.GetAll(m => m.UserName == PhoneNumber);
                if (user == null || user.Count() == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "用户不存在!";
                    return Json(result);
                }

                var adminUser = user.ElementAt(0);
                var hashedPwd = Encrypt.MD5Encrypt(OldPwd);

                //校验输入密码与原密码是否一致
                if (adminUser.Password != hashedPwd)
                {
                    result.IsSuccess = false;
                    result.Message = "输入的密码与原密码不一致!";
                }

                return new JsonResultEx(result);
            }
        }
        #endregion

        [ChildActionOnly]
        public new ActionResult UserInfo()
        {
            return View("~/Views/Shared/_UserInfo.cshtml", Session[DataKey.UserInfo] as T_USER);
        }

        #region "退出登录"
        public new ActionResult LoginOut()
        {
            base.LoginOut();

            return RedirectToAction("Login");
        }
        #endregion
    }
}