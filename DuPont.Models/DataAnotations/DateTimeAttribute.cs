using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.DataAnotations
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeAttribute : ValidationAttribute
    {
        public DateTimeAttribute()
            : base("{0}格式不正确!")
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                DateTime date;
                if (!DateTime.TryParse(valueAsString, out date))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);//提供格式化的提示信息
                }
            }

            return ValidationResult.Success;
        }
    }
}
