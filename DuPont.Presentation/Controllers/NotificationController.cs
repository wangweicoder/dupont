using DuPont.Global.ActionResults;
using DuPont.Models.Dtos.Foreground.Notification;
using DuPont.Extensions;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Global.Exceptions;

namespace DuPont.Presentation.Controllers
{

    public class NotificationController : BaseController
    {

        #region "Android轮询接口"
        [HttpPost]
        public JsonResult MyMessage(MyMessageInput input)
        {
            string errorMessage = null;
            if (!CheckInputWhenReturnActionResult(ref errorMessage))
            {
                using (var result = new ResponseResult<object>())
                {
                    result.Message = errorMessage;
                    result.IsSuccess = false;
                    return new JsonResultEx(result);
                }
            }

            var parameters = ModelHelper.GetPropertyDictionary<MyMessageInput>(input);
            using (var responseResult = PostStandardWithSameControllerAction<MyMessageOutput>(this, parameters))
            {
                //if (responseResult.IsSuccess)
                //{
                //    var interval = 0;

                //    int.TryParse(ConfigurationManager.AppSettings[DataKey.AndroidPollingFrequency], out interval);
                //    if (interval == 0)
                //        interval = 1;

                //    responseResult.Entity.Interval = interval;
                //}
                return new JsonResultEx(responseResult);
            }
        }
        [HttpPost]
        public JsonResult NotificationMessage(MyMessageInput input)
        {
            string errorMessage = null;
            if (!CheckInputWhenReturnActionResult(ref errorMessage))
            {
                using (var result = new ResponseResult<object>())
                {
                    result.Message = errorMessage;
                    result.IsSuccess = false;
                    return new JsonResultEx(result);
                }
            }

            var parameters = ModelHelper.GetPropertyDictionary<MyMessageInput>(input);
            using (var responseResult = PostStandardWithSameControllerAction<MyMessageOutput>(this, parameters))
            {
                if (responseResult.IsSuccess)
                {
                    var articleBaseHost = ConfigHelper.GetAppSetting(DataKey.ArticleStaticPageBasePath);
                    foreach(var item in responseResult.Entity.MsgList)
                    {
                        if (item.NotificationType == 1)//说明是文章
                        {
                            item.NotificationSource = articleBaseHost+item.NotificationSource;
                        }
                    }
                }
                return new JsonResultEx(responseResult);
            }
        }
        #endregion

        #region "IOS端传送DeviceToken接口"
        [HttpPost]
        public JsonResult RegisterToken(RegisterTokenInput input)
        {
            string errorMessage = null;
            if (!CheckInputWhenReturnActionResult(ref errorMessage))
            {
                using (var result = new ResponseResult<object>())
                {
                    result.Message = errorMessage;
                    result.IsSuccess = false;
                    return new JsonResultEx(result);
                }
            }

            var parameters = ModelHelper.GetPropertyDictionary<RegisterTokenInput>(input);
            using (var responseResult = PostStandardWithSameControllerAction<object>(this, parameters))
            {
                return new JsonResultEx(responseResult);
            }
        }
        #endregion
    }
}
