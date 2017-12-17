using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_USER_PASSWORD_HISTORY
    {
        public long UserID { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
    }
}
