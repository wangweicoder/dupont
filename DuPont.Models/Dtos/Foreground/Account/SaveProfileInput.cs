using DuPont.Entity.Enum;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Account
{
    /// <summary>
    /// 保存用户个人信息输入模型
    /// </summary>
    public class SaveProfileInput
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType RoleType { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        public Int64 UserId { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 用户省、市、区县、乡镇、村地址（格式：省编号|市编号|区县编号|乡镇编号|村编号）
        /// </summary>
        [Required]
        [RegularExpression(@"^\d*[|]\d*[|]\d*[|]\d*[|]\d*$", ErrorMessage = "地址格式不正确!")]
        public string Address { get; set; }

        /// <summary>
        /// 用户详细地址
        /// </summary>
        public string DetailAddress { get; set; }

        #region "--农机手的额外信息"
        /// <summary>
        /// 农机数据--仅农机手角色拥有
        /// </summary>
        public string Machinery { get; set; }

        /// <summary>
        /// 其它农机数据--仅农机手角色拥有
        /// </summary>
        public string OtherMachinery { get; set; }
        #endregion

        #region "--大农户的额外信息"
        /// <summary>
        /// 已购先锋亩数--仅大农户角色拥有
        /// </summary>
        public int? PurchasedProductsQuantity { get; set; }

        /// <summary>
        /// 共有土地数量--仅大农户角色拥有
        /// </summary>
        public int? Land { get; set; }
        #endregion

        #region "--产业商的额外信息"
        /// <summary>
        /// 收购类型（数量项之间以英文逗号隔开）--仅产业商角色拥有
        /// </summary>
        public string PurchaseType { get; set; }

        /// <summary>
        /// 业务描述--仅产业商角色拥有
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 用户手机号码
        /// </summary>
        [RegularExpression(@"1[3578]\d{9}",ErrorMessage="手机号码格式不正确!")]
        public string PhoneNumber { get; set; }
        #endregion
    }
}
