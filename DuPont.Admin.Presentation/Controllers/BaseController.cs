
using DuPont.Admin.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Text;
using System.Web.Configuration;

using DuPont.Utility.LogModule.Model;
using DuPont.Utility;

using DuPont.Extensions;
using DuPont.Global.Exceptions;
using DuPont.Models.Models;
using DuPont.Models.Enum;

namespace DuPont.Admin.Presentation.Controllers
{
    public class BaseController : Controller
    {
        private static readonly object cacheLock = new object();
        //后台Api服务器地址
        protected static readonly string bgApiServerUrl = ConfigurationManager.AppSettings[DataKey.RemoteBackgroundApi];

        protected Controller ChildController;

        /// <summary>
        /// 登陆者登录信息
        /// </summary>
        /// <returns></returns>
        protected AdminUserLoginInfo GetLoginInfo()
        {                       
            var loginInfo = Session[DataKey.UserInfo] as AdminUserLoginInfo;

            if (loginInfo == null && !Request.IsAjaxRequest())
            {
                Response.Redirect("~/Account/Login");
                Response.End();
                return new AdminUserLoginInfo();
            }
            return loginInfo;
        }

        protected void LoginOut()
        {
            Session.Clear();
        }

        protected string CurrentUrl
        {
            get
            {
                return Request.Url.LocalPath;
            }
        }

        /// <summary>
        /// 获取证书密码
        /// </summary>
        /// <returns>System.String.</returns>
        protected string GetCertificationPwd()
        {
            return ConfigurationManager.AppSettings[DataKey.CertificatePwd];
        }

        /// <summary>
        /// 获取证书地址
        /// </summary>
        /// <returns>System.String.</returns>
        protected string GetCertificationFilePath()
        {
            return Server.MapPath("~") + ConfigurationManager.AppSettings[DataKey.CertificateUrl];
        }

        public void SetJsonHeader()
        {
            Response.ContentType = "application/jsons";
            Response.ContentEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// 获取Session的Cookie名称
        /// </summary>
        public string SessionCookieName
        {
            get
            {

                string cacheKey = "CookieName";

                var cacheValue = HttpRuntime.Cache.Get(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue.ToString();
                }

                //线程同步处理
                lock (cacheLock)
                {
                    cacheValue = HttpRuntime.Cache.Get(cacheKey);
                    if (cacheValue != null)
                    {
                        return cacheValue.ToString();
                    }

                    if (cacheValue == null)
                    {
                        Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                        var section = config.GetSection("system.web/sessionState");

                        if (section != null)
                        {
                            var sessionStateSection = section as SessionStateSection;
                            string cookieName = sessionStateSection.CookieName;
                            if (!string.IsNullOrEmpty(cookieName))
                            {
                                HttpRuntime.Cache.Insert(cacheKey, cookieName);
                                return cookieName;
                            }
                        }
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="statusCode"></param>
        protected void ThrowException(int statusCode, string message, bool returnJsonWhenNoPermission = false)
        {
            switch (statusCode)
            {
                case -1003:
                    if (returnJsonWhenNoPermission)
                    {
                        this.HttpContext.Items["JsonErrorOutput"] = true;
                    }
                    throw new UnauthorizedAccessException(message);
                default:
                    throw new CustomException(message);
            }
        }

        protected void CheckPermission(Int64 userId, string url, bool returnJsonWhenNoPermission = false)
        {
            if (userId <= 0)
                throw new ArgumentException("userId");

            if (url.IsNullOrEmpty())
                throw new ArgumentNullException("url");


            var postUrl = bgApiServerUrl + "Permission/CheckPermission";
            var postParas = new Dictionary<string, string>();
            var certification = GetCertificationFilePath();//证书的路径
            var certificationPwd = GetCertificationPwd();//证书的密码
            postParas.Add("url", url);
            postParas.Add("userId", userId.ToString());

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, userId.ToString());
            }

            var responseString = HttpAsynchronousTool.CustomHttpWebRequestPost(postUrl, postParas, certification, certificationPwd);
            var responseObj = JsonHelper.FromJsonTo<ResponseResult<object>>(responseString);
            if (responseObj != null)
            {
                if (responseObj.IsSuccess == false)
                {
                    ThrowException(responseObj.State.Id, responseObj.Message, returnJsonWhenNoPermission);
                }
            }
        }

        protected Dictionary<string, string> GetPostParameters()
        {
            var postParas = new Dictionary<string, string>();
            foreach (var formKey in Request.Form.AllKeys)
            {
                var formValue = Request.Form[formKey];
                postParas.Add(formKey, formValue);
            }

            return postParas;
        }

        /// <summary>
        /// 获取请求的api地址
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        protected string GetCurrentUrl<TEntity>(TEntity type)
            where TEntity : Controller
        {
            return bgApiServerUrl + type.GetType().Name.Replace("Controller", "") + "/" + Convert.ToString(RouteData.Values["action"]);
        }

        /// <summary>
        /// 检查用户输入
        /// </summary>
        /// <returns></returns>
        protected bool CheckInputWhenReturnActionResult()
        {
            var inputIsValidResult = ModelState.IsValid;
            if (inputIsValidResult == false)
            {
                var errorMessageBuilder = new StringBuilder();
                var errorMessage = string.Empty;
                this.ModelState.Values.Select(value => value.Errors.Select(error => errorMessageBuilder.AppendLine(error.ErrorMessage)).Count()).Count();
                errorMessage = errorMessageBuilder.ToString();
                if (errorMessage.EndsWith("\r\n"))
                    errorMessage = errorMessage.Substring(0, errorMessage.Length - 2);
                this.TempData["Error"] = errorMessage.ToString();
            }

            return inputIsValidResult;
        }

        protected ResponseResult<TReceiveType> PostJsonWithSameControllerAction<TReceiveType, TParameterType>(Controller currentController, TParameterType parameterInstance)
            where TReceiveType : class,new()
            where TParameterType : class,new()
        {
            Dictionary<string, string> parameters = null;
            if (parameterInstance != null)
                parameters = ModelHelper.GetPropertyDictionary<TParameterType>(parameterInstance);

            if (parameters == null)
                parameters = new Dictionary<string, string>();

            if (!parameters.ContainsKey(DataKey.UserId))
            {
                parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }



            return RestSharpHelper.PostWithApplicationJson<ResponseResult<TReceiveType>>(GetCurrentUrl(currentController), parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        /// <summary>
        /// 访问远程服务器同名控制器的同名Action
        /// </summary>
        /// <typeparam name="TReceiveType"></typeparam>
        /// <typeparam name="TParameterType"></typeparam>
        /// <param name="currentController"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected ResponseResult<TReceiveType> PostJsonWithSameControllerAction<TReceiveType>(Controller currentController, Dictionary<string, string> parameters)
            where TReceiveType : class,new()
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            if (!parameters.ContainsKey(DataKey.UserId))
            {
                parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            return RestSharpHelper.PostWithApplicationJson<ResponseResult<TReceiveType>>(GetCurrentUrl(currentController), parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        /// <summary>
        /// 访问远程服务器同名控制器的同名Action
        /// </summary>
        /// <typeparam name="TReceiveType"></typeparam>
        /// <param name="currentController"></param>
        /// <returns></returns>
        protected ResponseResult<TReceiveType> PostJsonWithSameControllerAction<TReceiveType>(Controller currentController)
            where TReceiveType : class,new()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            return RestSharpHelper.PostWithApplicationJson<ResponseResult<TReceiveType>>(GetCurrentUrl(currentController), parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        protected ResponseResult<TReceiveType> PostJson<TReceiveType, TParameterType>(string remoteControllerName, string remoteActionName, TParameterType parameterInstance)
            where TReceiveType : class,new()
            where TParameterType : class,new()
        {
            if (string.IsNullOrEmpty(remoteControllerName))
                throw new ArgumentNullException("remoteControllerName");

            if (string.IsNullOrEmpty(remoteActionName))
                throw new ArgumentNullException("remoteActionName");

            var url = bgApiServerUrl + remoteControllerName + "/" + remoteActionName;
            var parameters = ModelHelper.GetPropertyDictionary<TParameterType>(parameterInstance);
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            if (!parameters.ContainsKey(DataKey.UserId))
            {
                parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            return RestSharpHelper.PostWithApplicationJson<ResponseResult<TReceiveType>>(
                url, parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        protected ResponseResult<TReceiveType> PostStandardWithSameControllerAction<TReceiveType, TParameterType>(Controller currentController, TParameterType parameterInstance)
            where TReceiveType : class,new()
            where TParameterType : class,new()
        {
            Dictionary<string, string> parameters = null;
            if (parameterInstance != null)
                parameters = ModelHelper.GetPropertyDictionary<TParameterType>(parameterInstance);

            if (parameters == null)
                parameters = new Dictionary<string, string>();

            if (!parameters.ContainsKey(DataKey.UserId))
            {
                parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }



            return RestSharpHelper.PostWithStandard<ResponseResult<TReceiveType>>(GetCurrentUrl(currentController), parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        /// <summary>
        /// 访问远程服务器同名控制器的同名Action
        /// </summary>
        /// <typeparam name="TReceiveType"></typeparam>
        /// <typeparam name="TParameterType"></typeparam>
        /// <param name="currentController"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected ResponseResult<TReceiveType> PostStandardWithSameControllerAction<TReceiveType>(Controller currentController, Dictionary<string, string> parameters)
            where TReceiveType : class,new()
        {
            if (parameters == null)
                parameters = new Dictionary<string, string>();
            var currentUrl = "/" + (currentController.RouteData.Values["controller"].ToString() + "/" + currentController.RouteData.Values["action"]).ToLower();
            if (!parameters.ContainsKey(DataKey.UserId) && currentUrl != "/account/login")
            {
                parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            return RestSharpHelper.PostWithStandard<ResponseResult<TReceiveType>>(GetCurrentUrl(currentController), parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        /// <summary>
        /// 访问远程服务器同名控制器的同名Action
        /// </summary>
        /// <typeparam name="TReceiveType"></typeparam>
        /// <param name="currentController"></param>
        /// <returns></returns>
        protected ResponseResult<TReceiveType> PostStandardWithSameControllerAction<TReceiveType>(Controller currentController)
            where TReceiveType : class,new()
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            return RestSharpHelper.PostWithStandard<ResponseResult<TReceiveType>>(GetCurrentUrl(currentController), parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        protected ResponseResult<TReceiveType> PostStandardWithSameControllerAction<TReceiveType, TParameterType>(string remoteControllerName, string remoteActionName, TParameterType parameterInstance)
            where TReceiveType : class,new()
            where TParameterType : class,new()
        {
            if (string.IsNullOrEmpty(remoteControllerName))
                throw new ArgumentNullException("remoteControllerName");

            if (string.IsNullOrEmpty(remoteActionName))
                throw new ArgumentNullException("remoteActionName");

            var url = bgApiServerUrl + remoteControllerName + "/" + remoteActionName;
            var parameters = ModelHelper.GetPropertyDictionary<TParameterType>(parameterInstance);
            if (parameters == null)
                parameters = new Dictionary<string, string>();

            if (!parameters.ContainsKey(DataKey.UserId))
            {
                parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            return RestSharpHelper.PostWithStandard<ResponseResult<TReceiveType>>(
                url, parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        protected ResponseResult<TReceiveType> PostStandardWithControllerAction<TReceiveType>(string remoteControllerName, string remoteActionName, Dictionary<string, string> parameters)
            where TReceiveType : class,new()
        {
            if (string.IsNullOrEmpty(remoteControllerName))
                throw new ArgumentNullException("remoteControllerName");

            if (string.IsNullOrEmpty(remoteActionName))
                throw new ArgumentNullException("remoteActionName");

            var url = bgApiServerUrl + remoteControllerName + "/" + remoteActionName;

            if (parameters == null)
                parameters = new Dictionary<string, string>();

            if (!parameters.ContainsKey(DataKey.UserId))
            {
                parameters.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            return RestSharpHelper.PostWithStandard<ResponseResult<TReceiveType>>(
                url, parameters, GetCertificationFilePath(), GetCertificationPwd());
        }
    }
}