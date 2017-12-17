using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_EXPERT
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool IsEnabled { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime LastModifiedTime { get; set; }
    }
}
