using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_BUSINESS_VERIFICATION_INFO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string RealName { get; set; }
        public string PurchaseType { get; set; }
        public string Introduction { get; set; }
        public string PictureIds { get; set; }
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
    }
}
