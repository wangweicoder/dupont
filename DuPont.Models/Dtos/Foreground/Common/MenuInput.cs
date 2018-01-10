using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Common
{
    public class MenuInput
    {
        /// <summary>
        /// 父级id
        /// </summary>
        public int ParentId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

}
