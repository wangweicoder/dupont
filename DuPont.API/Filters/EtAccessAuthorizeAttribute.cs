

using DuPont.Interface;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Extensions;

using DuPont.API.Infrastructure;
using Ninject;
using DuPont.Models.Models;
using DuPont.Entity.Enum;
using System.Configuration;
using DuPont.Models.Enum;
using System.Text;

namespace DuPont.API.Filters
{
    public class EtAccessAuthorizeAttribute : ActionFilterAttribute
    {
        //private readonly IUser repository = NinjectDependencyResolver.Kernel.Get<IUser>();
        private readonly IUserToken repository = NinjectDependencyResolver.Kernel.Get<IUserToken>();
        public EtAccessAuthorizeAttribute()
        {
        }
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
                    return authorizedFailedMessage ?? "用户验证失败！";
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
        /// <summary>
        /// 字符串编码转换
        /// </summary>
        /// <param name="srcEncoding">原编码</param>
        /// <param name="dstEncoding">目标编码</param>
        /// <param name="srcBytes">原字符串</param>
        /// <returns>字符串</returns>
        public static string TransferEncoding(Encoding srcEncoding, Encoding dstEncoding, string srcStr)
        {
            byte[] srcBytes = srcEncoding.GetBytes(srcStr);
            byte[] bytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes);
            return dstEncoding.GetString(bytes);

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string strController = filterContext.RouteData.Values["controller"].ToString();
            string strAction = filterContext.RouteData.Values["action"].ToString();

            //接口协议验证
            string keyString = string.Empty;
            IEnumerable<string> keys = null;

            keys = filterContext.HttpContext.Request.Form.AllKeys.Where(k =>
           {
               var lowerKey = k.ToLower();
               var isNotEncryptString = lowerKey != "encrypt";
               var isNotGUserId = lowerKey != "guserid";
               return isNotEncryptString && isNotGUserId;
           }).OrderBy(k => k).ToList();

            if (keys.Count() == 0)
            {
                var actionResult = new AccessFailResult();
                actionResult.AuthorizedFailedMessage = ResponseStatusCode.BadRequest.GetDescription();
                actionResult.responseResult.State.Id = (int)ResponseStatusCode.BadRequest;
                actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                filterContext.Result = actionResult;
            }
            else
            {
                var formKeys = filterContext.HttpContext.Request.Form.AllKeys;
                var isMissParameter = false;
                var missParameterErrorMessage = string.Empty;                
                if (!formKeys.Contains("Token"))
                {
                    missParameterErrorMessage = "参数Token不能为空!";
                    isMissParameter = true;
                }
                else if (!formKeys.Contains("encrypt"))
                {
                    missParameterErrorMessage = "参数encrypt不能为空!";
                    isMissParameter = true;
                }
                else if (!formKeys.Contains("cur_time"))
                {
                    missParameterErrorMessage = "参数cur_time不能为空!";
                    isMissParameter = true;
                }

                if (isMissParameter)
                {
                    var actionResult = new AccessFailResult();
                    actionResult.AuthorizedFailedMessage = missParameterErrorMessage;
                    actionResult.responseResult.State.Id = (int)ResponseStatusCode.NotLogin;
                    actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                    filterContext.Result = actionResult;
                    return;
                }
                //登陆者id
                long guserid = Convert.ToInt64(filterContext.HttpContext.Request.Form["GUserId"] ?? "0");
                foreach (var key in keys)
                {
                    if (key == "Token")
                    {
                        string token = filterContext.HttpContext.Request.Form[key];
                        var user = repository.GetByToken(token);

                        //判断当前用户是否登录
                        if (user == null || string.IsNullOrEmpty(user.Token))
                        {
                            var actionResult = new AccessFailResult();
                            actionResult.AuthorizedFailedMessage = ResponseStatusCode.NotLogin.GetDescription();
                            actionResult.responseResult.State.Id = (int)ResponseStatusCode.NotLogin;
                            actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                            filterContext.Result = actionResult;
                            return;
                        }
                        else if (user.IsDeleted)
                        {
                            var actionResult = new AccessFailResult();
                            actionResult.AuthorizedFailedMessage = ResponseStatusCode.UserIsLock.GetDescription();
                            actionResult.responseResult.State.Id = (int)ResponseStatusCode.UserIsLock;
                            actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                            filterContext.Result = actionResult;
                            return;
                        }

                        //检测E田调用接口登录获得Token的时间--在webconfig中配置了10分钟
                        int validLoginDays = int.Parse(ConfigHelper.GetAppSetting(DataKey.ValidLoginToken));
                        if (!user.LastLoginTime.HasValue || (DateTime.Now - user.LastLoginTime.Value).Minutes > validLoginDays)
                        {
                            //提示用户重新登录
                            var actionResult = new AccessFailResult();
                            actionResult.AuthorizedFailedMessage = ResponseStatusCode.PleaseReLogin.GetDescription();
                            actionResult.responseResult.State.Id = (int)ResponseStatusCode.PleaseReLogin;
                            actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                            filterContext.Result = actionResult;                           
                            return;
                        }
                        keyString += user.Token;
                    }
                    else
                    {
                        var specialstr = System.Configuration.ConfigurationManager.AppSettings["specialstr"];
                        if(!specialstr.Contains(key))
                        keyString += filterContext.HttpContext.Request.Form[key];
                    }
                }
                var privateKey = System.Configuration.ConfigurationManager.AppSettings["encryptKey"];
                keyString += privateKey;
                keyString = keyString.ToLower();
                keyString = TransferEncoding(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, keyString);
                string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "安全验证加密对比";
                IOHelper.WriteLogToFile(logErrstring + "加密前：" + keyString, filterContext.HttpContext.Server.MapPath("~/App_Data/Log") + @"\DuPontRequestEtLog");
                //对服务器端token做加密处理
                var encryptedAuthorizedStr = new Encrypt().SHA256_Encrypt(keyString);
                //获取客户端token
                var encrypt = filterContext.HttpContext.Request.Form["encrypt"];
                IOHelper.WriteLogToFile(logErrstring + "对比前：" + encrypt + "\r\n" + encryptedAuthorizedStr, filterContext.HttpContext.Server.MapPath("~/App_Data/Log") + @"\DuPontRequestEtLog");
                //将服务端token和客户端token进行比较
                bool isEquals = encryptedAuthorizedStr.Equals(encrypt, StringComparison.CurrentCultureIgnoreCase);
                if (isEquals == false)
                {
                    IOHelper.WriteLogToFile("将服务端token和客户端token进行比较:" + encrypt + "\r\n" + encryptedAuthorizedStr, filterContext.HttpContext.Server.MapPath("~/App_Data/Log") + @"\DuPontRequestEtLog");
                    var actionResult = new AccessFailResult();
                    actionResult.AuthorizedFailedMessage = ResponseStatusCode.BadRequest.GetDescription();
                    actionResult.responseResult.State.Id = (int)ResponseStatusCode.BadRequest;
                    actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                    filterContext.Result = actionResult;
                }
            }
        }
    }
}