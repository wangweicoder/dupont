using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_ARTICLE_CATEGORY
    {
        public T_ARTICLE_CATEGORY()
        {
            this.T_ARTICLE = new List<T_ARTICLE>();
            this.T_LEARNING_GARDEN_CAROUSEL = new List<T_LEARNING_GARDEN_CAROUSEL>();
        }

        public long CategoryId { get; set; }
        public string Name { get; set; }
        public virtual List<T_ARTICLE> T_ARTICLE { get; set; }
        public virtual List<T_LEARNING_GARDEN_CAROUSEL> T_LEARNING_GARDEN_CAROUSEL { get; set; }
    }
}
