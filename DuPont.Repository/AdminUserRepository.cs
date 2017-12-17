
using DuPont.Extensions;
using DuPont.Entity.Enum;
using DuPont.Interface;
using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class AdminUserRepository : BaseRepository<T_ADMIN_USER>, IAdminUser
    {
        private IUser_Role userRoleRepository;
        public AdminUserRepository(IUser_Role userRoleRepository)
        {
            this.userRoleRepository = userRoleRepository;
        }

        public List<T_USER_ROLE_RELATION> GetRoles(long adminUserId)
        {
            return userRoleRepository.GetAll(rel => rel.UserID == adminUserId && rel.MemberType == false).ToList();
        }


        public bool CreateUser(T_ADMIN_USER adminUser, T_USER_ROLE_RELATION userRoleRelation)
        {
            using (var ctx = GetDbContextInstance())
            {
                var tran = ctx.Database.BeginTransaction();
                Batch(ctx, EntityState.Added, adminUser);
                ctx.SaveChanges();
                userRoleRelation.UserID = adminUser.Id;
                var userRoleRelationEntry = ctx.Entry<T_USER_ROLE_RELATION>(userRoleRelation);
                userRoleRelationEntry.State = EntityState.Added;
                var effect = ctx.SaveChanges();

                //如果添加的用户为经销商角色还需要添加经销商的分管区域记录
                if (userRoleRelation.RoleID == (int)RoleType.Dealer)
                {
                    string manageAID = null;
                    Func<string, bool> IsValidAddressValue = (areaCode) => !(string.IsNullOrEmpty(areaCode) || areaCode == "0");

                    if (IsValidAddressValue(adminUser.Village))
                        manageAID = adminUser.Village;
                    else if (IsValidAddressValue(adminUser.Township))
                        manageAID = adminUser.Township;
                    else if (IsValidAddressValue(adminUser.Region))
                        manageAID = adminUser.Region;
                    else if (IsValidAddressValue(adminUser.Province))
                        manageAID = adminUser.Province;

                    var supplesAreaEntry = ctx.Entry<T_SUPPLIERS_AREA>(new T_SUPPLIERS_AREA
                    {
                        AID = manageAID,
                        UserID = adminUser.Id,
                        CreateDateTime = userRoleRelation.CreateTime,
                        State = true
                    });
                    supplesAreaEntry.State = EntityState.Added;
                    effect = ctx.SaveChanges();
                }

                tran.Commit();
                return effect > 0;
            }
        }

        /// <summary>
        /// 获取后台指定角色用户列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="roleType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<BackgroundUserModel> GetBackgroundUserList(System.Linq.Expressions.Expression<Func<T_ADMIN_USER, bool>> predicate, RoleType roleType, int pageIndex, int pageSize, out int totalCount)
        {
            var roleTypeId = (int)roleType;
            using (var ctx = GetDbContextInstance())
            {
                List<BackgroundUserModel> backgroundUserList = null;

                if (roleType != RoleType.Unknown)
                {
                    var query = from u in ctx.Set<T_ADMIN_USER>().Where(p=>!p.IsSuperAdmin).Where(predicate)
                                join userRole_RL in ctx.Set<T_USER_ROLE_RELATION>() on u.Id equals userRole_RL.UserID
                                where userRole_RL.RoleID == roleTypeId &&!userRole_RL.MemberType
                                select new
                                {
                                    User = u,
                                    userRole_RL.RoleID
                                };

                    totalCount = query.Count();
                    var data = query.OrderByDescending(p => p.User.CreateTime)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

                    if (totalCount > 0)
                    {
                        backgroundUserList = new List<BackgroundUserModel>();
                        foreach (var user in data)
                        {
                            var bgUserModelItem = new BackgroundUserModel();
                            ClassValueCopyHelper.Copy(bgUserModelItem, user.User);
                            bgUserModelItem.RoleName = ((RoleType)user.RoleID).GetDescription();
                            backgroundUserList.Add(bgUserModelItem);
                        }
                    }
                }
                else
                {
                    var query = from u in ctx.Set<T_ADMIN_USER>().Where(p => !p.IsSuperAdmin).Where(predicate)
                                join userRole_RL in ctx.Set<T_USER_ROLE_RELATION>() on u.Id equals userRole_RL.UserID
                                where !userRole_RL.MemberType
                                select new
                                {
                                    User = u,
                                    userRole_RL.RoleID
                                };

                    totalCount = query.Count();
                    var data = query.OrderByDescending(p => p.User.CreateTime)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

                    if (totalCount > 0)
                    {
                        backgroundUserList = new List<BackgroundUserModel>();
                        foreach (var user in data)
                        {
                            var bgUserModelItem = new BackgroundUserModel();
                            ClassValueCopyHelper.Copy(bgUserModelItem, user.User);
                            bgUserModelItem.RoleName = ((RoleType)user.RoleID).GetDescription();
                            backgroundUserList.Add(bgUserModelItem);
                        }
                    }
                }

                return backgroundUserList;
            }
        }

        public int LockOrUnlock(bool isLock, params long[] userIds)
        {
            using (var ctx = GetDbContextInstance())
            {
                return ctx.Database.ExecuteSqlCommand("update " + typeof(T_ADMIN_USER).Name + " set IsLock=" + (isLock ? 1 : 0) + " where Id in(" + string.Join(",", userIds) + ")");
            }
        }
    }
}
