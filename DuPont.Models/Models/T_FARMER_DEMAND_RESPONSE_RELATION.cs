using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_FARMER_DEMAND_RESPONSE_RELATION
    {
        public long Id { get; set; }
        public int BonusDPoint { get; set; }
        public long DemandId { get; set; }
        public long UserId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Comments { get; set; }
        public System.DateTime ReplyTime { get; set; }
        public int Score { get; set; }
        /// <summary>
        /// 评价大农户的内容
        /// <author>ww</author>
        /// </summary>
        public string CommentsFarmer { get; set; }
        public System.DateTime ReplyTimeFarmer { get; set; }
        public int ScoreFarmer { get; set; }
        /// <summary>
        /// 0 本站，1 E 田
        /// </summary>
        public int SourceType { set; get; }
    }
}
