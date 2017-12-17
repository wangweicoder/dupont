using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class FarmBookListInput
    {
        public int FarmId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
