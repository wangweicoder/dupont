using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class VM_GET_PENDING_AUDIT_LIST
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string PhoneNumber { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int AuditState { get; set; }
    }
}
