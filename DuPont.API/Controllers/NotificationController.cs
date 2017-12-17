using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.API.Filters;
using DuPont.Models.Models;
using DuPont.Models.Dtos.Foreground.Notification;
using DuPont.Interface;
using DuPont.Global.ActionResults;
using System.Data.Linq.SqlClient;
using AutoMapper;
using DuPont.Extensions;
using DuPont.Global.Exceptions;
using EntityFramework.Extensions;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Enum;
using System.Web.Caching;


namespace DuPont.API.Controllers
{
#if(!DEBUG)
    [AccessAuthorize]
#endif
    public class NotificationController : Controller
    {
        private readonly INotification _notificationService;
        private readonly IVisitNotification _visitNotificationService;
        private readonly IUser _userService;
        private readonly ISendNotificationResult _sendNotificationResult;
        private readonly ISysSetting _systemSettingService;

        private const string getDataSuccessResult = "获取数据成功!";
        private const string getDataFailResult = "获取数据失败!";

        public NotificationController(
            INotification notificationService,
            IVisitNotification visitNotificationService,
            IUser userService,
            ISendNotificationResult sendNotificationResult,
            ISysSetting systemSettingService
            )
        {
            _notificationService = notificationService;
            _visitNotificationService = visitNotificationService;
            _userService = userService;
            _sendNotificationResult = sendNotificationResult;
            _systemSettingService = systemSettingService;
        }

        #region "Android轮询接口"
        [HttpPost]       
        public JsonResult MyMessage(MyMessageInput input)
        {
            using (var result = new ResponseResult<MyMessageOutput>())
            {
                var myMessage = new MyMessageOutput();
                result.Entity = myMessage;
                result.Message = getDataSuccessResult;
                var predicate = PredicateBuilder.True<T_NOTIFICATION>();
                var curDate = DateTime.Now;
                var todayStart = new DateTime(curDate.Year, curDate.Month, curDate.Day);
                var todayEnd = new DateTime(curDate.Year, curDate.Month, curDate.Day, 23, 59, 59);

                Cache cache = ControllerContext.HttpContext.Cache;

                if (cache[DataKey.NotificationPollingFrequency] == null)
                {
                    var notificationPoolingSetting = _systemSettingService.GetAll(p => p.SETTING_ID == DataKey.NotificationPollingFrequency).FirstOrDefault();
                    int poolMinutes = 1;
                    if (notificationPoolingSetting == null || !int.TryParse(notificationPoolingSetting.SETTING_VALUE, out poolMinutes))
                    {
                        result.Entity.Interval = 1;
                    }
                    else
                    {
                        result.Entity.Interval = poolMinutes;
                    }

                    cache.Insert(DataKey.NotificationPollingFrequency, result.Entity.Interval, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                }
                else
                {
                    result.Entity.Interval = int.Parse(Convert.ToString(cache[DataKey.NotificationPollingFrequency]) ?? "1");
                }

                var predicateCommon = predicate.And(m => !m.IsDeleted && !m.IsOnDate && m.CreateTime >= todayStart && m.CreateTime <= todayEnd);
                //公共的推送消息
                var predicateForPublic = predicateCommon.And(m => m.IsPublic);

                //先按用户来查询
                if (input.UserId.HasValue)
                {
                    //个人的推送消息
                    var predicateForPersonal = predicateCommon.And(m => !m.IsPublic && m.TargetUserId == input.UserId.Value);
                    //获取今天已接收的公开推送消息
                    var receivedNotificationList = _sendNotificationResult.GetAll(p => p.UserId == input.UserId);
                    var receivedNotificationIdList = receivedNotificationList.Select(p => p.MsgId).ToList();
                    //获取今天还可以接收的公开推送消息
                    var validPublicNotificationListPredicate = predicateCommon.And(p => !receivedNotificationIdList.Contains(p.MsgId)).And(p => p.IsPublic);
                    var validPublicNotificationList = _notificationService.GetAll(validPublicNotificationListPredicate);
                    var myAllNotificationList = new List<T_NOTIFICATION>();

                    #region 公共消息
                    if (validPublicNotificationList != null && validPublicNotificationList.Any())
                    {
                        myAllNotificationList.AddRange(validPublicNotificationList);

                        //标识已经发送的公开消息
                        var haveSentPublicNotificationList = new List<T_SEND_NOTIFICATION_RESULT>();
                        foreach (var notification in validPublicNotificationList)
                        {
                            haveSentPublicNotificationList.Add(new T_SEND_NOTIFICATION_RESULT
                            {
                                MsgId = notification.MsgId,
                                SendTime = DateTime.Now,
                                UserId = input.UserId.Value
                            });
                        }

                        _sendNotificationResult.Insert(haveSentPublicNotificationList);
                    }
                    #endregion

                    #region 个人消息
                    //获取针对个人的推送消息列表
                    var personalNotificationList = _notificationService.GetAll(predicateForPersonal);

                    if (personalNotificationList != null && personalNotificationList.Count() > 0)
                    {
                        myAllNotificationList.AddRange(personalNotificationList);

                        personalNotificationList.Select(m => m.IsDeleted = true).Count();

                        //更新个人推送消息的状态
                        _notificationService.Update(personalNotificationList);
                    }
                    #endregion

                    result.Entity.MsgList = Mapper.Map<List<MessageItem>>(myAllNotificationList);

                    return new JsonResultEx(result);
                }


                //按设备标识来查询(只能获取公共消息)
                var publicNotifications = _notificationService.GetAll(predicateForPublic);
                if (publicNotifications == null || !publicNotifications.Any())
                    return new JsonResultEx(result);

                var haveSentNotification = _visitNotificationService.GetAll(m => m.SendTime >= todayStart && m.SendTime <= todayEnd && m.OsType == input.OsType && m.DeviceToken == input.DeviceToken);
                if (haveSentNotification == null || !haveSentNotification.Any())
                {
                    var haveSentNotificationIdArray = haveSentNotification.Select(m => m.MsgId).ToArray();
                    var validNotification = publicNotifications.Except(publicNotifications.Where(m => haveSentNotificationIdArray.Contains(m.MsgId)));
                    result.Entity.MsgList = Mapper.Map<List<MessageItem>>(validNotification);
                    //标识已经推送过了
                    if (validNotification != null && validNotification.Any())
                    {
                        var haveSentNotifications = new List<T_VISITOR_RECEIVED_NOTIFICATION>();
                        foreach (var personalNotification in validNotification)
                        {
                            var notification = new T_VISITOR_RECEIVED_NOTIFICATION
                            {
                                MsgId = personalNotification.MsgId,
                                DeviceToken = input.DeviceToken,
                                SendTime = DateTime.Now,
                                OsType = input.OsType
                            };

                            haveSentNotifications.Add(notification);
                        }

                        _visitNotificationService.Insert(haveSentNotifications);
                        //删除过时的消息
                        _visitNotificationService.Delete(m => m.SendTime < todayStart);
                    }
                }
                return new JsonResultEx(result);
            }
        }

        [HttpPost]       
        public JsonResult NotificationMessage(MyMessageInput input)
        {
            using (var result = new ResponseResult<MyMessageOutput>())
            {
                var myMessage = new MyMessageOutput();
                result.Entity = myMessage;
                result.Message = getDataSuccessResult;
                var predicate = PredicateBuilder.True<T_NOTIFICATION>();
                var curDate = DateTime.Now;
                var todayStart = new DateTime(curDate.Year, curDate.Month, curDate.Day);
                var todayEnd = new DateTime(curDate.Year, curDate.Month, curDate.Day, 23, 59, 59);

                Cache cache = ControllerContext.HttpContext.Cache;

                if (cache[DataKey.NotificationPollingFrequency] == null)
                {
                    var notificationPoolingSetting = _systemSettingService.GetAll(p => p.SETTING_ID == DataKey.NotificationPollingFrequency).FirstOrDefault();
                    int poolMinutes = 1;
                    if (notificationPoolingSetting == null || !int.TryParse(notificationPoolingSetting.SETTING_VALUE, out poolMinutes))
                    {
                        result.Entity.Interval = 1;
                    }
                    else
                    {
                        result.Entity.Interval = poolMinutes;
                    }

                    cache.Insert(DataKey.NotificationPollingFrequency, result.Entity.Interval, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
                }
                else
                {
                    result.Entity.Interval = int.Parse(Convert.ToString(cache[DataKey.NotificationPollingFrequency]) ?? "1");
                }

                var predicateCommon = predicate.And(m => !m.IsDeleted && !m.IsOnDate && m.CreateTime >= todayStart && m.CreateTime <= todayEnd);
                //公共的推送消息
                var predicateForPublic = predicateCommon.And(m => m.IsPublic);

                //先按用户来查询
                if (input.UserId.HasValue)
                {
                    //个人的推送消息
                    var predicateForPersonal = predicateCommon.And(m => !m.IsPublic && m.TargetUserId == input.UserId.Value);
                    //获取今天已接收的公开推送消息
                    var receivedNotificationList = _sendNotificationResult.GetAll(p => p.UserId == input.UserId);
                    var receivedNotificationIdList = receivedNotificationList.Select(p => p.MsgId).ToList();
                    //获取今天还可以接收的公开推送消息
                    var validPublicNotificationListPredicate = predicateCommon.And(p => !receivedNotificationIdList.Contains(p.MsgId)).And(p => p.IsPublic);
                    var validPublicNotificationList = _notificationService.GetAll(validPublicNotificationListPredicate);
                    var myAllNotificationList = new List<T_NOTIFICATION>();

                    #region 公共消息
                    if (validPublicNotificationList != null && validPublicNotificationList.Any())
                    {
                        myAllNotificationList.AddRange(validPublicNotificationList);

                        //标识已经发送的公开消息
                        var haveSentPublicNotificationList = new List<T_SEND_NOTIFICATION_RESULT>();
                        foreach (var notification in validPublicNotificationList)
                        {
                            haveSentPublicNotificationList.Add(new T_SEND_NOTIFICATION_RESULT
                            {
                                MsgId = notification.MsgId,
                                SendTime = DateTime.Now,
                                UserId = input.UserId.Value
                            });
                        }

                        _sendNotificationResult.Insert(haveSentPublicNotificationList);
                    }
                    #endregion

                    #region 个人消息
                    //获取针对个人的推送消息列表
                    var personalNotificationList = _notificationService.GetAll(predicateForPersonal);

                    if (personalNotificationList != null && personalNotificationList.Count() > 0)
                    {
                        myAllNotificationList.AddRange(personalNotificationList);

                        personalNotificationList.Select(m => m.IsDeleted = true).Count();

                        //更新个人推送消息的状态
                        _notificationService.Update(personalNotificationList);
                    }
                    #endregion

                    result.Entity.MsgList = Mapper.Map<List<MessageItem>>(myAllNotificationList);

                    return new JsonResultEx(result);
                }


                //按设备标识来查询(只能获取公共消息)
                var publicNotifications = _notificationService.GetAll(predicateForPublic);
                if (publicNotifications == null || !publicNotifications.Any())
                    return new JsonResultEx(result);

                var haveSentNotification = _visitNotificationService.GetAll(m => m.SendTime >= todayStart && m.SendTime <= todayEnd && m.OsType == input.OsType && m.DeviceToken == input.DeviceToken);
                if (haveSentNotification == null || !haveSentNotification.Any())
                {
                    var haveSentNotificationIdArray = haveSentNotification.Select(m => m.MsgId).ToArray();
                    var validNotification = publicNotifications.Except(publicNotifications.Where(m => haveSentNotificationIdArray.Contains(m.MsgId)));
                    result.Entity.MsgList = Mapper.Map<List<MessageItem>>(validNotification);
                    //标识已经推送过了
                    if (validNotification != null && validNotification.Any())
                    {
                        var haveSentNotifications = new List<T_VISITOR_RECEIVED_NOTIFICATION>();
                        foreach (var personalNotification in validNotification)
                        {
                            var notification = new T_VISITOR_RECEIVED_NOTIFICATION
                            {
                                MsgId = personalNotification.MsgId,
                                DeviceToken = input.DeviceToken,
                                SendTime = DateTime.Now,
                                OsType = input.OsType
                            };

                            haveSentNotifications.Add(notification);
                        }

                        _visitNotificationService.Insert(haveSentNotifications);
                        //删除过时的消息
                        _visitNotificationService.Delete(m => m.SendTime < todayStart);
                    }
                }
                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "IOS端传送DeviceToken接口"
        [HttpPost]
        public JsonResult RegisterToken(RegisterTokenInput input)
        {
            using (var result = new ResponseResult<object>())
            {
                if (!ModelState.IsValid)
                {
                    result.Message = getDataFailResult;
                    result.IsSuccess = false;
                    return new JsonResultEx(result);
                }

                result.Message = getDataSuccessResult;
                var user = _userService.GetByKey(input.UserId);
                if (user == null)
                    throw new CustomException("用户不存在!");

                if (user.IsDeleted)
                    throw new CustomException("账号已被锁定!");

                //删除同名的DeviceToken
                _notificationService.RemoveSameDeviceToken(input.DeviceToken);

                user.IosDeviceToken = input.DeviceToken;
                result.IsSuccess = _userService.Update(user) > 0;
                result.Message = result.IsSuccess ? getDataSuccessResult : getDataFailResult;
                return new JsonResultEx(result);
            }
        }
        #endregion
    }
}