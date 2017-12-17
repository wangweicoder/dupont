using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Filters
{
    public class HandleErrorEx : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Server.ClearError();
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (!filterContext.IsChildAction && (!filterContext.ExceptionHandled && filterContext.HttpContext.IsCustomErrorEnabled))
            {
                Exception innerException = filterContext.Exception;
                if ((new HttpException(null, innerException).GetHttpCode() == 500) && this.ExceptionType.IsInstanceOfType(innerException))
                {
                    string controllerName = (string)filterContext.RouteData.Values["controller"];
                    string actionName = (string)filterContext.RouteData.Values["action"];
                    HandleErrorInfo model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
                    ViewResult result = new ViewResult
                    {
                        ViewName = this.View,
                        MasterName = this.Master,
                        ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                        TempData = filterContext.Controller.TempData
                    };
                    filterContext.Result = result;
                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.Clear();
                    filterContext.HttpContext.Response.StatusCode = 200;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
            }
        }
    }
}