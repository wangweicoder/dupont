using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DuPont.Models.DataAnotations
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class IllegalJavaScriptAttribute : ValidationAttribute
    {
        private Regex illegalJsRegex = new Regex(@"(?<content><\s*script[\s\S]*?>[\s\S]+?<\s*/\s*script\s*>)");

        public IllegalJavaScriptAttribute()
            : base("{0} 含有风险脚本!")
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {   var s="";
                var str=value.ToString();
                s = str.Replace("&amp;", "&");
                s = s.Replace("&lt;", "<");
                s = s.Replace("&gt", ">");
                if (illegalJsRegex.Matches(s).Count > 0)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);//提供格式化的提示信息
                }
            }

            return ValidationResult.Success;
        }
    }
}
