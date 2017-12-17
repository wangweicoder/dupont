using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.FarmerRequirement
{
    public class DtoSaveOperatorRequirement : DtoSaveRequirement
    {
        [SQLValidate]
        [Required]
        public string personIds { get; set; }
    }
}