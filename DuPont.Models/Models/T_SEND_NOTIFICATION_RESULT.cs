using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    /// <summary>
    /// 已推送的消息列表(针对个人消息)
    /// </summary>
    public class T_SEND_NOTIFICATION_RESULT
    {
        public T_SEND_NOTIFICATION_RESULT()
        {
            SendTime = DateTime.Now;
        }

        /// <summary>
        /// 消息编号
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        public virtual T_USER TargetUser { get; set; }
        public virtual T_NOTIFICATION Notification { get; set; }
    }
}
