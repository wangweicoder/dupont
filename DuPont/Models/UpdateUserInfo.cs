using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models
{
   public  class UpdateUserInfo
    {
       /// <summary>
       /// 用户编号
       /// </summary>
       [System.Web.Mvc.HiddenInput]
       public long Id { get;set; }
       /// <summary>
       /// 用户电话
       /// </summary>
       public string PhoneNumber { get; set; }
       /// <summary>
       /// 用户旧密码
       /// </summary>
       [DataType(DataType.Password)]
       [Required(ErrorMessage = "请输入旧密码!")]
       public string OldPwd { get; set; }
       /// <summary>
       /// 用户新密码
       /// </summary>
       [Display(Name = "新密码")]
       [DataType(DataType.Password)]
       [Required(ErrorMessage = "请输入新密码!")]
       public string NewPwd { get; set; }
       /// <summary>
       /// 确认新密码
       /// </summary>
       [Display(Name = "确认新密码")]
       [DataType(DataType.Password)]
       [Compare("NewPwd", ErrorMessage = "两次输入的密码不一致，请重新输入!")]
       public string SavePwd { get; set; }
    }
}
