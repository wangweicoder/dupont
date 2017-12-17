using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.User
{
    public class SearchBackgroundUserListOutput
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 登录账号名
        /// </summary>
        public string LoginUserName { get; set; }

        /// <summary>
        /// 省份编号
        /// </summary>
        public string ProvinceCode { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public string CityCode { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 区县编号
        /// </summary>
        public string RegionCode { get; set; }

        /// <summary>
        /// 区县名称
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        ///// <summary>
        ///// 乡镇
        ///// </summary>
        //public string TownshipName { get; set; }

        ///// <summary>
        ///// 村
        ///// </summary>
        //public string VillageName { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 是否已锁定
        /// </summary>
        public bool IsLocked { get; set; }
    }
}
