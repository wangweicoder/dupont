using DuPont.Models.DataAnotations;
// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 01-06-2016
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 01-07-2016
// ***********************************************************************
// <copyright file="DemonstateFarmInput.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel.DataAnnotations;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class DemonstateFarmDetailViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "省份不能为空!")]
        [RegularExpression(@"\d+", ErrorMessage = "省份地址不正确!")]
        public string ProvinceAid { get; set; }

        [Required(ErrorMessage = "城市不能为空!")]
        [RegularExpression(@"\d+", ErrorMessage = "城市地址不正确!")]
        public string CityAid { get; set; }

        [Required(ErrorMessage = "区县不能为空!")]
        [RegularExpression(@"\d+", ErrorMessage = "区县地址不正确!")]
        public string RegionAid { get; set; }
        [XSSJavaScript]
        [Required(ErrorMessage = "农场名称不可为空!")]
        public string Name { get; set; }

        public bool IsOpen { get; set; }
        [XSSJavaScript]
        public DateTime? OpenStartDate { get; set; }
        [XSSJavaScript]
        public DateTime? OpenEndDate { get; set; }
        [XSSJavaScript]
        [Required(ErrorMessage = "农场面积不可为空!")]
        public string PlantArea { get; set; }

        [Required(ErrorMessage = "主要品种不可为空!")]
        [XSSJavaScript]
        public string Variety { get; set; }
        [XSSJavaScript]
        [Required(ErrorMessage = "播种时间不可为空!")]
        public string SowTime { get; set; }
        [XSSJavaScript]
        [Required(ErrorMessage = "种植要点不可为空!")]
        public string PlantPoint { get; set; }

        public int? Id { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IsOpen && (OpenEndDate == null || OpenStartDate == null))
            {
                yield return new ValidationResult("请将开放时间的起止日期填写完整!");
            }
        }
    }
}