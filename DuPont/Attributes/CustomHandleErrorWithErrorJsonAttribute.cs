﻿

using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Global.Exceptions;
using DuPont.Infrastructure;
using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace DuPont.Attributes
{
    public class CustomHandleErrorWithErrorJsonAttribute : HandleErrorAttribute
    {
        private ISysLog logRepository = (ISysLog)NinjectDependencyResolver.Kernel.GetService(typeof(ISysLog));
        public override void OnException(ExceptionContext filterContext)
        {
            var responseResult = new ResponseResult<object>();
            var canPersistence = false;

            //获取当前登录者的用户信息
            var currentLoginUser = Global.Global.GetAdminLoginInfo();

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

            try
            {
                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.StatusCode = 200;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                responseResult.IsSuccess = false;
                responseResult.State.Id = (int)ResponseStatusCode.ApplicationError;
                responseResult.Message = ResponseStatusCode.ApplicationError.GetDescription();

                filterContext.HttpContext.Response.Clear();
                throw filterContext.Exception;
            }
            catch (ArgumentException)
            {
                //参数错误
                responseResult.State.Id = (int)ResponseStatusCode.ExpectError;
                responseResult.State.Description = ResponseStatusCode.ExpectError.GetDescription();
                responseResult.Message = ResponseStatusCode.InvalidArgument.GetDescription();
            }
            catch (UnauthorizedAccessException)
            {
                //未被授权的访问
                responseResult.State.Id = (int)ResponseStatusCode.ExpectError;
                responseResult.State.Description = ResponseStatusCode.ExpectError.GetDescription();
                responseResult.Message = ResponseStatusCode.AccessDenied.GetDescription();
            }
            catch (EntityException ex)
            {
                //数据库异常
                responseResult.State.Id = (int)ResponseStatusCode.DbServerError;
                responseResult.State.Description = ResponseStatusCode.DbServerError.GetDescription();
                responseResult.Message = ResponseStatusCode.DbServerError.GetDescription();

                logEntry.Message = ExceptionHelper.Build(ex);
                logEntry.StackTrace = ex.StackTrace;

                canPersistence = true;//标记该错误需要被记下来
            }
            catch (CustomException ex)
            {
                //预期异常
                responseResult.State.Id = (int)ResponseStatusCode.ExpectError;
                responseResult.State.Description = ResponseStatusCode.ExpectError.GetDescription();
                responseResult.Message = ex.Message;

                logEntry.Message = ExceptionHelper.Build(ex);
                logEntry.StackTrace = ex.StackTrace;

            }
            catch (Exception ex)
            {
                if (ex is SqlException && ex.Message.Contains("FK_"))
                {
                    responseResult.Message = "该数据条目下有关联数据,已终止操作!";
                    responseResult.State.Id = (int)ResponseStatusCode.ExpectError;
                    responseResult.State.Description = ResponseStatusCode.ExpectError.GetDescription();
                }
                else
                {
                    //系统错误
                    responseResult.State.Id = (int)ResponseStatusCode.ApplicationError;
                    responseResult.State.Description = ResponseStatusCode.ApplicationError.GetDescription();
                    responseResult.Message = ResponseStatusCode.ApplicationError.GetDescription();

                    logEntry.Message = ExceptionHelper.Build(ex);
                    logEntry.StackTrace = ex.StackTrace;

                    canPersistence = true;//标记该错误需要被记下来
                }
            }
            finally
            {
                filterContext.HttpContext.Response.ContentType = "application/json;charset=utf-8";
                filterContext.HttpContext.Response.Write(JsonHelper.ToJsJson(responseResult));
            }

            filterContext.HttpContext.Response.End();

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
                //记录日志到数据库
                logRepository.Insert(logEntry);
            }
            #endregion
        }
    }
}