﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.FarmerRequirement
{
    public class DtoRequirementDetail : BaseModel
    {
        [Required]
        public long id { set; get; }
        [Required]
        public int roletype { set; get; }

    }
}