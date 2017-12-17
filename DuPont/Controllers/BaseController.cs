using DuPont.Attributes;
using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Global.ActionResults;
using DuPont.Interface;
using DuPont.Models;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Web.Mvc;

namespace DuPont.Controllers
{
    public class BaseController : Controller
    {
        protected IPermissionProvider permissionProvider;
        protected IAdminUser adminUserRepository;
        private T_ADMIN_USER _userInfo;
        public BaseController(IPermissionProvider permissionProvider)
        {
            this.permissionProvider = permissionProvider;
        }

        public BaseController(IPermissionProvider permissionProvider, IAdminUser adminUserRepository)
        {
            this.permissionProvider = permissionProvider;
            this.adminUserRepository = adminUserRepository;
        }

        #region "登陆者用户信息"
        protected T_ADMIN_USER UserInfo
        {
            get
            {
                if (adminUserRepository == null)
                    throw new ArgumentNullException("adminUserRepository");

                if (_userInfo == null)
                {
                    _userInfo = adminUserRepository.GetByKey(UserId);
                }

                return _userInfo;
            }
        }
        #endregion

        #region "登录者的用户编号"
        private Int64 _userId;
        /// <summary>
        /// 登录者用户编号
        /// </summary>
        protected Int64 UserId
        {
            get
            {
                if (_userId != 0)
                {
                    return _userId;
                }
                var str_UserId = Request[DataKey.UserId];
                Int64 userId;

                //参数空异常
                if (string.IsNullOrEmpty(str_UserId))
                    throw new ArgumentNullException(DataKey.UserId);

                //参数数据异常
                if (Int64.TryParse(str_UserId.Split(',')[0], out userId) == false)
                    throw new ArgumentException(DataKey.UserId);

                _userId = userId;
                return _userId;
            }
        }
        #endregion

        #region "当前请求的地址"
        /// <summary>
        /// 当前访问地址
        /// </summary>
        protected string CurrentUrl
        {
            get
            {
                return Request.Url.LocalPath;
            }
        }
        #endregion

        #region "登出当前会话"
        /// <summary>
        /// 登出
        /// </summary>
        protected void LoginOut()
        {
            Session.Clear();
        }
        #endregion

        #region "检查用户访问权限"
        /// <summary>
        /// 检查用户访问权限
        /// </summary>
        protected void CheckPermission()
        {


            var havePermission = this.permissionProvider.HaveAuthority(UserId, CurrentUrl);
            if (!havePermission)
            {
                throw new UnauthorizedAccessException();
            }
        }
        #endregion

        #region "获取权限检查的结果"
        /// <summary>
        /// 获取权限检查的结果
        /// </summary>
        /// <returns></returns>
        protected ActionResult GetCheckPermissionResult()
        {
            CheckPermission();
            using (var result = new ResponseResult<object>())
            {
                result.IsSuccess = true;
                return Json(result);
            }
        }
        #endregion

        #region "设置Json响应体"
        protected void SetJosnResult<TEntity>(ResponseResult<TEntity> result, int pageIndex, int pageSize, long totalNums, string message) where TEntity : class,new()
        {
            result.TotalNums = totalNums;
            result.PageIndex = pageIndex;
            result.PageSize = pageSize;
            result.Message = message;
        }
        #endregion

        public JsonResult ResponseErrorWithJson<TEntity>(ResponseResult<TEntity> result, string message) where TEntity : class,new()
        {
            result.IsSuccess = false;
            result.Message = message;
            return new JsonResultEx(result);
        }

        protected JsonResult ResponseSuccessWithJson<TEntity>(ResponseResult<TEntity> result, string message) where TEntity : class,new()
        {
            result.IsSuccess = true;
            result.Message = message;
            return new JsonResultEx(result);
        }
    }
}