

using DuPont.Interface;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Extensions;

using DuPont.API.Infrastructure;
using Ninject;
using DuPont.Models.Models;
using DuPont.Entity.Enum;
using System.Configuration;
using DuPont.Models.Enum;

namespace DuPont.API.Filters
{
    public class AccessAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly IUser repository = NinjectDependencyResolver.Kernel.Get<IUser>();
        public AccessAuthorizeAttribute()
        {
        }
        private class AccessFailResult : ActionResult
        {
            private string authorizedFailedMessage;
            public ResponseResult<object> responseResult;
            public AccessFailResult()
            {
                this.responseResult = new ResponseResult<object>();
                this.responseResult.State = new StateInfo();
            }
            public string AuthorizedFailedMessage
            {
                get
                {
                    return authorizedFailedMessage ?? "用户验证失败！";
                }
                set
                {
                    authorizedFailedMessage = value;
                }
            }
            public override void ExecuteResult(ControllerContext context)
            {
                using (responseResult)
                {
                    responseResult.IsSuccess = false;
                    responseResult.Message = AuthorizedFailedMessage;
                    context.HttpContext.Response.ContentType = "application/json;charset=utf-8";
                    context.HttpContext.Response.Write(JsonHelper.ToJsJson(responseResult));
                }
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string strController = filterContext.RouteData.Values["controller"].ToString();
            string strAction = filterContext.RouteData.Values["action"].ToString();
            //免验证url数组
            string[] NotValidUrlArray = { "/account/register", 
                                            "/account/login",
                                            "/account/logout",
                                            "/account/sendcode", 
                                            "/account/validcode",
                                            "/account/updpas",
                                            "/common/getvalidatecode",
                                            "/common/appversion",
                                            "/common/checkvalidatecodestate",
                                            "/common/savevalidatecodesendrecord",
                                            "/common/carouselpictures",
                                            "/common/check_cas_displaystate",
                                            "/notification/mymessage",
                                            "/notification/registertoken",
                                            "/account/sociallogin",
                                            "/farmerrequirement/publishedforoperatorandbusiness", //ww增加未登录的大农户发布给产业商和农机手的需求列表
                                            "/business/publishedforfarmerbytime", //ww增加未登录产业商发布给大农户的需求列表
                                            //"/common/getdictionaryitems",//ww增加未登录根据父节点获取子节点字典数据
                                            //"/farmerrequirement/requirementdetail", //ww增加未登录查看需求详情
                                            "/question/list", //ww增加未登录查看专业咨询列表
                                            "/question/carouselpictures", //ww增加未登录查看问题列表轮播图接口
                                            "/question/detail", //ww增加未登录查看问题详情
                                            "/question/replylist", //ww增加未登录回复列表                                            
                                            "/notification/notificationmessage", //ww增加推送
                                            "/farmerrequirement/returnordermodel", //ww增加返回订单整理后的数据（E田需要的）
                                            "/operator/acceptorder",//ww增加返回更新订单数据（E田需要的）
                                            "/operator/replyfarmerrequirement",//ww增加靠谱作业农机手接订单数据（E田需要的）
                                            "/operator/etcommentrequirement",//ww增加农机手对大农户评价（E田调用时）
                                            "/account/gettoken",//ww增加E田登录返回token                                             
                                            "/farmerrequirement/commentorderforoperator",//ww返回靠谱作业农机手数据（传E田使用）
                                            "/account/updatefarmerrequirementstate", //定时任务
                                            "/account/updatebusinessrequirementstate",//定时任务
                                            "/operator/cancelfarmerrequirement",//ww增加靠谱作业农机手取消订单数据（E田需要的）
                                            "/common/getmodlist",//ww 增加玉米列表
                                            "/common/getmoddatalist",//ww 增加玉米价格表
                                            "/common/getcornprices",//ww 增加保存玉米价格json文件
                                            "/common/getmodpriceurl",//ww 增加玉米获取数据抓取地址
                                        };
            string urlstring = @"/" + strController + "/" + strAction;

            //免过滤列表
            if (NotValidUrlArray.Contains(urlstring.ToLower()) ||
                urlstring.ToLower() == "/common/checkdeployment" ||
                urlstring.ToLower() == "/common/charu" ||
                urlstring.ToLower().StartsWith("/learningworld/"))
            {
                return;
            }


            //接口协议验证
            string keyString = string.Empty;
            IEnumerable<string> keys = null;

            //上传图片接口特殊处理
            if (urlstring.ToLower() == "/common/uploadpicture")
            {
                keys = filterContext.HttpContext.Request.Form.AllKeys
                    .Where(k => k != "encrypt"
                        && k != "GUserId"
                        && k != "Pic"
                        && k != "Path"
                    ).OrderBy(k => k).ToList();
            }
            else
            {
                keys = filterContext.HttpContext.Request.Form.AllKeys.Where(k =>
               {
                   var lowerKey = k.ToLower();
                   var isNotEncryptString = lowerKey != "encrypt";
                   var isNotGUserId = lowerKey != "guserid";
                   return isNotEncryptString && isNotGUserId;
               }).OrderBy(k => k).ToList();
            }


            if (keys.Count() == 0)
            {
                var actionResult = new AccessFailResult();
                actionResult.AuthorizedFailedMessage = ResponseStatusCode.BadRequest.GetDescription();
                actionResult.responseResult.State.Id = (int)ResponseStatusCode.BadRequest;
                actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                filterContext.Result = actionResult;
            }
            else
            {
                var formKeys = filterContext.HttpContext.Request.Form.AllKeys;
                var isMissParameter = false;
                var missParameterErrorMessage = string.Empty;
                ////对必要的验证参数进行判断
                //if (!formKeys.Contains("GUserId"))
                //{
                //    missParameterErrorMessage = "参数GUserId不能为空!";
                //    isMissParameter = true;
                //}
                //else
                if (!formKeys.Contains("Token"))
                {
                    missParameterErrorMessage = "参数Token不能为空!";
                    isMissParameter = true;
                }
                else if (!formKeys.Contains("encrypt"))
                {
                    missParameterErrorMessage = "参数encrypt不能为空!";
                    isMissParameter = true;
                }
                else if (!formKeys.Contains("cur_time"))
                {
                    missParameterErrorMessage = "参数cur_time不能为空!";
                    isMissParameter = true;
                }

                if (isMissParameter)
                {
                    var actionResult = new AccessFailResult();
                    actionResult.AuthorizedFailedMessage = missParameterErrorMessage;
                    actionResult.responseResult.State.Id = (int)ResponseStatusCode.NotLogin;
                    actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                    filterContext.Result = actionResult;
                    return;
                }



                //登陆者id
                long guserid = Convert.ToInt64(filterContext.HttpContext.Request.Form["GUserId"] ?? "0");
                foreach (var key in keys)
                {
                    if (key == "Token" && guserid != 0 && guserid != -1)
                    {
                        var user = repository.GetByKey(guserid);

                        //判断当前用户是否登录
                        if (user == null || string.IsNullOrEmpty(user.LoginToken))
                        {
                            var actionResult = new AccessFailResult();
                            actionResult.AuthorizedFailedMessage = ResponseStatusCode.NotLogin.GetDescription();
                            actionResult.responseResult.State.Id = (int)ResponseStatusCode.NotLogin;
                            actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                            filterContext.Result = actionResult;
                            return;
                        }
                        else if (user.IsDeleted)
                        {
                            var actionResult = new AccessFailResult();
                            actionResult.AuthorizedFailedMessage = ResponseStatusCode.UserIsLock.GetDescription();
                            actionResult.responseResult.State.Id = (int)ResponseStatusCode.UserIsLock;
                            actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                            filterContext.Result = actionResult;
                            return;
                        }

                        //[已废弃]检测用户的登录时间--2016-03-07
                        //int validLoginDays = int.Parse(ConfigHelper.GetAppSetting(DataKey.SaveValidLoginDays));
                        //if (!user.LastLoginTime.HasValue || (DateTime.Now - user.LastLoginTime.Value).TotalDays > validLoginDays)
                        //{
                        //    //提示用户重新登录
                        //    var actionResult = new AccessFailResult();
                        //    actionResult.AuthorizedFailedMessage = ResponseStatusCode.PleaseReLogin.GetDescription();
                        //    actionResult.responseResult.State.Id = (int)ResponseStatusCode.PleaseReLogin;
                        //    actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                        //    filterContext.Result = actionResult;
                        //    return;
                        //}

                        //只对手机号码注册用户检测密码设定有效时间
                        if (user.Type == 0)
                        {
                            int validPasswordDays = int.Parse(ConfigHelper.GetAppSetting(DataKey.SaveValidUserPasswordDays));

                            //检查用户的密码有效时间
                            if (!user.LastUpdatePwdTime.HasValue || (DateTime.Now - user.LastUpdatePwdTime.Value).TotalDays > validPasswordDays)
                            {
                                //提示用户修改密码
                                var actionResult = new AccessFailResult();
                                actionResult.AuthorizedFailedMessage = ResponseStatusCode.PleaseUpdatePassword.GetDescription();
                                actionResult.responseResult.State.Id = (int)ResponseStatusCode.PleaseUpdatePassword;
                                actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                                filterContext.Result = actionResult;
                                return;
                            }
                        }

                        keyString += user.LoginToken;
                    }
                    else
                    {
                        keyString += filterContext.HttpContext.Request.Form[key];
                    }
                }
                var privateKey = System.Configuration.ConfigurationManager.AppSettings["encryptKey"];
                keyString += privateKey;
                keyString = keyString.ToLower();
                //对服务器端token做加密处理
                var encryptedAuthorizedStr = Encrypt.MD5EncryptWithoutKey(keyString);
                //获取客户端token
                var encrypt = filterContext.HttpContext.Request.Form["encrypt"];
                //将服务端token和客户端token进行比较
                bool isEquals = encryptedAuthorizedStr.Equals(encrypt, StringComparison.CurrentCultureIgnoreCase);
                if (isEquals == false)
                {
                    var actionResult = new AccessFailResult();
                    actionResult.AuthorizedFailedMessage = ResponseStatusCode.LoginoutByOtherDevice.GetDescription();
                    actionResult.responseResult.State.Id = (int)ResponseStatusCode.LoginoutByOtherDevice;
                    actionResult.responseResult.State.Description = actionResult.AuthorizedFailedMessage;
                    filterContext.Result = actionResult;
                }
            }
        }
    }
}