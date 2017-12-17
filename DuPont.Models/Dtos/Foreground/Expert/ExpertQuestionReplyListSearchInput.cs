using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertQuestionReplyListSearchInput
    {
        public long QuestionId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
