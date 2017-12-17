using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Question
{
    public class QuestionDetailWithReplyListOutput
    {
        public long QuestionId { get; set; }
        public string Title { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public string Description { get; set; }

        public List<QuestionReply> ReplyList { get; set; }

    }

    public class QuestionReply
    {

        public long ReplyId { get; set; }
        public int RoleId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime ReplyTime { get; set; }

    }
}
