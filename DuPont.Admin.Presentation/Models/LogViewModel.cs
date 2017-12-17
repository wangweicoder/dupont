
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Webdiyer.WebControls.Mvc;

namespace DuPont.Admin.Presentation.Models
{
    public class ListLogViewModel
    {
        public PagedList<string> Pager { get; set; }
        public List<LogModel> listModel { get; set; }
    }
    public class ListLogViewModelWithoutPager
    {
        //public PagedList<string> Pager { get; set; }
        public List<LogModel> listModel { get; set; }
    }

}