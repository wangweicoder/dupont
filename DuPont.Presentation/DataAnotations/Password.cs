using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.DataAnotations
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class Password : ValidationAttribute
    {
        public Password()
            : base("{0}请输入6位有效的数字!")
        {
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                if (!DuPont.Utility.RegexHelper.IsMatch(valueAsString,@"^\d{6}$"))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);//提供格式化的提示信息
                }
            }

            return ValidationResult.Success;
        }
    }
}