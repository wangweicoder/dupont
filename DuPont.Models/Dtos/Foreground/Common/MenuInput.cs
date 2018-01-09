using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Common
{
    public class MenuInput
    {
        public string CurrentItem { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

}
