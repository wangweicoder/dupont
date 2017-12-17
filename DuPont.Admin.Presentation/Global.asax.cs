using DuPont.Admin.Presentation.Infrastructure;
using DuPont.Models.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace DuPont.Admin.Presentation
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
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("Server");

            var NeedCleanCookie = !string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Items[DataKey.RemoveSessionCookie]));

            if (_isMvcRequest && Request.Cookies.Count > 0)
            {
                if (NeedCleanCookie)
                {
                    foreach (var key in Request.Cookies.AllKeys)
                    {
                        var newCookie = Response.Cookies[key];
                        newCookie.Value = null;
                        newCookie.Expires = DateTime.UtcNow.AddDays(-3);
                    }
                    HttpContext.Current.Items.Clear();
                }
                else
                {
                    foreach (var key in Request.Cookies.AllKeys)
                    {
                        var cookie = Request.Cookies[key];
                        Response.Cookies[key].HttpOnly = true;
                        Response.Cookies[key].Value = cookie.Value;
                        if (Request.Url.Scheme == "https" && cookie.Secure == false)
                        {
                            Response.Cookies[key].Secure = true;
                        }
                    }
                }
            }
            else
            {
                //Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                //var section = config.GetSection("system.web/sessionState");
                //var sessionSection = section as SessionStateSection;
                //var cookie = Response.Cookies[sessionSection.CookieName];
                //if (cookie != null)
                //{
                //    Response.Cookies.Remove(cookie.Name);
                //}
                Response.Headers.Remove("Set-Cookie");
            }

            if (NeedCleanCookie && (Request.Url.LocalPath.ToLower() == "/account/login" || Request.Url.LocalPath.ToLower() == "/"))
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                var section = config.GetSection("system.web/sessionState");
                var sessionSection = section as SessionStateSection;
                var cookie = Response.Cookies[sessionSection.CookieName];
                if (cookie != null)
                {
                    cookie.Value = null;
                    cookie.Expires = DateTime.UtcNow.AddDays(-3);
                }
            }
        }

        protected void Application_Error(Object sender, EventArgs e)
        {

        }

        private bool _isMvcRequest
        {
            get
            {
                return !Request.Url.LocalPath.ToLower().StartsWith("/custompage/") && System.IO.Path.GetExtension(Request.Url.LocalPath) == "";
            }
        }

    }
}