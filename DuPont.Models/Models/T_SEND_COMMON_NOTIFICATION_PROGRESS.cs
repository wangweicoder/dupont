using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    /// <summary>
    /// 公共信息推送进度表
    /// </summary>
    public class T_SEND_COMMON_NOTIFICATION_PROGRESS
    {
        public T_SEND_COMMON_NOTIFICATION_PROGRESS()
        {
            SendTotalCount = 0;
            CreateTaskTime = DateTime.Now;
        }

        /// <summary>
        /// 消息编号
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 最近推送的截止用户编号
        /// </summary>
        public long LastMaxUserId { get; set; }

        /// <summary>
        /// 已推送的用户总数
        /// </summary>
        public long SendTotalCount { get; set; }

        /// <summary>
        /// 推送任务创建时间
        /// </summary>
        public DateTime CreateTaskTime { get; set; }

        public virtual T_NOTIFICATION Notification { get; set; }
    }
}
