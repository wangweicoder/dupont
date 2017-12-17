using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoAppVersion : BaseModel
    {
        [Required(ErrorMessage="平台字段为必填项")]
        [SQLValidate]
        public string platform { set; get; }
    }
}