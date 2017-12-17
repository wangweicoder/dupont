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
using System;
namespace DuPont.Admin.Presentation.Models
{
    public class FarmerVerificationInfoViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string DupontOrderNumbers { get; set; }
        public string PurchasedProducts { get; set; }
        public Nullable<int> Land { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> AuditUserId { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public int AuditState { get; set; }
        public string RejectReason { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public string RealName { get; set; }
        public string PhoneNumber { get; set; }
        public string DetailAddress { get; set; }
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