

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DuPont.Extensions;
using DuPont.Global.ActionResults;
using DuPont.Entity.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using DuPont.Models.Enum;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace DuPont.Global.Filters.ActionFilters
{
    public class ValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentController = ((Controller)filterContext.Controller);
            if (!currentController.ModelState.IsValid)
            {           

                var errorMessageBuilder = new StringBuilder();
                var errorMessage = string.Empty;
                currentController.ModelState.Values.Select(value => value.Errors.Select(error =>
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
                #region 再次验证
                errorMessage = errorMessageBuilder.ToString().ToLower();
                    //.Replace("script", "");
                int sindex = errorMessage.IndexOf('<');
                int eindex = errorMessage.IndexOf('>');
                for (int i = 0; i < eindex - sindex; i += 5)
                {
                    errorMessage = errorMessage.Replace("script", "");
                }
                
                //errorMessage = errorMessageBuilder.ToString();
               
                #endregion
                if (errorMessage.EndsWith("\r\n"))
                {
                    errorMessage = errorMessage.TrimEnd("\r\n".ToCharArray());
                }

                if (currentController.Request.ContentType.Contains("/json") || currentController.Request.Url.ToString().ToLower().Contains("/api/") || currentController.ToString().ToLower().StartsWith("dupont.api") || currentController.ToString().ToLower().StartsWith("dupont.presentation") || currentController.ToString().ToLower().StartsWith("dupont.controllers"))
                {

                    var jsonResult = new JsonResultEx();
                    var responseResult = new ResponseResult<object>() { IsSuccess = false, Message = errorMessage };


                    responseResult.State.Id = (int)ResponseStatusCode.ExpectError;
                    responseResult.State.Description = ResponseStatusCode.ExpectError.GetDescription();
                    jsonResult.Data = responseResult;
                    filterContext.Result = jsonResult;
                }
                else
                {
                    currentController.TempData["Error"] = errorMessage;
                    filterContext.Result = new ViewResult { ViewName = null, MasterName = null, ViewData = currentController.ViewData, TempData = currentController.TempData, ViewEngineCollection = currentController.ViewEngineCollection };
                }               
            }
            foreach (var item in currentController.Request.Headers.AllKeys)
            {
                if (item.ToLower().Contains("referer"))
                {
                    var referer = currentController.Request.Headers.Get("Referer");
                    int sindex = referer.ToString().IndexOf("//");
                    int eindex = referer.ToString().IndexOf('/', sindex + 2);
                    string refererhost = null;
                    if (eindex != -1)
                    {
                        refererhost = referer.ToString().Substring(sindex + 2, eindex - (sindex + 2));
                    }
                    else {
                        refererhost = referer.ToString().Substring(sindex + 2);
                    }
                    if (refererhost != currentController.Request.Headers.Get("Host"))
                    {
                        currentController.TempData["Error"] += "禁止CSRF";
                        currentController.Response.StatusCode = 500;  
                        filterContext.Result = new ViewResult { ViewName = null, MasterName = null, ViewData = currentController.ViewData, TempData = currentController.TempData, ViewEngineCollection = currentController.ViewEngineCollection };
                        currentController.HttpContext.Items[DataKey.RemoveSessionCookie] = "yes";                        
                    }
                }
                else if (item.ToLower().Contains("x-forwarded-for"))
                {
                    currentController.Request.Headers.Set("X-Forwarded-For", "");
                    currentController.TempData["Error"] += "禁止XFF";
                    currentController.Response.StatusCode = 500;  
                    filterContext.Result = new ViewResult { ViewName = null, MasterName = null, ViewData = currentController.ViewData, TempData = currentController.TempData, ViewEngineCollection = currentController.ViewEngineCollection };
                    currentController.HttpContext.Items[DataKey.RemoveSessionCookie] = "yes";
                }

            }  
            if (!(currentController.Request.Url.LocalPath.ToLower() == "/account/login" || currentController.Request.Url.LocalPath.ToLower() == "/"))
            {
                NameValueCollection parames = new NameValueCollection();
                if (currentController.Request.Url.LocalPath.ToLower().IndexOf("form") == -1)
                {
                    parames.Add(currentController.Request.QueryString);
                }
                if (currentController.Request.Url.LocalPath.ToLower() != "/learngarden/addarticle")
                {
                    parames.Add(currentController.Request.Form); 
                }
                foreach (var item in parames)
                {
                    string inputparams = currentController.Request[item.ToString()].ToString();
                    if (!ValidateSqlStr(inputparams))
                    {
                        currentController.TempData["Error"] += "禁止脚本注入";
                        currentController.Response.StatusCode = 500;                     
                        filterContext.Result = new ViewResult { ViewName = null, MasterName = null, ViewData = currentController.ViewData, TempData = currentController.TempData, ViewEngineCollection = currentController.ViewEngineCollection };

                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
        public bool ValidateSqlStr(string Str)
        {

            bool ReturnValue = true;

            try
            {

                if (Str.Trim() != "")
                {

                    string SqlStr = @"exec|insert|select|delete|update|count|chr|mid|master|truncate|char|declare|drop|create|iframe|script|count(|chr|mid(|mid|master|truncate|char|char(|declare|and|or|'|--|trim|$";

                    string[] anySqlStr = SqlStr.Split('|');

                    foreach (string ss in anySqlStr)
                    {

                        if (Str.ToLower().IndexOf(ss) >= 0)
                        {

                            ReturnValue = false;

                            break;

                        }

                    }

                }

            }

            catch
            {

                ReturnValue = false;

            }

            return ReturnValue;

        }
    }
}
