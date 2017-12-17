using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_USER_ROLE_DEMANDTYPELEVEL_RELATION
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public int DemandId { get; set; }
        public int Star { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
