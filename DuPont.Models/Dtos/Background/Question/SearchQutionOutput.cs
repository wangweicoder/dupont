using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Question
{
    public class SearchQutionOutput
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsOpen { get; set; }
        public bool IsDeleted { get; set; }
        public long UserId { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModifiedTime { get; set; }

        public bool IsPutOnCarousel { get; set; }
    }
}