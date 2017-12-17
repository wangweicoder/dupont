using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Notification
{
    public class PersonalNotificationListOutput
    {
        public long MsgId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContent { get; set; }
        public string IosDeviceToken { get; set; }
        public string PhoneNumber { get; set; }
    }
}
