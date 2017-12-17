using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace DuPont.WebApplication
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);  
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("Server");

            if (Response.Cookies.Count > 0)
            {
                foreach (string s in Response.Cookies.AllKeys)
                {
                    if (s == FormsAuthentication.FormsCookieName || s.ToLower() == "asp.net_sessionid")
                    {
                        Response.Cookies[s].Secure = true;
                    }
                }
            }
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var lastError = Server.GetLastError();

            if (lastError != null)
            {
                var httpError = lastError as HttpException;
                if (httpError != null)
                {
                    switch (httpError.ErrorCode)
                    {
                        case 500:
                            using (var result = new ResponseResult<object>())
                            {
                                result.Message = "参数异常! bmp-ae";
                                result.IsSuccess = false;
                                //Response.Write(JsonHelper.ToJsJson(result));
                                Server.ClearError();
                                return;
                            }
                        case 404:
                        default:
                            using (var result = new ResponseResult<object>())
                            {
                                result.Message = "地址不存在! bmp-ae";
                                result.IsSuccess = false;
                                //Response.Write(JsonHelper.ToJsJson(result));
                                Server.ClearError();
                                return;
                            }
                    }
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }
        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}