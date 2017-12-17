using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.WebApplication.api
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler,System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";            
            var LoginUserName = context.Request.Params["LoginUserName"];
            var Password = context.Request.Params["Password"];
            if (LoginUserName == "DuPontTest" && Password.Contains("dupont123"))
            {
               System.Web.HttpContext.Current.Application["num"] = 1; 
               
               System.Web.HttpContext.Current.Session["user"] = LoginUserName;
                context.Response.Write("ok");
            }
            else {
                context.Response.Write("no");
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}