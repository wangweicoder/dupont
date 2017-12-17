using DuPont.Entity.Enum;
using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class SocialLogin
    {
        [Required(ErrorMessage = "没有获取到第三方账号!")]
        [Display(Name = "第三方账号")]
        [SQLValidate]
        public string SocialId { get; set; }

        [Required(ErrorMessage="昵称不能为空!")]
        public string NickName { get; set; }

        [Required(ErrorMessage="用户类型不能为空!")]
        public UserTypes type { get; set; }

    }
}