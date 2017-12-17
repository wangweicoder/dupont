

using DuPont.API.Models.Account;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Presentation.Controllers
{
    public class BaseController : Controller
    {
        protected static readonly string bgApiServerUrl = ConfigHelper.GetAppSetting(DataKey.RemoteApiForRelease);
        private readonly string paramers = System.Configuration.ConfigurationManager.AppSettings["specialstr"];
        public Dictionary<string, string> GetPostParameters()
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
        /// 验证参数模型
        /// </summary>
        /// <returns></returns>
        public ResponseResult<Object> ValidateParamModel()
        {
            //验证参数
            if (!ModelState.IsValid)
            {
                using (var res = new ResponseResult<Object>())
                {
                    res.IsSuccess = false;
                    res.Message = "参数异常";
                    return res;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取请求的api地址
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        protected string GetCurrentUrl<TEntity>(TEntity type)
            where TEntity : Controller
        {
            return bgApiServerUrl + Request.FilePath.Substring(1);
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

            return RestSharpHelper.PostWithStandard<ResponseResult<TReceiveType>>(
                url, parameters, GetCertificationFilePath(), GetCertificationPwd());
        }

        protected ResponseResult<TReceiveType> PostStandardWithSameControllerAction<TReceiveType>(string remoteControllerName, string remoteActionName, Dictionary<string, string> parameters)
            where TReceiveType : class,new()
        {
            if (string.IsNullOrEmpty(remoteControllerName))
                throw new ArgumentNullException("remoteControllerName");

            if (string.IsNullOrEmpty(remoteActionName))
                throw new ArgumentNullException("remoteActionName");
            //本地调试时去掉api//  
            var url = bgApiServerUrl + "api/" + remoteControllerName + "/" + remoteActionName;

            if (parameters == null)
                parameters = new Dictionary<string, string>();


            return RestSharpHelper.PostWithStandard<ResponseResult<TReceiveType>>(
                url, parameters, GetCertificationFilePath(), GetCertificationPwd());
        }


        /// <summary>
        /// 检查用户输入
        /// </summary>
        /// <returns></returns>
        protected bool CheckInputWhenReturnActionResult(ref string errorMessage)
        {
            var inputIsValidResult = ModelState.IsValid;
            if (inputIsValidResult == false)
            {
                var errorMessageBuilder = new StringBuilder();

                this.ModelState.Values.Select(value => value.Errors.Select(error => errorMessageBuilder.AppendLine(error.ErrorMessage)).Count()).Count();
                errorMessage = errorMessageBuilder.ToString();
                if (errorMessage.EndsWith("\r\n"))
                    errorMessage = errorMessage.Substring(0, errorMessage.Length - 2);


            }

            return inputIsValidResult;
        }
        /// <summary>
        /// 返回E田Token
        /// </summary>
        /// <returns></returns>
        public string ReturnCustomToken()
        {
            Dictionary<string, string> content = new Dictionary<string, string>();
            string username = ConfigHelper.GetAppSetting("EtUserName");
            string password = ConfigHelper.GetAppSetting("EtPassword");
            content.Add("UserName", username);
            content.Add("Password", password);
            string Apiurl = ConfigHelper.GetAppSetting("EtApiUrl");
            string url = Apiurl + "/wei/work/dupont/login.jsp";
            var modlist = HttpAsynchronousTool.CustomHttpWebRequestPost(url, content, GetCertificationFilePath(), GetCertificationPwd());
            ResponseResult<TokenModel> token = JsonConvert.DeserializeObject<ResponseResult<TokenModel>>(modlist);
            TokenModel m = token.Entity;
            return m.token;
        }
        /// <summary>
        /// 加密要传递的参数
        /// </summary>
        /// <param name="etcontent"></param>
        /// <returns></returns>
        protected Dictionary<string, string> EncryptDictionary(Dictionary<string, string> etcontent)
        {
            try
            {
                etcontent.Add("Token", ReturnCustomToken());
                //排序
                var orurlcontent = etcontent.OrderBy(k => k.Key).ToList();
                //排序后放入的新集合
                Dictionary<string, string> rurlcontent = new Dictionary<string, string>();
                string keyString = null;
                foreach (var item in orurlcontent)
                {
                    rurlcontent.Add(item.Key, item.Value);
                    if (!paramers.Contains(item.Key))
                    {
                        keyString += item.Value;
                    }
                }
                var privateKey = System.Configuration.ConfigurationManager.AppSettings["ETencryptKey"];
                keyString += privateKey;
                keyString = keyString.ToLower();
                keyString = TransferEncoding(Encoding.GetEncoding("iso-8859-1"), Encoding.UTF8, keyString);
                //对服务器端token做加密处理
                var encryptedAuthorizedStr = new Encrypt().SHA256_Encrypt(keyString);
                rurlcontent.Add("Encrypt", encryptedAuthorizedStr);
                return rurlcontent;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, string>();
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
        /// <summary>
        /// 日志相对路径
        /// </summary>
        /// <returns></returns>
        protected string RelativePath()
        {
            return HttpContext.Server.MapPath("~/App_Data/Log");
        }
    }
}