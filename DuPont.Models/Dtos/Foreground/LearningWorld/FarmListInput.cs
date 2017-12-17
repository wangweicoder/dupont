// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 01-05-2016
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 01-05-2016
// ***********************************************************************
// <copyright file="FarmListInput.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

namespace DuPont.Models.Dtos.Foreground.LearningWorld
{
    public class FarmListInput
    {
        [Required]
        [RegularExpression(@"^\d*\|\d*\|\d*\|\d*\|\d*$", ErrorMessage = "地址格式不正确!")]
        public string Address { get; set; }
    }
}