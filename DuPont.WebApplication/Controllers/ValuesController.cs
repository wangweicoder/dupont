using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.WebSockets;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using System.Text;


namespace DuPont.WebApplication.Controllers
{
    public class ValuesController :ApiController
    {
      
        //配置文件中的地址
        string bgApiServerUrl = ConfigHelper.GetAppSetting("RemotePresentationApi");
        #region 通用方法
        // GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        
        // GET api/<controller>/5
        /// <summary>
        /// 获得请求次数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Getnum(int id)
        {
            return System.Web.HttpContext.Current.Application["num"].ToString();
        }

        // POST api/<controller>
        public void Post(string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id,[FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public string Delete(int id)
        {
           var s=id;
           return s.ToString();
        }
        /// <summary>
        /// 获取证书密码
        /// </summary>
        /// <returns>System.String.</returns>
        protected string GetCertificationPwd()
        {
            return ConfigurationManager.AppSettings["CertificatePwd"];
        }

        /// <summary>
        /// 获取证书地址
        /// </summary>
        /// <returns>System.String.</returns>        
        protected string GetCertificationFilePath()
        {

            return System.Web.HttpContext.Current.Server.MapPath("~") + ConfigurationManager.AppSettings["CertificateUrl"];
        }
        /// <summary>
        /// 加密要传递的参数（先锋帮）
        /// </summary>
        /// <param name="etcontent"></param>
        /// <returns></returns>
        protected static Dictionary<string, string> EncryptDictionary(Dictionary<string, string> etcontent)
        {
            etcontent.Add("Token", "1");
            etcontent.Add("cur_time", "2");
            //排序
            var orurlcontent = etcontent.OrderBy(k => k.Key).ToList();
            //排序后放入的新集合
            Dictionary<string, string> rurlcontent = new Dictionary<string, string>();
            string keyString = null;
            foreach (var item in orurlcontent)
            {
                rurlcontent.Add(item.Key, item.Value);
                keyString += item.Value;
            }           
            var privateKey = System.Configuration.ConfigurationManager.AppSettings["encryptKey"];   
            keyString += privateKey;
            keyString = keyString.ToLower();
            //对服务器端token做加密处理
            var encryptedAuthorizedStr = Encrypt.MD5EncryptWithoutKey(keyString);
            rurlcontent.Add("GUserId","0");
            rurlcontent.Add("encrypt", encryptedAuthorizedStr);
            return rurlcontent;
        }
        /// <summary>
        /// 加密要传递的参数（E田用）
        /// </summary>
        /// <param name="etcontent"></param>
        /// <returns></returns>
        protected static Dictionary<string, string> EtEncryptDictionary(Dictionary<string, string> etcontent)
        {
            etcontent.Add("Token", "61E6D9C7A8F94C21A453FB36AD4698A7");
            etcontent.Add("cur_time", "1496372238826");
            //排序
            var orurlcontent = etcontent.OrderBy(k => k.Key).ToList();
            //排序后放入的新集合
            Dictionary<string, string> rurlcontent = new Dictionary<string, string>();
            string keyString = null;
            foreach (var item in orurlcontent)
            {
                rurlcontent.Add(item.Key, item.Value);
                keyString += item.Value;
            }
            var privateKey = System.Configuration.ConfigurationManager.AppSettings["encryptKey"];
            keyString += privateKey;
            keyString = keyString.ToLower();
            //对服务器端token做加密处理
            var encryptedAuthorizedStr = new Encrypt().SHA256_Encrypt(keyString);            
            rurlcontent.Add("encrypt", encryptedAuthorizedStr);
            return rurlcontent;
        }
        /// <summary>
        /// 处理字符串
        /// </summary>
        /// <param name="order"></param>
        /// <param name="certification"></param>
        /// <param name="certificationPwd"></param>
        /// <param name="content">字典数据</param>
        /// <returns></returns>
        private string HandleString(string order, out string certification, out string certificationPwd, out Dictionary<string, string> content)
        {
            order = order.Replace("\n", "");
            string[] orders = order.Split(';');
            //证书的路径
            certification = GetCertificationFilePath();
            //证书的密码
            certificationPwd = GetCertificationPwd();
            content = new Dictionary<string, string>();
            foreach (var item in orders)
            {
                string[] items = item.Split(':');
                if (items.Length > 1)
                    content.Add(items[0].ToString(), items[1]);
            }
            return order;
        }
        #endregion
        #region 接口测试
        [HttpGet]
        public string SaveRequirement(string order)
        {
            try
            {
                string certification;
                string certificationPwd;
                Dictionary<string, string> content;
                order = HandleString(order, out certification, out certificationPwd, out content);
                content = EncryptDictionary(content);
                string result = HttpAsynchronousTool.CustomHttpWebRequestPost(bgApiServerUrl + "FarmerRequirement/SaveRequirement", content, certification, certificationPwd);
                System.Web.HttpContext.Current.Application["num"] = (int)System.Web.HttpContext.Current.Application["num"] +1;
                
                return result;
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }
        
        /// <summary>
        /// 评价订单，更新e田的订单状态
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public string CommentRequirement(string order)
        {
            try
            {
                string certification;
                string certificationPwd;
                Dictionary<string, string> content;
                order = HandleString(order, out certification, out certificationPwd, out content);
                content = EncryptDictionary(content);
                string result = HttpAsynchronousTool.CustomHttpWebRequestPost(bgApiServerUrl + "Operator/CommentRequirement", content, certification, certificationPwd);
                System.Web.HttpContext.Current.Application["num"] = (int)System.Web.HttpContext.Current.Application["num"] +1;
                
                return result;
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }

        /// <summary>
        /// e田接订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public string ReplyFarmerRequirement(string order)
        {
            try
            {
                string certification;
                string certificationPwd;
                Dictionary<string, string> content;
                order = HandleString(order, out certification, out certificationPwd, out content);
                content = EtEncryptDictionary(content);
                string result = HttpAsynchronousTool.CustomHttpWebRequestPost(bgApiServerUrl + "Operator/ReplyFarmerRequirement", content, certification, certificationPwd);
                System.Web.HttpContext.Current.Application["num"] = (int)System.Web.HttpContext.Current.Application["num"] + 1;

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 先锋帮大农户评价靠谱作业的农机手
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public string CommentRequirementForOperator(string order)
        {
            try
            {
                string certification;
                string certificationPwd;
                Dictionary<string, string> content;
                order = HandleString(order, out certification, out certificationPwd, out content);
                content = EncryptDictionary(content);
                string result = HttpAsynchronousTool.CustomHttpWebRequestPost(bgApiServerUrl + "FarmerRequirement/CommentRequirementForOperator", content, certification, certificationPwd);
                System.Web.HttpContext.Current.Application["num"] = (int)System.Web.HttpContext.Current.Application["num"] + 1;

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 靠谱作业的农机手评价先锋帮大农户
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public string EtCommentRequirement(string order)
        {
            try
            {
                string certification;
                string certificationPwd;
                Dictionary<string, string> content;
                order = HandleString(order, out certification, out certificationPwd, out content);
                content = EtEncryptDictionary(content);
                string result = HttpAsynchronousTool.CustomHttpWebRequestPost(bgApiServerUrl + "Operator/CommentRequirement", content, certification, certificationPwd);
                System.Web.HttpContext.Current.Application["num"] = (int)System.Web.HttpContext.Current.Application["num"] + 1;

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// e田取消订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpGet]
        public string CancelFarmerRequirement(string order)
        {
            try
            {
                string certification;
                string certificationPwd;
                Dictionary<string, string> content;
                order = HandleString(order, out certification, out certificationPwd, out content);
                content = EtEncryptDictionary(content);
                string result = HttpAsynchronousTool.CustomHttpWebRequestPost(bgApiServerUrl + "Operator/CancelFarmerRequirement", content, certification, certificationPwd);
                System.Web.HttpContext.Current.Application["num"] = (int)System.Web.HttpContext.Current.Application["num"] + 1;

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion
        #region 登录登出
        public class Loginmodel
        {
            public string LoginUserName { get; set; }
            public string Password { get; set; }
        }
        [HttpPost]
        public string Login([FromBody] Loginmodel model) 
        {
            if (model.LoginUserName == "DuPontTest" && model.Password.Contains("dupont123"))
            {
                System.Web.HttpContext.Current.Application["num"] = 1;
                HttpCookie cookie = new HttpCookie("Login");
                cookie.Values["name"] = model.LoginUserName;
                cookie.Values["password"] = model.LoginUserName;
                cookie.Expires=DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Add(cookie);
                //设置缓存时间，10分钟            
                double Seconds = 10;
                System.Web.HttpContext.Current.Session["user"] = model.LoginUserName;
                System.Web.HttpContext.Current.Cache.Insert("user",model.LoginUserName,null,System.Web.Caching.Cache.NoAbsoluteExpiration,TimeSpan.FromMinutes(Seconds));
                return "ok";
            }
            else { 
                return "on";
            }
        
        }
        [HttpGet]
        public void LoginOut()
        {
           //System.Web.HttpContext.Current.Session.Clear();
           System.Web.HttpContext.Current.Application.Contents.Clear();
           System.Web.HttpContext.Current.Cache.Remove("user");           
        }

        #endregion
    }
}