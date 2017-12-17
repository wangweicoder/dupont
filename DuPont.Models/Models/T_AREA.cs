using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_AREA
    {
        public string AID { get; set; }
        public string DisplayName { get; set; }
        public string ParentAID { get; set; }
        public byte Level { get; set; }
        public string Lng { get; set; }
        public string Lat { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
