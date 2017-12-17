using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoGetAreaChild : BaseModel
    {
        [Required]
        [SQLValidate]
        public string ParentAId { set; get; }
    }
}