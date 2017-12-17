using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.LearningWorld
{
    public class CarouselPictureSearchInput : IValidatableObject
    {
        [Required(ErrorMessage = "参数'{0}'不能为空")]
        public int CatId { get; set; }

        [Required(ErrorMessage = "参数'{0}'不能为空")]
        public string OrderBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OrderBy != "-date" && OrderBy != "-click")
            {
                yield return new ValidationResult(string.Format("'{0}'参数格式错误! 例如: -date或-click", "OrderBy"), new[] { "OrderBy" });
            }
        }
    }
}
