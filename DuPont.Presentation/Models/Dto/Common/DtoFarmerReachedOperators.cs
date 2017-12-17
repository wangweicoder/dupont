using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoFarmerReachedOperators : BaseModel
    {
        public long UserId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }
}