// Author           : 李伟
// Created          : 08-20-2015
//
// Last Modified By : 李伟
// Last Modified On : 08-20-2015
// ***********************************************************************
// <copyright file="PublishedModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    /// <summary>
    /// 发布列表公用model
    /// </summary>
    public class PublishedModel
    {
        /// <summary>
        /// 需求编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 需求状态编号
        /// </summary>
        public int PublishStateId { get; set; }
        /// <summary>
        /// 需求状态
        /// </summary>
        public string PublishState { get; set; }
        /// <summary>
        /// 需求类别编号
        /// </summary>
        public int RequirementTypeId { get; set; }
        /// <summary>
        /// 需求类别
        /// </summary>
        public string RequirementType { get; set; }
        /// <summary>
        /// 需求发布日期
        /// </summary>
        public string PublishedDate { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 编号地址
        /// </summary>
        public string AddressCode { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }
        /// <summary>
        /// 期望的日期
        /// </summary>
        public string Dates { get; set; }
        /// <summary>
        /// 发布者姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 发布者手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 发布者信誉（）
        /// </summary>
        public byte? Credit { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// 预期最低价格
        /// </summary>
        public double ExpectedStartPrice { get; set; }
        /// <summary>
        /// 预期最高价格
        /// </summary>
        public double ExpectedEndPrice { get; set; }
        /// <summary>
        /// 田地亩数区间编号
        /// </summary>
        public int AcreageId { get; set; }
        /// <summary>
        /// 田地亩数区间描述
        /// </summary>
        public string Acreage { get; set; }
        /// <summary>
        /// 农作物描述编号
        /// </summary>
        public int CropId { get; set; }
        /// <summary>
        /// 农作物描述
        /// </summary>
        public string Crop { get; set; }
        /// <summary>
        /// 收购区间类型编号
        /// </summary>
        public int PurchaseWeightId { get; set; }
        /// <summary>
        /// 收购区间类型描述
        /// </summary>
        public string PurchaseWeight { get; set; }
        /// <summary>
        /// 起购重量编号
        /// </summary>
        public int CommenceWeightId { get; set; }
        /// <summary>
        /// 起购重量描述
        /// </summary>
        public string CommenceWeight { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Lat { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public string Lng { get; set; }
        /// <summary>
        /// 创建者用户编号
        /// </summary>
        public long CreateUserId { get; set; }

        /// <summary>
        /// 发布者的角色等级
        /// </summary>
        public long Level { get; set; }

        /// <summary>
        /// 是否开放
        /// </summary>
        public bool IsOpen { get; set; }
    }
}
