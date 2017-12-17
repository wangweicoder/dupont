// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 01-06-2016
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 01-06-2016
// ***********************************************************************
// <copyright file="FarmBookingInput.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;

namespace DuPont.Models.Dtos.Foreground.LearningWorld
{
    public class FarmBookingInput : IValidatableObject
    {
        public int FarmId { get; set; }
        public long UserId { get; set; }

        [Required]
        public DateTime? VisitDate { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (VisitDate != null && VisitDate <= DateTime.Now)
            {
                yield return new ValidationResult("预约的日期必须至少提前一天!", new[] { "VisitDate" });
            }
        }
    }
}