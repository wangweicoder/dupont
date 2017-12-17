using DuPont.Admin.Presentation.Models;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Filters
{
    public class SysAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var loginInfo = httpContext.Session[DataKey.UserInfo] as AdminUserLoginInfo;
            if (loginInfo==null)
            {
                httpContext.Response.Redirect("~/Account/Login");
            }
            return loginInfo != null;
        }
    }
}