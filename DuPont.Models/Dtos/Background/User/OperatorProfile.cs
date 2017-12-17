using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.User
{
    public class OperatorProfile
    {
        public OperatorProfile()
        {
            Machinery = new List<ProductInfo>();
        }
        public long UserId { get; set; }

        public string Name { get; set; }

        public long Star { get; set; }

        public List<ProductInfo> Machinery { get; set; }
        public string Lng { get; set; }
        public string Lat { get; set; }
    }
}
