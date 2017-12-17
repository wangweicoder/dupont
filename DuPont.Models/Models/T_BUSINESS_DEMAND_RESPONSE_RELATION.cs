using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_BUSINESS_DEMAND_RESPONSE_RELATION
    {
        public long Id { get; set; }
        public int BonusDPoint { get; set; }
        public long DemandId { get; set; }
        public long UserId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Comments { get; set; }
        public System.DateTime ReplyTime { get; set; }
        public int Score { get; set; }
        public int WeightRangeTypeId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Brief { get; set; }
    }
}
