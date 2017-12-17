using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.ViewModel
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "*{0}不能为空!")]
        [DisplayName("姓名")]
        [IllegalJavaScript]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*{0}不能为空!")]
        [DisplayName("登录用户名")]
        [IllegalJavaScript]
        public string LoginUserName { get; set; }

        //[Required(ErrorMessage = "*{0}不能为空!")]
        //[RegularExpression(@"^\d{11}", ErrorMessage = "*格式错误！")]
        //[DisplayName("手机号")]
        //public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "*{0}不能为空!")]
        [PasswordAttribute]
        [DisplayName("密码")]
        public string Password { get; set; }


        [Required(ErrorMessage = "*{0}不能为空!")]
        [DisplayName("确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配！")]
        public string conformPhoneNumber { get; set; }
         [IllegalJavaScript]
        [DisplayName("角色")]
        public string Role { get; set; }

         [IllegalJavaScript]
        [DisplayName("省")]
        [Required(ErrorMessage = "*{0}不能为空!")]
        public string Province { get; set; }

         [IllegalJavaScript]
        [DisplayName("市")]
        [Required(ErrorMessage = "*{0}不能为空!")]
        public string City { get; set; }

         [IllegalJavaScript]
        [DisplayName("区县")]
        [Required(ErrorMessage = "*{0}不能为空!")]
        public string Region { get; set; }

         [IllegalJavaScript]
        [DisplayName("乡镇")]
        public string Township { get; set; }

         [IllegalJavaScript]
        [DisplayName("村")]
        public string Village { get; set; }
         [IllegalJavaScript]
         [DisplayName("详细地址")]
        public string DetailedAddress { get; set; }

        public System.DateTime CreateTime { get; set; }

        [Required(ErrorMessage = "*{0}不能为空!")]
        [DisplayName("先锋币")]
        [DefaultValue(0)]
        public Nullable<int> DPoint { get; set; }
    }
}
