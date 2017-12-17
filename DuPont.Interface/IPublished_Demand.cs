
// Author           : 李伟
// Created          : 08-11-2015
//
// Last Modified By : 李伟
// Last Modified On : 08-11-2015
// ***********************************************************************
// <copyright file="IPublished_Demand.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Models.Models;
using System.Linq.Expressions;

namespace DuPont.Interface
{
    public interface IPublished_Demand : IRepository<T_FARMER_PUBLISHED_DEMAND>
    {
        /// <summary>
        /// 获取发布列表
        /// </summary>
        /// <param name="UserId">发布者id</param>
        /// <param name="IsClosed">发布状态：0表示进行中，1表示已关闭</param>
        /// <param name="RoleTyp">角色类别（根据角色类别读取相应表的数据）</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">一页显示的条数</param>
        /// <returns>返回发布列表集合</returns>
        //List<PublishedDemandRepositoryModel> GetDemandList(long userid, int IsClosed, int RoleTyp, int PageIndex, int PageSize, out long TotalNums);
        //IList<PublishedDemandRepositoryModel> GetDemandList<T1, T2, T3>(int IsClosed, int RoleTyp, int PageIndex, int PageSize, Expression<Func<T2, long>> where1, Expression<Func<T2, long>> where2, Expression<Func<T1, T2, dynamic>> where3, Expression<Func<T3, long>> where4, Expression<Func<dynamic, int>> where5, Expression<Func<T3, int>> where6, Expression<Func<dynamic, T3, dynamic>> where7);
       
        /// <summary>
        /// 获取当前（userid）发布的需求
        /// </summary>
        /// <param name="userid">发布者id</param>
        /// <param name="IsClosed">发布状态：0表示进行中，1表示已关闭</param>
        /// <param name="roletype">角色类别（根据角色类别读取相应表的数据）</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">一页显示的条数</param>
        /// <param name="TotalNums">总条数</param>
        /// <returns>发布当前产业商发布的需求列表集合</returns>
        List<PublishedModel> GetBusinessPublishedRequirement(long userid, int IsClosed, int roletype, int PageIndex, int PageSize, out long TotalNums);
        /// <summary>
        /// 获取当前（userid）发布的需求
        /// </summary>
        /// <param name="userid">发布者id</param>
        /// <param name="IsClosed">发布状态：0表示进行中，1表示已关闭</param>
        /// <param name="roletype">角色类别（根据角色类别读取相应表的数据）</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">一页显示的条数</param>
        /// <param name="TotalNums">总条数</param>
        /// <returns>发布当前大农户发布的需求列表集合</returns>
        List<PublishedModel> GetFarmerPublishedRequirement(long userid, int IsClosed, int roletype, int PageIndex, int PageSize, out long TotalNums);
        /// <summary>
        /// 大农户的进行中需求列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="isclosed"></param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
       List<FarmerDemand> GetFarmerDemandList(int pageindex, int pagesize, int isclosed, out long TotalNums);
       /// <summary>
       /// 产业商的进行中需求列表
       /// </summary>
       /// <param name="pageindex"></param>
       /// <param name="pagesize"></param>
       /// <param name="isclosed"></param>
       /// <param name="TotalNums"></param>
       /// <returns></returns>
       List<PublishedModel> GetBusinessDemandList(int pageindex, int pagesize, int isclosed, out long TotalNums);
    }

}
