using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertMyReplyListOutput
    {
        public long QuestionId { get; set; }
        public string Content { get; set; }
        public long ReplyTime { get; set; }
    }
}
