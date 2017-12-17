using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertQuestionReplyInput
    {
        public long ReplyUserId { get; set; }
        public long QuestionId { get; set; }
        public int RoleId { get; set; }
        [Required]
        [StringLength(140, ErrorMessage = "内容不得超出140个字符!")]
        public string Content { get; set; }
    }
}
