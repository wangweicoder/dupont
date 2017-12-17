using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Demand
{
    public class DemandReplyItem
    {
        public string Comments { get; set; }
        public string WeightRange { get; set; }
        public DateTime ReplyTime { get; set; }
        /// <summary>
        /// 应答人用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 应答人手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        public int Score { get; set; }
    }
}
