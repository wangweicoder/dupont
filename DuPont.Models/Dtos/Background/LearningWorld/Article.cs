using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class Article
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public string Url { get; set; }
        public int Click { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public string Title { get; set; }
        public string CatName { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool IsPutOnCarousel { get; set; }

        public bool IsDeleted { get; set; }
    }
}
