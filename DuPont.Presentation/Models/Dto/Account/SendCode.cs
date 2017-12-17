using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class SendCode : BaseModel
    {
        [Required(ErrorMessage = "手机号码为必填项")]
        [Display(Name = "手机号码")]
        [PhoneNumber]
        [SQLValidate]
        public string phonenumber { get; set; }
    }
}