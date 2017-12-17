using DuPont.Models.DataAnotations;
// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 01-07-2016
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 01-07-2016
// ***********************************************************************
// <copyright file="FarmListSearchInput.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Models.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class FarmListInput : ListDto
    {
        [SQLValidate]
        public string Keywords { get; set; }
        public bool? IsOpen { get; set; }
        public bool? IsDeleted { get; set; }

        [RegularExpression(@"\d+", ErrorMessage = "省份地址不正确!")]
        public string ProvinceAid { get; set; }

        [RegularExpression(@"\d+", ErrorMessage = "城市地址不正确!")]
        public string CityAid { get; set; }

        [RegularExpression(@"\d+", ErrorMessage = "区县地址不正确!")]
        public string RegionAid { get; set; }
        public DateTime? OpenStartDate { get; set; }
        public DateTime? OpenEndDate { get; set; }
    }
}
