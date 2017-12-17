using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertQuestionListOutput
    {
        public long QuestionId { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public long CreateTime { get; set; }
        public long CreateUserId { get; set; }
        public int RoleId { get; set; }
        public long ReplyCount { get; set; }
        public string UserName { get; set; }
    }
}
