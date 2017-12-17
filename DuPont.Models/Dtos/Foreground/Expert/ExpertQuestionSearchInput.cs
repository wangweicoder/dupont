using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertQuestionSearchInput
    {
        public string Keywords { get; set; }

        [Required]
        public int PageIndex { get; set; }

        [Required]
        public int PageSize { get; set; }

        [Domain("1", "0")]
        public int IsOpen { get; set; }
    }
}
