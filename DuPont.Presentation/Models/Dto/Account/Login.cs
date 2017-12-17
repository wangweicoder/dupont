using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class Login:BaseModel
    {
        [Required(ErrorMessage = "手机号码为必填项")]
        [Display(Name = "手机号码")]
        [PhoneNumber]
        [SQLValidate]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "密码为必填项")]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}