using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_ARTICLE
    {
        public T_ARTICLE()
        {
            this.T_LEARNING_GARDEN_CAROUSEL = new List<T_LEARNING_GARDEN_CAROUSEL>();
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public System.DateTime CreateTime { get; set; }
        public long Click { get; set; }
        public long CatId { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPutOnCarousel { get; set; }
        public virtual T_ARTICLE_CATEGORY T_ARTICLE_CATEGORY { get; set; }
        public virtual List<T_LEARNING_GARDEN_CAROUSEL> T_LEARNING_GARDEN_CAROUSEL { get; set; }
    }
}
