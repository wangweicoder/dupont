using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_SMS_MESSAGE
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Captcha { get; set; }
        public System.DateTime SendTime { get; set; }
    }
}
