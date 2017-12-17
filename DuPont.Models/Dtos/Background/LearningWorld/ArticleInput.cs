using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class ArticleInput : IValidatableObject
    {
        [Required(ErrorMessage = "参数'{0}'不能为空")]
        [DisplayName("标题")]
        [IllegalJavaScript]
        public string Title { get; set; }

        [Required(ErrorMessage = "参数'{0}'不能为空")]
        [DisplayName("内容")]
        [IllegalJavaScript]
        public string Content { get; set; }

        [Required(ErrorMessage = "参数'{0}'不能为空")]
        [DisplayName("文章分类")]
        public int CatId { get; set; }

        [DisplayName("是否添加到轮播")]
        public bool AddToCarousel { get; set; }

        [DisplayName("创建者编号")]
        public long CreateUserId { get; set; }

        public bool AddToPushNotification { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.CatId <= 0)
            {
                yield return new ValidationResult(string.Format("先选择'{0}'文章分类!", "文章分类"), new[] { "CatId" });
            }

            var regexImg = new Regex(@"(<img[\s\S]+?src="".+?""){1}");

            if (!regexImg.IsMatch(HttpUtility.HtmlDecode(this.Content)))
            {
                yield return new ValidationResult("内容中至少放上一张图片吧!");
            }
        }
    }
}
