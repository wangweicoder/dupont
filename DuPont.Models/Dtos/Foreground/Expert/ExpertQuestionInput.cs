using DuPont.Models.DataAnotations;
using DuPont.Entity.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertQuestionInput
    {
        [Required]
        public long CreateUserId { get; set; }
        [Required]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50,ErrorMessage="标题不得超出50个字符!")]
        public string Title { get; set; }

        [Required]
        [StringLength(140, ErrorMessage = "描述不得超出140个字符!")]
        public string Description { get; set; }

        [Required]
        public QuestionType QuestionType { get; set; }

        [RegularExpression("[0-9,]+")]
        public string PictureIds { get; set; }

    }
}
