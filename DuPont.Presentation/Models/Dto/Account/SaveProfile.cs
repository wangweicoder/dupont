﻿using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class SaveProfile : BaseModel
    {
        [Required(ErrorMessage = "用户编号为必填项")]
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "用户编号必须为数字")]
        public Int64 UserId{get;set;}
        
        [SQLValidate]
        public string Name{get;set;}

        [SQLValidate]
        public string Address{get;set;}

        [SQLValidate]
        public string DetailAddress { get; set; }
    }
}