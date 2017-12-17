using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Global.Common
{
    public class DemandIdWithDistanceItem
    {
        public long DemandId { get; set; }
        public double Distance { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }

        public long CreateUserId { get; set; }
        public int AcresId { get; set; }
    }
}
