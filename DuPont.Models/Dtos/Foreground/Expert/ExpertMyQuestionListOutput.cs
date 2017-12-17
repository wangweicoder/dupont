using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertMyQuestionListOutput
    {
        public long QuestionId { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public long CreateTime { get; set; }
        public bool IsOpen { get; set; }
    }
}
