﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Operator
{
    public class DtoAcceptRequirement : BaseModel
    {
        [Required]
        public int requirementid { set; get; }
    }
}