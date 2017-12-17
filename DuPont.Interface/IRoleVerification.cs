// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 毛文君
// Created          : 08-11-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-11-2015
// ***********************************************************************
// <copyright file="IRoleVerification.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Models.Models;
using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
namespace DuPont.Interface
{
    public interface IRoleVerification : IRepository<VM_GET_PENDING_AUDIT_LIST>
    {
        IList<VM_GET_PENDING_AUDIT_LIST> GetAll(int pageIndex, int pageSize, out long reocrdCount);
        IList<VM_GET_PENDING_AUDIT_LIST> GetAll(int pageIndex, int pageSize, out long reocrdCount, WhereModel model);
        long GetRecordCount(System.Linq.Expressions.Expression<Func<VM_GET_PENDING_AUDIT_LIST, bool>> predicate);

        /// <summary>
        /// 同意大农户的角色申请
        /// </summary>
        /// <param name="verificationId">角色验证记录编号</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="roleLevel">角色级别（星数）</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        bool ApproveFarmerVerification(long verificationId, long auditUserId, byte roleLevel);


        /// <summary>
        /// 同意农机手的角色申请
        /// </summary>
        /// <param name="verificationId">角色验证记录编号</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="demandLevelInfoList">服务类别技能信息列表</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        bool ApproveOperatorVerification(long verificationId, long auditUserId, Dictionary<int, int> demandLevelInfoList);

        /// <summary>
        /// 同意产业商的角色申请
        /// </summary>
        /// <param name="verificationId">角色验证记录编号</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="demandLevelInfoList">服务类别技能信息列表</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        bool ApproveBusinessVerification(long verificationId, long auditUserId, Dictionary<int, int> demandLevelInfoList);

        /// <summary>
        /// 拒绝角色申请操作
        /// </summary>
        /// <param name="verificationId">角色验证记录编号</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="roleType">角色类型</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        bool RejectVerification(long verificationId, long auditUserId, RoleType roleType);

        /// <summary>
        /// 修改农机手农机与需求对应关系
        /// </summary>        /// 
        /// <param name="UserId">审核人用户编号</param>
        /// <param name="demandLevelInfoList">服务类别技能信息列表</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        bool UpdateOperatorVerification(long auditUserId, Dictionary<int, int> demandLevelInfoList);
    }
}
