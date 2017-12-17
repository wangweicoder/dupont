using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.DataAnotations
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SQLValidate : ValidationAttribute
    {
        public SQLValidate()
            : base("{0}有SQL注入潜在危险!")
        {
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var valueAsString = value.ToString();
                if (!ValidateSqlStr(valueAsString))
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);//提供格式化的提示信息
                }
            }

            return ValidationResult.Success;
        }

        public bool ValidateSqlStr(string Str)
        {

            bool ReturnValue = true;

            try
            {

                if (Str.Trim() != "")
                {

                    string SqlStr = @"exec|insert|select|delete|update|count|chr|mid|master|truncate|char|declare|drop|create|iframe|script|count(|chr|mid(|mid|master|truncate|char|char(|declare|and|or|'|--|trim";

                    string[] anySqlStr = SqlStr.Split('|');

                    foreach (string ss in anySqlStr)
                    {

                        if (Str.ToLower().IndexOf(ss) >= 0)
                        {

                            ReturnValue = false;

                            break;

                        }

                    }

                }

            }

            catch
            {

                ReturnValue = false;

            }

            return ReturnValue;

        }
    }
}
