
using DuPont.Models.Models;
using DuPont.Presentation.Infrastructure;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace DuPont.Presentation
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            DependencyResolver.SetResolver(new NinjectDependencyResolver());
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
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
                                Response.Write(JsonHelper.ToJsJson(result));
                                Server.ClearError();
                                return;
                            }
                        case 404:
                        default:
                            using (var result = new ResponseResult<object>())
                            {
                                result.Message = "地址不存在! bmp-ae";
                                result.IsSuccess = false;
                                Response.Write(JsonHelper.ToJsJson(result));
                                Server.ClearError();
                                return;
                            }
                    }
                }
            }
        }
    }
}