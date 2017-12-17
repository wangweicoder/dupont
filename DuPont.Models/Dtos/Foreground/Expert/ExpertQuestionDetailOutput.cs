using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertQuestionDetailOutput
    {
        public ExpertQuestionDetailOutput()
        {
            PictureUrlList = new List<string>();
        }
        public long QuestionId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public List<string> PictureUrlList { get; set; }
        public long CreateTime { get; set; }
        public bool IsOpen { get; set; }
    }
}
