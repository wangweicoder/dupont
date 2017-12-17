using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Demand
{
    public class FarmerDemandReplyItem
    {
        public string PhoneNumber { get; set; }
        public string Comments { get; set; }
        public DateTime ReplyTime { get; set; }
        /// <summary>
        /// 应答人用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 评价的星数
        /// </summary>
        public int Score { get; set; }
    }
}
