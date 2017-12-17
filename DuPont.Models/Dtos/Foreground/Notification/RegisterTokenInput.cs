using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Extensions;

namespace DuPont.Models.Dtos.Foreground.Notification
{
    public class RegisterTokenInput : IValidatableObject
    {
        public string DeviceToken { get; set; }
        public long UserId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DeviceToken.IsNullOrEmpty() || UserId == 0)
            {
                //yield return new ValidationResult("设备编号与用户编号为必传!", new string[] { "UserId", "DeviceToken" });
                yield return new ValidationResult("服务器开小差啦,请重试!");
            }

            if (UserId == 0 && !DeviceToken.IsNullOrEmpty() && DeviceToken.Length < 10)
            {
                yield return new ValidationResult("服务器开小差啦,请重试!");
            }
        }
    }
}
