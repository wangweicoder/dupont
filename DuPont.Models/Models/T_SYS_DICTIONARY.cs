using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_SYS_DICTIONARY
    {
        public int Code { get; set; }
        public int ParentCode { get; set; }
        public string DisplayName { get; set; }
        public int Order { get; set; }
    }
}
