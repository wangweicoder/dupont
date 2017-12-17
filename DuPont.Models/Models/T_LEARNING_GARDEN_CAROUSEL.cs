using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_LEARNING_GARDEN_CAROUSEL
    {
        public long Id { get; set; }
        public long ArticleId { get; set; }
        public long CatId { get; set; }
        public virtual T_ARTICLE T_ARTICLE { get; set; }
        public virtual T_ARTICLE_CATEGORY T_ARTICLE_CATEGORY { get; set; }
    }
}
