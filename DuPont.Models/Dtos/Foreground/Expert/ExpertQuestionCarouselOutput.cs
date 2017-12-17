using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public partial class ExpertQuestionCarouselOutput
    {
        public long QuestionId { get; set; }
        public string Title { get; set; }
        public string PictureUrl { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }

    }
}
