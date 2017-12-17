using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_QUESTION
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsOpen { get; set; }
        public long ReplyCount { get; set; }
        public string PictureIds { get; set; }
        public string QuestionType { get; set; }
        public System.DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime LastModifiedTime { get; set; }
        public bool IsPutOnCarousel { get; set; }

        public virtual T_USER User { get; set; }
    }
}
