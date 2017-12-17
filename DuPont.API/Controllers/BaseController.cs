using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DuPont.Extensions;
using System.Configuration;
using DuPont.Models.Enum;
using Newtonsoft.Json;
using DuPont.API.Models.Account;

namespace DuPont.API.Controllers
{
    public class BaseController : Controller
    {
        private readonly string paramers = System.Configuration.ConfigurationManager.AppSettings["specialstr"];
        #region "设置Json响应体"
        protected void SetJosnResult<TEntity>(ResponseResult<TEntity> result, int pageIndex, int pageSize, long totalNums, string message) where TEntity : class,new()
        {
            result.TotalNums = totalNums;
            result.PageIndex = pageIndex;
            result.PageSize = pageSize;
            result.Message = message;
        }

        public JsonResult ResponseErrorWithJson<TEntity>(ResponseResult<TEntity> result, string message) where TEntity : class,new()
        {
            result.IsSuccess = false;
            result.Message = message;
            return Json(result);
        }

        protected JsonResult ResponseSuccessWithJson<TEntity>(ResponseResult<TEntity> result, string message) where TEntity : class,new()
        {
            result.IsSuccess = true;
            result.Message = message;
            return Json(result);
        }

        protected string GetModelStateErrorMessage(ModelStateDictionary modelState)
        {
            var errorMessageBuilder = new StringBuilder();
            var errorMessage = string.Empty;
            modelState.Values.Select(value => value.Errors.Select(error =>
            {
                if (!error.ErrorMessage.IsNullOrEmpty())
                {
                    errorMessageBuilder.AppendLine(error.ErrorMessage);
                }
                else
                {
                    errorMessageBuilder.AppendLine(ExceptionHelper.Build(error.Exception));
                }

                return error;
            }).Count()).Count();

            errorMessage = errorMessageBuilder.ToString();
            if (errorMessage.EndsWith("\r\n"))
            {
                errorMessage = errorMessage.TrimEnd("\r\n".ToCharArray());
            }
            return errorMessage;
        }

        #endregion
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
            return m.Token;
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
        
    }
}