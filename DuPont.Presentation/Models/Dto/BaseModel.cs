using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto
{
    public class BaseModel
    {
        [SQLValidate]
        [RegularExpression(@"\d*",ErrorMessage="格式不正确!")]
        public string cur_time { get; set; }
    }
}