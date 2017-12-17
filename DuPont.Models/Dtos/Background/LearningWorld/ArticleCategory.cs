using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class ArticleCategory
    {
        public int CatId { get; set; }
        [Required]
        [Display(Name="分类名称")]
        [IllegalJavaScript]
        [SQLValidate]
        public string CatName { get; set; }
    }
}
