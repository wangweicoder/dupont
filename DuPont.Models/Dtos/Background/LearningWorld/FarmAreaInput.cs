using DuPont.Models.DataAnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class FarmAreaInput
    {
        public int FarmId { get; set; }
        [XSSJavaScript]
        public string FarmName { get; set; }

        [Required(ErrorMessage = "分区名称不能为空!")]
        [XSSJavaScript]
        public string Name { get; set; }

        public bool IsMachineryArea { get; set; }

        [Required(ErrorMessage = "内容不能为空!")]
        [IllegalJavaScript]
        public string Content { get; set; }

       public IEnumerable<ValidationResult>Validate(ValidationContext validationContext)
        {
            if (FarmId <= 0)
                yield return new ValidationResult(string.Format("展区编号{}必须为正整数", FarmId), new[] { "FarmId" });
        }
    }
}