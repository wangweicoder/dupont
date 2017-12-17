using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_MENU_ROLE_RELATION
    {
        public int MenuId { get; set; }
        public int RoleId { get; set; }
        public long AuditUserId { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
