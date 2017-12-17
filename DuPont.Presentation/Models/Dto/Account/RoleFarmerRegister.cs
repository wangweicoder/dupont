using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class RoleFarmerRegister : BaseModel
    {
        [Required(ErrorMessage = "用户编号为必填项")]
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "用户编号必须为数字")]
        public Int64 UserId { get; set; }

        [Required(ErrorMessage = "手机号码为必填项")]
        [Display(Name = "手机号码")]
        [PhoneNumber]
        [SQLValidate]
        public string PhoneNumber { get; set; }
        [SQLValidate]
        public string RealName { get; set; }

        [Required(ErrorMessage = "地址为必填项")]
        [SQLValidate]
        [Address]
        public string Address { get; set; }
        [SQLValidate]
        public string DetailAddress { get; set; }

        [SQLValidate]
        public string DoPuntOrderNumbers { get; set; }
        [Required(ErrorMessage = "亩数为必填项")]
        public int Land { get; set; }

        [Required(ErrorMessage = "已购先锋亩数")]
        public int PurchasedProductsQuantity { get; set; }
    }
}