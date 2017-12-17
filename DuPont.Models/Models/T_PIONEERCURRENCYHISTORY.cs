using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_PIONEERCURRENCYHISTORY
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public int DPoint { get; set; }
        public Nullable<long> AuditUserId { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
    }
}
