using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Question
{
    public class SearchQuestionInput
    {
        [IllegalJavaScript]
        public string Keywords { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public bool? IsOpen { get; set; }
        public bool? IsDeleted { get; set; }
    }
}