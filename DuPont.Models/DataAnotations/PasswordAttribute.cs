using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DuPont.Models.DataAnotations
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class PasswordAttribute : ValidationAttribute
    {
        public PasswordAttribute()
            : base("{0}太简单!")
        {
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                //if (!Regex.IsMatch(valueAsString, "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[-.!@#$%^&*()+?><])[0-9a-zA-Z-.!@#$%^&*()+?><]{7,18}$"))
                if (!Regex.IsMatch(valueAsString, "^[a-zA-Z0-9]{6,21}$"))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);//提供格式化的提示信息
                }
            }

            return ValidationResult.Success;
        }
    }
}
