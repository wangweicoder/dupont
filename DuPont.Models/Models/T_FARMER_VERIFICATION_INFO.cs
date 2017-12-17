using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_FARMER_VERIFICATION_INFO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string RealName { get; set; }
        public string DupontOrderNumbers { get; set; }
        /// <summary>
        /// 已够亩数
        /// </summary>
        public string PurchasedProducts { get; set; }
        /// <summary>
        /// 共有土地
        /// </summary>
        public Nullable<int> Land { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> AuditUserId { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public int AuditState { get; set; }
        public string RejectReason { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public string DetailAddress { get; set; }
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 审核亩数状态
        /// </summary>
        public int LandAuditState { get; set; }
    }
}
