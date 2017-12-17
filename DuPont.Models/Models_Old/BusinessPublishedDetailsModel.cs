// Author           : 李伟
// Created          : 08-20-2015
//
// Last Modified By : 李伟
// Last Modified On : 08-20-2015
// ***********************************************************************
// <copyright file="BusinessPublishedDetailsModel.cs" company="">
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
    /// 产业商详情公用model
    /// </summary>
    public class BusinessPublishedDetailsModel
    {
        /// <summary>
        /// 需求编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 需求发布日期
        /// </summary>
        public string PublishedDate { get; set; }
        /// <summary>
        /// 需求类别编号
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 需求类别描述
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 需求状态编号
        /// </summary>
        public int PublishStateId { get; set; }
        /// <summary>
        /// 需求状态描述
        /// </summary>
        public string PublishState { get; set; }
        /// <summary>
        /// 发布需求者id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 发布需求者姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 发布需求者级别
        /// </summary>
        public long? UserLevel { get; set; }
        /// <summary>
        /// 发布需求者手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 期望的日期
        /// </summary>
        public string Dates { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }
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
        /// 预期收购最低价格
        /// </summary>
        public double ExpectedStartPrice { get; set; }
        /// <summary>
        /// 预期收购最高价格
        /// </summary>
        public double ExpectedEndPrice { get; set; }

        /// <summary>
        /// 农作物编号
        /// </summary>
        public int CropId { get; set; }

        /// <summary>
        /// 农作物编号的名称
        /// </summary>
        public string Crop { get; set; }

        public List<ReplyDetailModel> ReplyList { get; set; }
    }
}
