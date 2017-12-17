using DuPont.Models.Dtos.Background.Demand;
using System;
// ***********************************************************************
// Assembly         : DuPont
// Author           : 张振
// Created          : 12-15-2015
//
// Last Modified By : 
// Last Modified On : 
// ***********************************************************************

// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;


namespace DuPont.Admin.Presentation.Models
{
    public class BusinessDemandViewModel
    {
        public PagedList<string> Pager { get; set; }
        public List<BusinessListModel> PendingAuditList { get; set; }
        public BusinessSeachModel Wheremodel { get; set; }
    }

    
}