
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Admin.Presentation.Models
{
    public class RoleVerificationViewModelWithNoPager
    {
        public List<VM_GET_PENDING_AUDIT_LIST> PendingAuditList { get; set; }
        public WhereModel Wheremodel { get; set; }
    }
}