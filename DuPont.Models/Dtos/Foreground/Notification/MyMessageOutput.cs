using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Notification
{
    public class MyMessageOutput
    {
        public MyMessageOutput()
        {
            Interval = 1;
            MsgList = new List<MessageItem>();
        }
        public int Interval { get; set; }
        public List<MessageItem> MsgList { get; set; }
    }

    public class MessageItem
    {
        public long MsgId { get; set; }
        public string Message { get; set; }
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
        public bool IsOpen { get; set; } 

    }
}
