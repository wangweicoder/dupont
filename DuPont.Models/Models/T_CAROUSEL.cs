using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_CAROUSEL
    {
        public int Id { get; set; }
        public long FileId { get; set; }
        public int Order { get; set; }
        public bool IsDisplay { get; set; }
        public long CreateUserId { get; set; }
        public int RoleId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> EditUserId { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
    }
}
