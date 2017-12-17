using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Business
{
    public class DtoReplyRequirement : BaseModel
    {
        [Required]
        public long id { set; get; }
        [Required]
        public long userId { set; get; }
    }
}