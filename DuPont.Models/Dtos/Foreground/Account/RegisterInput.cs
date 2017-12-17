using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Account
{
    public class RegisterInput : BaseModel, IValidatableObject
    {
        [Required(ErrorMessage = "手机号码为必填项")]
        [Display(Name = "手机号码")]
        [PhoneNumber]
        [SQLValidate]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "密码为必填项")]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "验证码为必填项")]
        [SQLValidate]
        public string ValidateCode { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var passwordValidate1 = Regex.IsMatch(Password, "^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[-.!@#$%^&*()+?><])[0-9a-zA-Z-.!@#$%^&*()+?><]{7,18}$");
            var passwordValidate2 = Regex.IsMatch(Password, @"^.{6,18}$");

            if (!passwordValidate1 && !passwordValidate2)
            {
                yield return new ValidationResult("密码不符合要求!");
            }
        }
    }
}
