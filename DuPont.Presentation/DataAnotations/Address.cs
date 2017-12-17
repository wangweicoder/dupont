using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DuPont.Presentation.DataAnotations
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class Address : ValidationAttribute
    {
        public Address()
            : base("{0}格式不正确!")
        {

        }
        //^\d*\|\d*\|\d*\|\d*\|\d*$
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);//提供格式化的提示信息
            }

            var valueAsString = value.ToString();
            var isFail = !Regex.IsMatch(valueAsString, @"^\d*\|\d*\|\d*\|\d*\|\d*$");

            if (isFail)
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);//提供格式化的提示信息
            }


            return ValidationResult.Success;
        }
    }
}