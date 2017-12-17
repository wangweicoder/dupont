using AutoMapper;
using DuPont.Global.ActionResults;
using DuPont.Interface;
using DuPont.Models.Dtos.Background.Notification;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotification _notificationService;
        private readonly IPublicNotification _publicNotificationService;
        private readonly IPersonalNotification _personalNotificationService;
        private readonly IUser _userService;
        private const string getDataSuccessResult = "获取数据成功!";
        public NotificationController(INotification notificationService,
            IPublicNotification publicNotificationService,
            IPersonalNotification personalNotificationService,
            IUser userService
            )
        {
            _notificationService = notificationService;
            _publicNotificationService = publicNotificationService;
            _personalNotificationService = personalNotificationService;
            _userService = userService;
        }

        #region "是否有公共的通知没有发送"
        [HttpPost]
        public JsonResult ExistsPublicNotification()
        {
            using (var result = new ResponseResult<object>())
            {
                bool isExist = _notificationService.ExistsPublicNotification();
                result.Message = getDataSuccessResult;
                result.Entity = isExist;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "创建公共推送任务"
        [HttpPost]
        public JsonResult CreatePublicNotificationTask()
        {
            using (var result = new ResponseResult<CreatePublicNotificationTaskOutput>())
            {
                var notification = _notificationService.GetOneValidPublicNotification();

                if (notification != null)
                {
                    //判断该条通和的推送任务有没有被创建
                    if (_publicNotificationService.Count(m => m.MsgId == notification.MsgId) == 0)
                    {
                        var model = new T_SEND_COMMON_NOTIFICATION_PROGRESS()
                        {
                            CreateTaskTime = DateTime.Now,
                            LastMaxUserId = 0,
                            MsgId = notification.MsgId,
                            SendTotalCount = 0
                        };
                        _publicNotificationService.Insert(model);
                    }
                    result.Entity = Mapper.Map<CreatePublicNotificationTaskOutput>(notification);
                }

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "获取公共通知的目标用户"
        [HttpPost]
        public JsonResult PublicNotificationUserList(long msgId, int userCount)
        {
            using (var result = new ResponseResult<List<PublicNotificationUserListOutput>>())
            {
                result.Message = getDataSuccessResult;

                var notification = _publicNotificationService.GetAll(m => m.MsgId == msgId).FirstOrDefault();
                if (notification == null)
                {
                    return new JsonResultEx(result);
                }

                long totalCount;
                var users = _userService.GetAll<long>(m => m.Id > notification.LastMaxUserId, m => m.Id, null, 1, userCount, out totalCount);
                if (users == null || !users.Any())
                {
                    return new JsonResultEx(result);
                }

                //获取实际需要推送的用户（过滤已经推送过的）
                users = _publicNotificationService.GetPublicNotificationValidUser(users, msgId);
                result.Entity = Mapper.Map<List<PublicNotificationUserListOutput>>(users);
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "更新公共通知推送进度"
        [HttpPost]
        public JsonResult UpdatePublicNotificationTaskProgress(long msgId, long lastMaxUserId, long addCount)
        {
            using (var result = new ResponseResult<object>())
            {
                var notificationTask = _publicNotificationService.GetByKey(msgId);
                notificationTask.LastMaxUserId = lastMaxUserId;
                notificationTask.SendTotalCount += addCount;
                _publicNotificationService.Update(notificationTask);
                result.Entity = notificationTask.SendTotalCount;
                result.Message = getDataSuccessResult;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "获取个人待推送的消息列表"
        [HttpPost]
        public JsonResult PersonalNotificationList(int usersCount)
        {
            using (var result = new ResponseResult<List<PersonalNotificationListOutput>>())
            {
                result.Message = getDataSuccessResult;

                long totalCount;
                var notifications = _notificationService.GetAll<long>(m => !m.IsPublic && !m.IsDeleted && !m.IsOnDate
                    && m.TargetUserId.HasValue && m.TargetUser.IosDeviceToken != "" && m.TargetUser.IosDeviceToken != null, m => m.MsgId, null, 1, usersCount, out totalCount, "TargetUser");
                if (notifications == null || !notifications.Any())
                    return new JsonResultEx(result);

                result.Entity = Mapper.Map<List<PersonalNotificationListOutput>>(notifications);
                return new JsonResultEx(result);
            }
        }
        #endregion


        #region "更新个人通知的发送状态"
        [HttpPost]
        public JsonResult UpadtePersonalNotification(string msgIdList)
        {
            using (var result = new ResponseResult<object>())
            {
                result.Message = getDataSuccessResult;
                var msgStringIdArary = msgIdList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var msgLongIdArray = new long[msgStringIdArary.Length];
                for (int i = 0; i < msgLongIdArray.Length; i++)
                {
                    msgLongIdArray[i] = int.Parse(msgStringIdArary[i]);
                }

                var notificationList = _notificationService.GetAll(m => msgLongIdArray.Contains(m.MsgId));
                if (notificationList != null)
                {
                    foreach (var notification in notificationList)
                    {
                        notification.IsDeleted = true;
                    }
                }

                var effectRows = _notificationService.Update(notificationList);
                result.Entity = effectRows > 0;
                return new JsonResultEx(result);
            }
        }
        #endregion

    }
}