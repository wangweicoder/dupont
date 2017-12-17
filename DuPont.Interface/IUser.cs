using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Models;
// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 毛文君
// Created          : 08-04-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-05-2015
// ***********************************************************************
// <copyright file="IUser.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface IUser : IRepositoryBase<T_USER>
    {
        //IList<T_USER> GetAll(System.Linq.Expressions.Expression<Func<T_USER, bool>> predicate);
        /// <summary>
        /// 根据用户编号获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        T_USER GetUserById(string phonename);

        List<T_ROLE> GetRoles(Int64 userId);
        /// <summary>
        /// 用于找回密码接口
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        T_USER GetByWhere(Expression<Func<T_USER, bool>> wherelambda);

        /// <summary>
        /// 获取用户所对应角色的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<T_USER_ROLE_RELATION> GetRoleRelationInfo(Int64 userId);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<UserRoleDemandLevel> GetUserRoleDemandData(Int64 userId, int roleId);

        /// <summary>
        /// 获取用户信誉
        /// 取评价在3星以上的个数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetUserCredit(Int64 userId, Entity.Enum.RoleType roleType);

        /// <summary>
        /// 获取先锋币
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Int64 GetDuPontMoney(Int64 userId);

        /// <summary>
        /// 获取待审核的角色列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>List&lt;T_ROLE&gt;.</returns>
        List<T_ROLE> GetWaitAuditRoleList(Int64 userId);
        /// <summary>
        /// 获取所有用户列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="reocrdCount"></param>
        /// <returns></returns>
        //IList<T_USER> GetAll(int pageIndex, int pageSize, out long reocrdCount);
        /// <summary>
        /// 根据查询条件获取用户列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="reocrdCount"></param>
        /// <param name="wheremodel">查询条件model</param>
        /// <returns>用户列表</returns>
        IList<VM_GET_USER_ROLE_INFO_LIST> GetUserList(int pageIndex, int pageSize, out long reocrdCount, WhereModel wheremodel);
        /// <summary>
        /// 获取技能列表
        /// </summary>
        /// <returns></returns>
        IList<SkillModel> GetSkill();
        /// <summary>
        /// 获取当前用户的技能列表
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        IList<OneUserSkillModel> GetOneUserSkill(long id);
        /// <summary>
        /// 获取当前用户的角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList<RoleModel> GetRoleList(long id);
        /// <summary>
        /// 获取最大的星星数
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        int GetMaxStar(long userid, int roleid);

        long UpdateUserInfo(T_USER model, Dictionary<int, byte?> demandLevelInfoList, long loginid);
        /// <summary>
        /// 获取登录者的token
        /// </summary>
        /// <param name="guserid"></param>
        /// <returns></returns>
        string GetToken(long guserid);
        int CreateUser(T_USER entity, T_USER_ROLE_RELATION rolemodel);

        /// <summary>
        /// 事物模式修改用户密码(曾普 2015-09-28)
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns></returns>
        bool TranUpdatePwd(T_USER userInfo, T_USER_PASSWORD_HISTORY deleteHis);

        /// <summary>
        /// 根据条件获取用户列表，支持按用户角色查询
        /// </summary>
        /// <param name="input"></param>
        /// <param name="roleId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IEnumerable<T_USER> GetAll<TKey>(SearchExpertListInput input, Func<T_USER, TKey> orderBy, Func<T_USER, TKey> orderByDecending, int pageIndex, int pageSize, out long totalCount);

        int LockOrUnlock(bool isLock, params long[] userIds);
    }
}
