// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-24-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-24-2015
// ***********************************************************************
// <copyright file="FarmerVerificationInfoViewModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Models.Models;
namespace DuPont.Models
{
    public class FarmerVerificationInfoViewModel:T_FARMER_VERIFICATION_INFO
    {
        public string UserName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        /// <summary>
        /// 区县名称
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 乡镇名称
        /// </summary>
        public string TownshipName { get; set; }
        /// <summary>
        /// 村的名称
        /// </summary>
        public string VillageName { get; set; }
        /// <summary>
        /// APP接口服务的地址
        /// </summary>
        public string RemoteApiUrl { get; set; }
        
    }
}