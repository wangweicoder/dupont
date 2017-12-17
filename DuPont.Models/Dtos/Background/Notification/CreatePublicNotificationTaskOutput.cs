using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Notification
{
    public class CreatePublicNotificationTaskOutput
    {
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
        /// 消息推送目标用户
        /// </summary>
        public T_USER TargetUser { get; set; }
    }
}
