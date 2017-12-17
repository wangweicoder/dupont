using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Account
{
    public class UpdatePasswordInput
    {
        [Required(ErrorMessage = "新密码为必填项")]
        [RegularExpression(@"^.{6,18}$",ErrorMessage="密码必须为6-18位!")]
        public string newpas { get; set; }

        [Required(ErrorMessage = "手机号码为必填项")]
        [Display(Name = "手机号码")]
        [PhoneNumber]
        [SQLValidate]
        public string phonenumber { get; set; }
    }
}
