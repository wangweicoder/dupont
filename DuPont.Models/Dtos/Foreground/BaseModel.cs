using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground
{
    public class BaseModel
    {
        [SQLValidate]
        [RegularExpression(@"\d*", ErrorMessage = "格式不正确!")]
        public string cur_time { get; set; }
    }
}
