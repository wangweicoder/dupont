using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertQuestionReplyListOutput
    {
        public long ReplyId { get; set; }
        public long ReplyUserId { get; set; }
        public string ReplyUserName { get; set; }
        public long ReplyTime { get; set; }
        public int RoleId { get; set; }
        public string Content { get; set; }
        public long LikeCount { get; set; }

        public bool IsAgree { get; set; }

    }
}
