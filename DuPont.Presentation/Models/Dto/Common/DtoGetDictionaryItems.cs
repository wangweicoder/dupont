using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoGetDictionaryItems : BaseModel
    {
        [Required]
        [SQLValidate]
        public string Code { set; get; }
    }
}