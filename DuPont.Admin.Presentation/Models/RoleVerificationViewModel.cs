
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;
namespace DuPont.Admin.Presentation.Models
{
    public class RoleVerificationViewModel
    {
        public PagedList<string> Pager { get; set; }
        public List<VM_GET_PENDING_AUDIT_LIST> PendingAuditList { get; set; }
        public WhereModel Wheremodel { get; set; }

    }

    public class RoleVerificationViewModelWithoutPager
    {
        public List<VM_GET_PENDING_AUDIT_LIST> PendingAuditList { get; set; }
        public WhereModel Wheremodel { get; set; }
    }
}