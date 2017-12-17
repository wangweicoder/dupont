// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-10-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-10-2015
// ***********************************************************************
// <copyright file="LoginViewModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace DuPont.Admin.Presentation.Models
{
    public class LoginViewModel
    {
        [ScaffoldColumn(false)]
        public long UserId { get; set; }

        [Required(ErrorMessage = "账号名不能为空!")]
        [Display(Name = "账号名")]
        public string LoginUserName { get; set; }


        [Required(ErrorMessage = "密码不能为空!")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}