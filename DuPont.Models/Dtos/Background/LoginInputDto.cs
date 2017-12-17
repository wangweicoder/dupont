using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background
{
    public class LoginInputDto
    {
        [ScaffoldColumn(false)]
        public long UserId { get; set; }

        [Required(ErrorMessage = "账号名不能为空!")]
        [Display(Name = "账号名")]
        public string LoginUserName { get; set; }


        [Required(ErrorMessage = "密码不能为空!")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}
