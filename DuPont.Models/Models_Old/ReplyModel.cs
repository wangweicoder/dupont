// Author           : 李伟
// Created          : 08-19-2015
//
// Last Modified By : 李伟
// Last Modified On : 08-19-2015
// ***********************************************************************
// <copyright file="BusinessReplyModel.cs" company="">
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
    public class ReplyModel
    {
        /// <summary>
        /// 应答记录id
        /// </summary>
        public long Id { get;set;}
        /// <summary>
        /// 需求发布状态描述
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 需求类型描述
        /// </summary>
        public string RequireTypeName { get; set; }
        /// <summary>
        /// 需求类型（我需要）
        /// </summary>
        public string Requirement{get;set;}
        /// <summary>
        /// 需求发布日期
        /// </summary>
        public string PublishedDate { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Remark { get;set;}

        /// <summary>
        /// 重量(产生商的需求则是起购重量、大农户发的则是区间)
        /// </summary>
        public string Acreage { get; set; }

        /// <summary>
        /// 应答者用户名
        /// </summary>
        public string PublisherUserName { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 发布者的用户编号
        /// </summary>
        public long PublisherUserId { get; set; }
    }
}
