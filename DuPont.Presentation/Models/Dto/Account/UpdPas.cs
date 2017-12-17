using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class UpdPas : BaseModel
    {
        [Required(ErrorMessage="新密码为必填项")]
        [SQLValidate]
        public string newpas { get; set; }

        [Required(ErrorMessage = "手机号码为必填项")]
        [Display(Name = "手机号码")]
        [PhoneNumber]
        [SQLValidate]
        public string phonenumber { get; set; }
    }
}