using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.LearningWorld
{
    public class CarouselPictureOutput
    {
        /// <summary>
        /// 文章编号
        /// </summary>
        public long ArticleId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string ArticleTitle { get; set; }

        /// <summary>
        /// 文章静态页地址
        /// </summary>
        public string ArticleUrl { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
