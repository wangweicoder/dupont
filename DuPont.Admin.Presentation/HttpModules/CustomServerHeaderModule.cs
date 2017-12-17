using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Admin.Presentation.HttpModules
{
    /// <summary>
    /// 自定义Response Http Header 模块
    /// </summary>
    public class CustomServerHeaderModule : IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            // remove the "Server" Http Header
            HttpContext.Current.Response.Headers.Remove("Server");
        }
    }
}