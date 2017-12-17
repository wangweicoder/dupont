using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Account
{
    public class UserProfileModel
    {
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 信誉
        /// </summary>
        public int Credit { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 先锋币
        /// </summary>
        public Int64 DuPontMoney { get; set; }

        /// <summary>
        /// 带详细地址的完整地址信息（地区信息与详细地址之间用英文空格字符隔开）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 仅地区信息（用竖线分开）
        /// </summary>
        public string OriginalAddress { get; set; }

        /// <summary>
        /// 地区地址的代码（用竖线分割）
        /// </summary>
        public string OriginalAddressCode { get; set; }

        /// <summary>
        /// 用户当前角色的级别
        /// </summary>
        public int Level { get; set; }

        public List<RoleBehavior> Behaviors { get; set; }

        #region "--农机手的额外信息"
        /// <summary>
        /// 农机数据--农机手
        /// </summary>
        public List<DuPont.Models.Models.ProductInfo> Machinery { get; set; }

        /// <summary>
        /// 其它农机数据--仅农机手角色拥有
        /// </summary>
        public string OtherMachinery { get; set; }
        #endregion

        #region "--大农户的额外信息"
        /// <summary>
        /// 已购先锋亩数--仅大农户角色拥有
        /// </summary>
        public int PurchasedProductsQuantity { get; set; }

        /// <summary>
        /// 共有土地数量--仅大农户角色拥有
        /// </summary>
        public int Land { get; set; }
        /// <summary>
        /// 亩数认证状态
        /// </summary>
        /// <author>ww</author>
        public int LandAuditState { get; set; }
       
        #endregion

        #region "--产业商的额外信息"
        /// <summary>
        /// 收购类型编号列表（数据项之间以英文逗号隔开）--仅产业商角色拥有
        /// </summary>
        public string PurchaseTypeCodes { get; set; }

        /// <summary>
        /// 收购类型描述列表（数据项之间以英文逗号隔开）--仅产业商角色拥有
        /// </summary>
        public string PurchaseTypeNames { get; set; }

        /// <summary>
        /// 业务描述--仅产业商角色拥有
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }
        #endregion

        public UserProfileModel()
        {
            RealName = string.Empty;
            PhoneNumber = string.Empty;
            DuPontMoney = 0;
            Credit = 0;
            Address = string.Empty;
            Behaviors = new List<RoleBehavior>();
            Machinery = new List<DuPont.Models.Models.ProductInfo>();
        }
    }

    public class RoleBehavior
    {
        /// <summary>
        /// 服务能力的名称 -- 对于不同的角色显示的内容是不同的
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务能力的级别
        /// </summary>
        public int Level { get; set; }
    }
}