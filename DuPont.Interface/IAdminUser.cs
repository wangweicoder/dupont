using DuPont.Entity.Enum;
using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace DuPont.Interface
{
    public interface IAdminUser : IRepositoryBase<T_ADMIN_USER>
    {
        List<T_USER_ROLE_RELATION> GetRoles(Int64 adminUserId);

        bool CreateUser(T_ADMIN_USER adminUser, T_USER_ROLE_RELATION userRoleRelation);

        /// <summary>
        /// 获取后台人员用户列表
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <param name="roleType">角色类型</param>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">每页显示的数据个数</param>
        /// <param name="totalCount">符合条件的总记录数</param>
        /// <returns></returns>
        List<BackgroundUserModel> GetBackgroundUserList(Expression<Func<T_ADMIN_USER, bool>> predicate, RoleType roleType, int pageIndex, int pageSize, out int totalCount);

        int LockOrUnlock(bool isLock, params long[] userIds);
    }
}
