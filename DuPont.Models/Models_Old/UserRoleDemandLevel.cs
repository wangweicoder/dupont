using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class UserRoleDemandLevel
    {
        public Int64 UserId { get; set; }

        public int RoleId { get; set; }

        public int Star { get; set; }

        public int DemandTypeId { get; set; }

        public string DemandTypeName { get; set; }
    }
}
