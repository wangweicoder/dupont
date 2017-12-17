using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_SYS_LOG
    {
        public long Id { get; set; }
        public string Level { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public Nullable<long> UserId { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string RequestParameter { get; set; }
    }
}
