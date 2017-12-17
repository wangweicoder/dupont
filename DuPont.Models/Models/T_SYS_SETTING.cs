using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_SYS_SETTING
    {
        public int ID { get; set; }
        public string SETTING_ID { get; set; }
        public string SETTING_VALUE { get; set; }
        public string COMMENT { get; set; }
        public int CREATE_USER { get; set; }
        public System.DateTime CREATE_DATE { get; set; }
        public Nullable<int> UPDATE_USER { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
    }
}
