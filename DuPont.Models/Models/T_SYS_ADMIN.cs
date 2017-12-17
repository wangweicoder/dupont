using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_SYS_ADMIN
    {
        public long UserId { get; set; }
        public string IsSuper { get; set; }
        public long CreateUserId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
    }
}
