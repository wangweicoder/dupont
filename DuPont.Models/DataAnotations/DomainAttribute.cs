// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 12-14-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-14-2015
// ***********************************************************************
// <copyright file="DomainAttribute.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DuPont.Models.DataAnotations
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DomainAttribute : ValidationAttribute
    {
        public IEnumerable<string> Values { get; private set; }

        public DomainAttribute(string value)
            : base("有效的{0}必须是{1}!")
        {
            this.Values = new string[] { value };
        }

        public DomainAttribute(params string[] values)
            : base("有效的{0}必须是{1}之一!")
        {
            this.Values = values;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                if (!this.Values.Any(item => value.ToString() == item))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);//提供格式化的提示信息
                }
            }

            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            string[] values = this.Values.Select(value => string.Format("'{0}'", value)).ToArray();
            return string.Format(base.ErrorMessageString, name, string.Join(",", values));
        }
    }
}
