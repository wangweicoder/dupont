using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class ValidCode : BaseModel
    {
        [Required(ErrorMessage = "验证码为必填项")]
        [RegularExpression(@"^\d+$", ErrorMessage = "验证码必须为正整数")]
        [SQLValidate]
        public string smscode { get; set; }

        [Required(ErrorMessage = "手机号为必填项")]
        [Display(Name = "手机号码")]
        [PhoneNumber]
        [SQLValidate]
        public string phonenumber { get; set; }
    }
}