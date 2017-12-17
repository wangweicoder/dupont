using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class T_VISITOR_RECEIVED_NOTIFICATION
    {
        public T_VISITOR_RECEIVED_NOTIFICATION()
        {
            SendTime = DateTime.Now;
        }

        /// <summary>
        /// 消息编号
        /// </summary>
        public long MsgId { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string DeviceToken { get; set; }

        /// <summary>
        /// 平台类型
        /// </summary>
        public string OsType { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        public virtual T_NOTIFICATION Notification { get; set; }
    }
}
