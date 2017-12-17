using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class RoleBusinessRegister : BaseModel
    {
        [Required(ErrorMessage = "用户编号为必填项")]
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "用户编号必须为数字")]
        public Int64 UserId { get; set; }

        [Required(ErrorMessage = "手机号为必填项")]
        [Display(Name = "手机号")]
        [SQLValidate]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "真实姓名为必填项")]
        [SQLValidate]
        public string RealName { get; set; }

        [Required(ErrorMessage = "地址为必填项")]
        [SQLValidate]
        [Address]
        public string Address { get; set; }
        [SQLValidate]
        public string DetailAddress { get; set; }

        [Required(ErrorMessage = "收购类型为必填项")]
        [SQLValidate]
        public string PurchaseType { get; set; }
        [SQLValidate]
        public string Description { get; set; }

        //[Required(ErrorMessage = "图片编号列表")]
        [RegularExpression(@"^[\d,]+$", ErrorMessage = "图片编号列表必须由数字和逗号组成")]
        [SQLValidate]
        public string PicturesIds { get; set; }
    }
}