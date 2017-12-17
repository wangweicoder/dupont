using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class ArticleListSearchOutput
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public long Click { get; set; }
        public string Url { get; set; }
        public string PictureUrl { get; set; }
    }
}
