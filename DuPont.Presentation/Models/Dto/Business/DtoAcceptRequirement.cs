using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Business
{
    public class DtoAcceptRequirement : BaseModel
    {
        [Required]
        public int requirementid { set; get; }
    }
}