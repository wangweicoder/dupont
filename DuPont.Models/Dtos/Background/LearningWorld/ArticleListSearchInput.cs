using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class ArticleListSearchInput : IValidatableObject
    {
        public ArticleListSearchInput()
        {
            this.CatId = 1;
            this.Keywords = "";
            this.OrderBy = "-date";
            this.PageIndex = 1;
            this.PageSize = 10;
        }
        [Required(ErrorMessage = "参数'{0}'不能为空")]
        public int PageIndex { get; set; }

        [Required(ErrorMessage = "参数'{0}'不能为空")]
        public int PageSize { get; set; }

        [Required(ErrorMessage = "参数'{0}'不能为空")]
        public string OrderBy { get; set; }

        [Required(ErrorMessage = "参数'{0}'不能为空")]
        public int CatId { get; set; }
        [SQLValidate]
        public bool? IsDeleted { get; set; }
        [SQLValidate]
        public string Keywords { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OrderBy != "-date" && OrderBy != "-click")
            {
                yield return new ValidationResult(string.Format("'{0}'参数格式错误! 例如: -date或-click", "OrderBy"), new[] { "OrderBy" });
            }
        }
    }
}
