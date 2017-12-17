// ***********************************************************************
// Assembly         : DuPont.Admin.Presentation
// Author           : 毛文君
// Created          : 12-04-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-04-2015
// ***********************************************************************
// <copyright file="CustomHandleErrorWithLogOnlyAttribute.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>适合前台页面使用</summary>
// ***********************************************************************


using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Global.Exceptions;
using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Attributes
{
    public class CustomHandleErrorWithLogOnlyAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = 200;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

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
                    return;
                }
            }

            //是否需要持久化异常数据
            var canPersistence = false;
            //获取当前登录者的用户信息
            var currentLoginUser = Global.Global.GetAdminLoginInfo();
            //获取当前控制器
            var currentController = (Controller)filterContext.Controller;

            //获取当前用户请求数据(不可直接记录本内容到数据库或本地磁盘)
            var requestBodyRawString = filterContext.HttpContext.Request.GetRequestBodyString();
            //加密请求数据(推荐)
            var requestBodyEncryptString = Encrypt.MD5Encrypt(requestBodyRawString);

            //构建日志实体
            var logEntry = new T_SYS_LOG
            {
                Level = "ERROR",
                CreateTime = DateTime.Now,
                RequestParameter = requestBodyEncryptString,
                Url = filterContext.HttpContext.Request.Path,
            };

            if (currentLoginUser != null)
            {
                logEntry.UserId = currentLoginUser.User.Id;
                logEntry.UserName = currentLoginUser.User.RealName ?? currentLoginUser.User.UserName;
            }

            filterContext.HttpContext.Response.Clear();

            if (filterContext.Exception is ArgumentException)
            {
                filterContext.HttpContext.Response.WriteErrorMessage(ResponseStatusCode.InvalidArgument.GetDescription());
                filterContext.HttpContext.Response.End();
            }
            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                if (filterContext.HttpContext.Items["JsonErrorOutput"] != null)
                {
                    new JsonResult { Data = new { success = false, responseText = "没有权限!" }, ContentType = null, ContentEncoding = null, JsonRequestBehavior = JsonRequestBehavior.DenyGet }.ExecuteResult(currentController.ControllerContext);
                    return;
                }
                else
                {
                    filterContext.HttpContext.Response.WriteErrorMessage(ResponseStatusCode.AccessDenied.GetDescription());
                    filterContext.HttpContext.Response.End();
                }
            }
            else if (filterContext.Exception is CustomException)
            {
                currentController.ModelState.AddModelError("", filterContext.Exception.Message);
                logEntry.Message = filterContext.Exception.Message;
                logEntry.StackTrace = filterContext.Exception.StackTrace;
            }
            else
            {
                //系统错误
                currentController.ModelState.AddModelError("", ResponseStatusCode.ApplicationError.GetDescription());

                logEntry.Message = filterContext.Exception.Message;
                logEntry.StackTrace = filterContext.Exception.StackTrace;

                canPersistence = true;//标记该错误需要被记下来
            }

            //标识异常已经被处理过
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult { ViewName = null, MasterName = null, ViewData = currentController.ViewData, TempData = currentController.TempData, ViewEngineCollection = currentController.ViewEngineCollection }; ;

            //filterContext.HttpContext.Response.End();

            #region 日志记录
            var logDateString = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n");
            var currentRequestUrl = "/" + filterContext.HttpContext.Request.HttpMethod + " " + filterContext.HttpContext.Request.Url + "\r\n";
            var logOnDiskContent = logDateString + currentRequestUrl + requestBodyEncryptString + "\r\nError:" + logEntry.Message + "\r\n";
            if (filterContext.Exception.InnerException != null)
            {
                logEntry.Message += "\r\n内部异常:" + filterContext.Exception.InnerException.Message + "\r\n";
            }
            //如果需要持久化,将日志分别记录到本地和数据库中(日志核心数据已加密)
            if (canPersistence)
            {
                //在网站本地生成日志文件
                IOHelper.WriteLogToFile(logOnDiskContent, filterContext.HttpContext.Server.MapPath("~/App_Data/Log"));
            }
            #endregion
        }
    }
}