using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_QUESTION_REPLY
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public long QuestionId { get; set; }
        public string Content { get; set; }
        public long LikeCount { get; set; }
        public bool IsAgree { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime LastModifiedTime { get; set; }
    }
}
