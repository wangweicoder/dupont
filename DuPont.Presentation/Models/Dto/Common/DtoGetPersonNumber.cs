using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoGetPersonNumber : BaseModel
    {
        [Required]
        public long id { set; get; }
        [Required]
        public int roleType { set; get; }

    }
}