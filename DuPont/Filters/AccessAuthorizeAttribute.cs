
using DuPont.Infrastructure;
using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using Ninject;
using System;
using System.Web.Mvc;

namespace DuPont.Filters
{
    public class AccessAuthorizeAttribute : ActionFilterAttribute
    {
        private IPermission _permissionManage;
        public AccessAuthorizeAttribute()
        {
            if (_permissionManage == null)
            {
                IKernel kerNel = NinjectDependencyResolver.Kernel;
                _permissionManage = kerNel.Get<IPermission>();
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                string authorityUrl = filterContext.HttpContext.Request.Url.LocalPath;
                Int64 userId = Convert.ToInt64(filterContext.ActionParameters["userId"]);
                var havePermission = _permissionManage.HaveAuthority(userId, authorityUrl);
                if (havePermission == false)
                {
                    //未授权的访问
                    filterContext.Result = new AccessFailResult();
                }
            }
            catch (Exception)
            {
                var accessFailResult = new AccessFailResult();
                accessFailResult.AuthorizedFailedMessage = "系统处理异常!";
                filterContext.Result = accessFailResult;
            }

           // base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 未被授权的访问
        /// </summary>
        private class AccessFailResult : ActionResult
        {
            private string authorizedFailedMessage;
            public ResponseResult<object> responseResult;
            public AccessFailResult()
            {
                this.responseResult = new ResponseResult<object>();
                this.responseResult.State = new StateInfo();
            }

            public string AuthorizedFailedMessage
            {
                get
                {
                    return authorizedFailedMessage ?? "拒绝访问！";
                }
                set
                {
                    authorizedFailedMessage = value;
                }
            }
            public override void ExecuteResult(ControllerContext context)
            {
                using (responseResult)
                {
                    responseResult.IsSuccess = false;
                    responseResult.Message = AuthorizedFailedMessage;
                    context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
                    context.HttpContext.Response.Write(JsonHelper.ToJsJson(responseResult));
                }
            }
        }


    }
}