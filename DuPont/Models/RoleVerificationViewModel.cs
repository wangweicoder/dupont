
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;
namespace DuPont.Models
{
    public class RoleVerificationViewModel
    {
        public PagedList<string> Pager { get; set; }
        public IList<VM_GET_PENDING_AUDIT_LIST> PendingAuditList { get; set; }
        public WhereModel Wheremodel { get; set; }
    }

    public class RoleVerificationViewModelWithoutPager
    {
        public IList<VM_GET_PENDING_AUDIT_LIST> PendingAuditList { get; set; }
        public WhereModel Wheremodel { get; set; }
    }
}