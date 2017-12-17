using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Admin.Presentation.Models
{
    public class SideMenuViewModel
    {
        public string CurrentItem { get; set; }
        public IList<SideMenuViewModel> SubItems { get; set; }
    }
}