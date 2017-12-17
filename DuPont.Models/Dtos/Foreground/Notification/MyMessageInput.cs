using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Extensions;

namespace DuPont.Models.Dtos.Foreground.Notification
{
    public class MyMessageInput : IValidatableObject
    {
        public string DeviceToken { get; set; }

        public long? UserId { get; set; }


        [Required(ErrorMessage = "平台类型不能为空!")]
        public string OsType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DeviceToken.IsNullOrEmpty() && (!UserId.HasValue || UserId.Value <= 0))
            {
                yield return new ValidationResult("必须传设备编号与用户编号之一!", new string[] { "UserId" });
            }

            if (!UserId.HasValue)
            {
                if ((DeviceToken ?? string.Empty).Length < 10)
                {
                    yield return new ValidationResult("设备标识格式不正确!", new string[] { "DeviceToken" });
                }
            }
        }
    }
}
