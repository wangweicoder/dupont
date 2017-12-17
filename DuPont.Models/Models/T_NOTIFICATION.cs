using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class T_NOTIFICATION
    {
        public T_NOTIFICATION()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
            IsOnDate = false;
            SendPrivateNotifications = new List<T_SEND_NOTIFICATION_RESULT>();
            SendVisitorNotifications = new List<T_VISITOR_RECEIVED_NOTIFICATION>();
        }

        /// <summary>
        /// 消息编号
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 是否是公开消息
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContent { get; set; }

        /// <summary>
        /// 消息推送目标用户编号
        /// </summary>
        public long? TargetUserId { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 消息是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 在预定时间推送
        /// </summary>
        public bool IsOnDate { get; set; }

        /// <summary>
        /// 预定时间
        /// </summary>
        public DateTime? SendOnDate { get; set; }
        /// <summary>
        /// 消息推送的类型（文章 1、回答 2、响应3）
        /// </summary>
        /// <author>ww</author>
        public int? NotificationType { get; set; }
        /// <summary>
        /// 消息推送来源（文章地址）
        /// </summary>
        /// <author>ww</author>
        public string NotificationSource { get; set; }
        /// <summary>
        /// 消息推送来源（文章编号、需求编号、回答问题编号）
        /// </summary>
        /// <author>ww</author>
        public long NotificationSourceId { get; set; }
        /// <summary>
        /// 问题是否公开（为前端跳转详情页面用）
        /// </summary>
        /// <author>ww</author>
        public  bool IsOpen { get; set; } 
        /// <summary>
        /// 消息推送目标用户
        /// </summary>
        public virtual T_USER TargetUser { get; set; }

        public virtual T_SEND_COMMON_NOTIFICATION_PROGRESS SendPublicNotificationProgressInfo { get; set; }
        public virtual List<T_SEND_NOTIFICATION_RESULT> SendPrivateNotifications { get; set; }
        public virtual List<T_VISITOR_RECEIVED_NOTIFICATION> SendVisitorNotifications { get; set; }
    }
}
