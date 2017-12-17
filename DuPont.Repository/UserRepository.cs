// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 08-04-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-05-2015
// ***********************************************************************
// <copyright file="UserRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************



using DuPont.Interface;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DuPont.Extensions;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;

using DuPont.Models.Models;
using DuPont.Models.Dtos.Background.User;
using DuPont.Entity.Enum;
using EntityFramework.Extensions;

namespace DuPont.Repository
{
    public class UserRepository : BaseRepository<T_USER>, IUser
    {
        #region "获取角色列表"
        public List<T_ROLE> GetRoles(Int64 userId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var roles = from r in dbContext.T_ROLE
                            join u in dbContext.T_USER_ROLE_RELATION on r.Id equals u.RoleID
                            where u.MemberType && u.UserID == userId
                            select r;

                return roles.ToList();
            }
        }
        #endregion



        public T_USER GetByWhere(Expression<Func<T_USER, bool>> wherelambda)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_USER.Where(wherelambda).FirstOrDefault();
            }
        }

        public List<T_USER_ROLE_RELATION> GetRoleRelationInfo(Int64 userId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var roles = from r in dbContext.T_USER_ROLE_RELATION
                            where r.MemberType && r.UserID == userId
                            select r;

                var roleList = roles.ToList();
                if (roleList != null)
                {
                    roleList = roleList.Select(role =>
                    {
                        if (role.Star == null)
                        {
                            role.Star = (byte)0;
                        }
                        return role;
                    }).ToList();
                }

                return roleList;
            }
        }

        public List<UserRoleDemandLevel> GetUserRoleDemandData(Int64 userId, int roleId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var roles = from r in dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION
                            join d in dbContext.T_SYS_DICTIONARY on r.DemandId equals d.Code
                            where r.UserId == userId && r.RoleId == roleId
                            select new UserRoleDemandLevel
                            {
                                UserId = r.UserId,
                                RoleId = r.RoleId,
                                DemandTypeId = r.DemandId,
                                DemandTypeName = d.DisplayName,
                                Star = r.Star
                            };

                return roles.ToList();
            }
        }

        public int GetUserCredit(Int64 userId, Entity.Enum.RoleType roleType)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                switch (roleType)
                {
                    case Entity.Enum.RoleType.Farmer:

                        //我发布的需求（产业商响应）
                        var creditFromSelfDemand = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                                    join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                                    where fpd.DemandTypeId > 100800 && fpd.DemandTypeId < 100900 && fpd.CreateUserId == userId && drr.Score >= 3
                                                    select drr.Id).Count();

                        //产业商发布的需求（大农户响应）
                        var creditFromBusinessDemand = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                                                        join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                                                        where bpd.DemandTypeId > 100200 && bpd.DemandTypeId < 100300 && drr.UserId == userId && drr.Score >= 3
                                                        select drr.Id).Count();
                        return creditFromSelfDemand + creditFromBusinessDemand;
                    case Entity.Enum.RoleType.MachineryOperator:
                        return dbContext.T_FARMER_DEMAND_RESPONSE_RELATION.Count(c => c.UserId == userId && c.Score >= 3);
                }

                return 0;
            }
        }

        public Int64 GetDuPontMoney(Int64 userId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var user = dbContext.T_USER.Where(d => d.Id == userId).FirstOrDefault();
                if (user == null)
                {
                    throw new ArgumentException("userId");
                }

                return user.DPoint ?? 0;
            }
        }

        /// <summary>
        /// 获取待审核的角色列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>List&lt;T_ROLE&gt;.</returns>
        public List<T_ROLE> GetWaitAuditRoleList(long userId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var farmerType = (int)RoleType.Farmer;
                var businessType = (int)RoleType.Business;
                var operatorType = (int)RoleType.MachineryOperator;
                var waitResponseList = (from m in dbContext.T_FARMER_VERIFICATION_INFO
                                        where m.AuditState == 0 && m.UserId == userId
                                        select farmerType
                              ).Union(
                                from m in dbContext.T_MACHINERY_OPERATOR_VERIFICATION_INFO
                                where m.AuditState == 0 && m.UserId == userId
                                select operatorType
                              ).Union(
                                   from m in dbContext.T_BUSINESS_VERIFICATION_INFO
                                   where m.AuditState == 0 && m.UserId == userId
                                   select businessType
                              ).Except(from m in dbContext.T_USER_ROLE_RELATION
                                       where m.MemberType && m.UserID == userId
                                       select m.RoleID
                              ).ToList();

                List<T_ROLE> roleList = null;
                if (waitResponseList != null && waitResponseList.Count > 0)
                {
                    roleList = new List<T_ROLE>();
                    waitResponseList.Select(m =>
                    {
                        var role = (RoleType)m;
                        roleList.Add(new T_ROLE() { Id = (int)m, RoleName = role.GetDescription() });
                        return m;
                    }).Count();
                }

                return roleList;
            }
        }

        public IList<VM_GET_USER_ROLE_INFO_LIST> GetUserList(int pageIndex, int pageSize, out long reocrdCount, WhereModel wheremodel)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var predicate = PredicateBuilder.True<VM_GET_USER_ROLE_INFO_LIST>();
                #region 拼接where条件

                //角色类别
                int roletype = wheremodel.RoleId;
                //省
                string provincecode = wheremodel.Province;
                //市
                string citycode = wheremodel.City;
                //区县
                string regioncode = wheremodel.Region;
                //手机号
                string phonenumber = wheremodel.PhoneNumber;
                //注册开始时间
                DateTime starttime = wheremodel.StartTime;
                //注册结束时间
                DateTime endtime = wheremodel.EndTime;

                if (roletype != 0)
                    predicate = predicate.And(p => p.RoleID == roletype);

                if (!string.IsNullOrEmpty(phonenumber))
                    predicate = predicate.And(p => p.PhoneNumber == phonenumber);

                //if (starttime.ToString() != "0001/1/1 0:00:00" && endtime.ToString() != "0001/1/1 0:00:00")
                //    predicate = predicate.And(p => p.CreateTime >= starttime && p.CreateTime <= endtime);

                if (starttime != DateTime.MinValue)
                    predicate = predicate.And(p => p.CreateTime >= starttime);

                if (endtime != DateTime.MinValue)
                    predicate = predicate.And(p => p.CreateTime <= endtime);

                if (regioncode != "0" && regioncode.IsNullOrEmpty() == false)
                    predicate = predicate.And(p => p.Region == regioncode);
                else
                {
                    if (citycode != "0" && citycode.IsNullOrEmpty() == false)
                    {
                        predicate = predicate.And(p => p.City == citycode);
                    }
                    else
                    {
                        if (provincecode != "0" && provincecode.IsNullOrEmpty() == false)
                            predicate = predicate.And(p => p.Province == provincecode);
                    }
                }

                //用户类型过滤
                if (wheremodel.UserTypeId.HasValue)
                    predicate = predicate.And(p => p.Type == wheremodel.UserTypeId.Value);


                #endregion

                reocrdCount = 0;
                List<VM_GET_USER_ROLE_INFO_LIST> userList = null;
                var listQuery = dbContext.VM_GET_USER_ROLE_INFO_LIST.Where(predicate)
                    .OrderByDescending(p => p.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).Future();
                var totalCountQuery = dbContext.VM_GET_USER_ROLE_INFO_LIST.Where(predicate).FutureCount();

    
                reocrdCount = totalCountQuery.Value;
                if (reocrdCount > 0)
                {
                    var userDataList = listQuery.ToList();
                    if (userDataList != null)
                    {
                        userList = new List<VM_GET_USER_ROLE_INFO_LIST>();
                        foreach (var user in userDataList)
                        {
                            userList.Add(new VM_GET_USER_ROLE_INFO_LIST
                            {
                                Id = user.Id,
                                UserId = user.UserId,
                                UserName = user.UserName,
                                PhoneNumber = user.PhoneNumber,
                                Province = user.Province,
                                City = user.City,
                                Region = user.Region,
                                Township = user.Township,
                                Village = user.Village,
                                CreateTime = user.CreateTime,
                                Land = user.Land,
                                RoleID = user.RoleID,
                                Type=user.Type
                            });
                        }

                        return userList;
                    }
                }

                return new List<VM_GET_USER_ROLE_INFO_LIST>();
            }
        }
        /// <summary>
        /// 获取所有角色的技能列表
        /// </summary>
        /// <returns></returns>
        public IList<SkillModel> GetSkill()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                List<T_SYS_DICTIONARY> list1 = dbContext.T_SYS_DICTIONARY.Where(s => s.ParentCode == 100100).ToList();
                list1 = list1 != null ? list1 : new List<T_SYS_DICTIONARY>();
                List<T_SYS_DICTIONARY> list2 = dbContext.T_SYS_DICTIONARY.Where(s => s.ParentCode == 100200).ToList();
                list2 = list2 != null ? list2 : new List<T_SYS_DICTIONARY>();
                List<SkillModel> skilllist = new List<SkillModel>() { 
                    new SkillModel { RoleId = (int)RoleType.Farmer, RoleName = RoleType.Farmer.GetDescription(), Skill = new  List<T_SYS_DICTIONARY> () },
                    new SkillModel { RoleId = (int)RoleType.MachineryOperator, RoleName =RoleType.MachineryOperator.GetDescription(), Skill = list1 },
                    new SkillModel { RoleId = (int)RoleType.Business, RoleName = RoleType.Business.GetDescription(), Skill = list2},
                    new SkillModel { RoleId = (int)RoleType.Dealer, RoleName = RoleType.Dealer.GetDescription(), Skill = new  List<T_SYS_DICTIONARY> () },
                    new SkillModel { RoleId = (int)RoleType.Admin, RoleName = RoleType.Admin.GetDescription(), Skill =new  List<T_SYS_DICTIONARY> ()}
                };
                return skilllist;
            }
        }
        /// <summary>
        /// 获取当前用户的技能列表
        /// </summary>
        /// <returns></returns>
        public IList<OneUserSkillModel> GetOneUserSkill(long id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var list = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Where(u => u.UserId == id).Select(u => new OneUserSkillModel
                {
                    UserId = u.UserId,
                    RoleId = u.RoleId,
                    SkillCode = u.DemandId,
                    Star = u.Star
                }).ToList();
                if (list == null)
                {
                    list = new List<OneUserSkillModel>();
                }
                return list;
            }
        }
        /// <summary>
        /// 获取当前用户的角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<RoleModel> GetRoleList(long id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var list = dbContext.T_USER_ROLE_RELATION.Where(u => u.UserID == id && u.MemberType).Select(u => new RoleModel
                {
                    RoleId = u.RoleID,
                    Star = u.Star != null ? u.Star : 1
                }).ToList();
                if (list == null)
                {
                    list = new List<RoleModel>();
                }
                return list;
            }
        }
        public int GetMaxStar(long userid, int roleid)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                int maxstar = 0;
                var model = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Where(u => u.UserId == userid && u.RoleId == roleid).OrderByDescending(u => u.Star).FirstOrDefault();
                if (model != null)
                {
                    maxstar = model.Star;
                }
                return maxstar;
            }
        }
        public long UpdateUserInfo(T_USER model, Dictionary<int, byte?> demandLevelInfoList, long loginid)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //创建事务，如果操作过程中出现异常，操作会回滚，防止产生脏数据
                using (var tran = dbContext.Database.BeginTransaction())
                {
                    //数据库操作
                    #region 修改用户基本信息
                    T_USER updmodel = dbContext.T_USER.Find(model.Id);
                    updmodel.UserName = model.UserName;
                    //updmodel.PhoneNumber = model.PhoneNumber;
                    updmodel.DPoint = model.DPoint;
                    updmodel.Province = model.Province;
                    updmodel.City = model.City;
                    updmodel.Region = model.Region;
                    updmodel.Township = model.Township;
                    updmodel.Village = model.Village;
                    updmodel.ModifiedUserId = loginid;
                    updmodel.ModifiedTime = Utility.TimeHelper.GetChinaLocalTime();
                    //执行修改操作
                    var upd = dbContext.T_USER.Find(updmodel.Id);
                    if (upd != null)
                    {
                        ClassValueCopyHelper.Copy(upd, updmodel);
                        dbContext.SaveChanges();
                    }
                    #endregion
                    #region 修改用户的角色信息以及技能信息
                    //1获取各角色的最高星星数
                    byte? farmerstar = demandLevelInfoList[0];
                    byte? operatorstar = 0;
                    byte? businesstar = 0;
                    T_USER_ROLE_DEMANDTYPELEVEL_RELATION skillmodel = new T_USER_ROLE_DEMANDTYPELEVEL_RELATION();
                    //农机手
                    for (int i = 100101; i <= 100108; i++)
                    {
                        //添加农机手的技能信息及星星数
                        skillmodel = new T_USER_ROLE_DEMANDTYPELEVEL_RELATION { UserId = model.Id, RoleId = (int)RoleType.MachineryOperator, DemandId = i, Star = Convert.ToInt32(demandLevelInfoList[i]), CreateTime = Utility.TimeHelper.GetChinaLocalTime() };
                        if (demandLevelInfoList[i] > 0)
                        {
                            //先删除原有记录
                            var dbEntry = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.SingleOrDefault(u => u.UserId == skillmodel.UserId && u.RoleId == skillmodel.RoleId && u.DemandId == skillmodel.DemandId);
                            if (dbEntry != null)
                            {
                                dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Remove(dbEntry);
                                dbContext.SaveChanges();
                            }
                            //执行添加操作
                            dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Add(skillmodel);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            //删除原有记录
                            var dbEntry = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.SingleOrDefault(u => u.UserId == skillmodel.UserId && u.RoleId == skillmodel.RoleId && u.DemandId == skillmodel.DemandId);
                            if (dbEntry != null)
                            {
                                dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Remove(dbEntry);
                                dbContext.SaveChanges();
                            }
                        }

                    }
                    //获取农机手角色的最大星星数
                    var operatorstarmodel = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Where(u => u.UserId == model.Id && u.RoleId == (int)RoleType.MachineryOperator).OrderByDescending(u => u.Star).FirstOrDefault();
                    if (operatorstarmodel != null)
                    {
                        operatorstar = (byte?)operatorstarmodel.Star;
                    }
                    //产业商
                    for (int i = 100201; i <= 100202; i++)
                    {
                        //添加产业商的技能信息及星星数
                        skillmodel = new T_USER_ROLE_DEMANDTYPELEVEL_RELATION { UserId = model.Id, RoleId = (int)RoleType.Business, DemandId = i, Star = Convert.ToInt32(demandLevelInfoList[i]), CreateTime = Utility.TimeHelper.GetChinaLocalTime() };
                        if (demandLevelInfoList[i] > 0)
                        {               //先删除原有记录
                            var dbEntry = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.SingleOrDefault(u => u.UserId == skillmodel.UserId && u.RoleId == skillmodel.RoleId && u.DemandId == skillmodel.DemandId);
                            if (dbEntry != null)
                            {
                                dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Remove(dbEntry);
                                dbContext.SaveChanges();
                            }
                            //执行添加操作
                            dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Add(skillmodel);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            //删除原有记录
                            var dbEntry = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.SingleOrDefault(u => u.UserId == skillmodel.UserId && u.RoleId == skillmodel.RoleId && u.DemandId == skillmodel.DemandId);
                            if (dbEntry != null)
                            {
                                dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Remove(dbEntry);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                    //获取产业商角色的最大星星数
                    var businessstarmodel = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Where(u => u.UserId == model.Id && u.RoleId == (int)RoleType.Business).OrderByDescending(u => u.Star).FirstOrDefault();
                    if (businessstarmodel != null)
                    {
                        businesstar = (byte?)businessstarmodel.Star;
                    }
                    //2判断各角色的星星数是否大于0，大于0执行添加操作，如果之前已经存在该角色的记录信息先删除在添加，等于0执行删除操作。
                    T_USER_ROLE_RELATION rolemodel = new T_USER_ROLE_RELATION();
                    if (farmerstar >= 0)//ww增加=，否则会删除角色表的数据
                    {
                        rolemodel = new T_USER_ROLE_RELATION { UserID = model.Id, MemberType = true, RoleID = (int)RoleType.Farmer, Star = farmerstar, AuditUserId = loginid, CreateTime = Utility.TimeHelper.GetChinaLocalTime() };
                        //先删除原有记录
                        var dbEntry = dbContext.T_USER_ROLE_RELATION.SingleOrDefault(u => u.MemberType && u.UserID == rolemodel.UserID && u.RoleID == rolemodel.RoleID);
                        if (dbEntry != null)
                        {
                            dbContext.T_USER_ROLE_RELATION.Remove(dbEntry);
                            dbContext.SaveChanges();
                        }
                        //执行添加操作
                        dbContext.T_USER_ROLE_RELATION.Add(rolemodel);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        //删除原有记录
                        var dbEntry = dbContext.T_USER_ROLE_RELATION.SingleOrDefault(u => u.MemberType && u.UserID == model.Id && u.RoleID == (int)RoleType.Farmer);
                        if (dbEntry != null)
                        {
                            dbContext.T_USER_ROLE_RELATION.Remove(dbEntry);
                            dbContext.SaveChanges();
                        }
                    }
                    if (operatorstar > 0)
                    {
                        rolemodel = new T_USER_ROLE_RELATION { UserID = model.Id, MemberType = true, RoleID = (int)RoleType.MachineryOperator, Star = operatorstar, AuditUserId = loginid, CreateTime = Utility.TimeHelper.GetChinaLocalTime() };
                        //先删除原有记录
                        var dbEntry = dbContext.T_USER_ROLE_RELATION.SingleOrDefault(u => u.MemberType && u.UserID == rolemodel.UserID && u.RoleID == rolemodel.RoleID);
                        if (dbEntry != null)
                        {
                            dbContext.T_USER_ROLE_RELATION.Remove(dbEntry);
                            dbContext.SaveChanges();
                        }
                        //执行添加操作
                        dbContext.T_USER_ROLE_RELATION.Add(rolemodel);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        //删除原有记录
                        var dbEntry = dbContext.T_USER_ROLE_RELATION.SingleOrDefault(u => u.MemberType && u.UserID == model.Id && u.RoleID == (int)RoleType.MachineryOperator);
                        if (dbEntry != null)
                        {
                            dbContext.T_USER_ROLE_RELATION.Remove(dbEntry);
                            dbContext.SaveChanges();
                        }
                    }
                    if (businesstar > 0)
                    {
                        rolemodel = new T_USER_ROLE_RELATION { UserID = model.Id, MemberType = true, RoleID = (int)RoleType.Business, Star = businesstar, AuditUserId = loginid, CreateTime = Utility.TimeHelper.GetChinaLocalTime() };
                        //先删除原有记录
                        var dbEntry = dbContext.T_USER_ROLE_RELATION.SingleOrDefault(u => u.MemberType && u.UserID == rolemodel.UserID && u.RoleID == rolemodel.RoleID);
                        if (dbEntry != null)
                        {
                            dbContext.T_USER_ROLE_RELATION.Remove(dbEntry);
                            dbContext.SaveChanges();
                        }
                        //执行添加操作
                        dbContext.T_USER_ROLE_RELATION.Add(rolemodel);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        //删除原有记录
                        var dbEntry = dbContext.T_USER_ROLE_RELATION.SingleOrDefault(u => u.MemberType && u.UserID == model.Id && u.RoleID == (int)RoleType.Business);
                        if (dbEntry != null)
                        {
                            dbContext.T_USER_ROLE_RELATION.Remove(dbEntry);
                            dbContext.SaveChanges();
                        }
                    }
                    #endregion


                    tran.Commit();//提交事务
                    return 1;
                }
            }
        }
        public int CreateUser(T_USER entity, T_USER_ROLE_RELATION rolemodel)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //创建事务，如果操作过程中出现异常，操作会回滚，防止产生脏数据
                using (var tran = dbContext.Database.BeginTransaction())
                {

                    dbContext.T_USER.Add(entity);
                    dbContext.SaveChanges();
                    rolemodel.MemberType = true;
                    rolemodel.UserID = entity.Id;
                    dbContext.T_USER_ROLE_RELATION.Add(rolemodel);
                    dbContext.SaveChanges();

                    //如果是经销商
                    if (rolemodel.RoleID == (int)RoleType.Dealer)
                    {
                        string controlAreaLevel = "";
                        if (!string.IsNullOrEmpty(entity.Village) && entity.Village != "0")
                        {
                            controlAreaLevel = entity.Village;
                        }
                        else if (!string.IsNullOrEmpty(entity.Region) && entity.Region != "0")
                        {
                            controlAreaLevel = entity.Region;
                        }
                        else if (!string.IsNullOrEmpty(entity.City) && entity.City != "0")
                        {
                            controlAreaLevel = entity.City;
                        }
                        else if (!string.IsNullOrEmpty(entity.Province) && entity.Province != "0")
                        {
                            controlAreaLevel = entity.Province;
                        }
                        else
                        {
                            throw new Exception("经销商的地址不能为空!");
                        }

                        var supplier_area = new T_SUPPLIERS_AREA
                         {
                             AID = controlAreaLevel,
                             CreateDateTime = DateTime.Now,
                             State = true,
                             UserID = entity.Id
                         };

                        var paras = new SqlParameter[] { 
                           new SqlParameter("@aid",supplier_area.AID),
                           new SqlParameter("@createdatetime",supplier_area.CreateDateTime),
                           new SqlParameter("@state",supplier_area.State),
                           new SqlParameter("@userid",supplier_area.UserID)
                        };

                        var sql = "insert into T_SUPPLIERS_AREA(aid,createdatetime,state,userid)";
                        sql += "values(@aid,@createdatetime,@state,@userid)";

                        dbContext.Database.ExecuteSqlCommand(sql, paras);

                        dbContext.SaveChanges();
                    }

                    tran.Commit();//提交事务
                    return 1;
                }

            }
        }
        /// <summary>
        /// 根据登录者的id获取登录时创建的token
        /// </summary>
        /// <param name="guserid"></param>
        /// <returns></returns>
        public string GetToken(long guserid)
        {
            string token = string.Empty;
            if (guserid != 0)
            {
                var model = GetByKey(guserid);
                if (model != null)
                {
                    token = model.LoginToken;
                }
            }
            return token;
        }

        /// <summary>
        /// 修改密码逻辑事物
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="deleteHis">所要删除的用户密码历史记录</param>
        /// <returns></returns>
        public bool TranUpdatePwd(T_USER userInfo, T_USER_PASSWORD_HISTORY deleteHis)
        {

            bool result = true;

            using (var dbContext = new DuPont_TestContext())
            {
                using (var tran = dbContext.Database.BeginTransaction())
                {
                    //更新用户表
                    int uURes = Update(userInfo);
                    if (uURes <= 0)
                    {
                        result = false;
                    }

                    //删除密码历史表中最老的记录
                    if (result && deleteHis != null)
                    {
                        var entity = dbContext.T_USER_PASSWORD_HISTORY.Where(a => a.UserID == deleteHis.UserID && a.Password == deleteHis.Password).FirstOrDefault();
                        dbContext.T_USER_PASSWORD_HISTORY.Remove(entity);
                        int delRes = dbContext.SaveChanges();
                        if (delRes <= 0)
                        {
                            result = false;
                        }
                    }

                    if (result)
                    {
                        dbContext.T_USER_PASSWORD_HISTORY.Add(new T_USER_PASSWORD_HISTORY
                        {
                            UserID = userInfo.Id,
                            Password = userInfo.Password,
                            CreateTime = DateTime.Now
                        });

                        int addRes = dbContext.SaveChanges();
                        if (addRes <= 0)
                        {
                            result = false;
                        }
                    }

                    if (result)
                    {
                        tran.Commit();
                    }

                }
            }

            return result;
        }
        /// <summary>
        /// 根据用户Id获得用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>

        public T_USER GetUserById(string phonename)
        {
            using (var dbContext = new DuPont_TestContext())
            {

                var model = dbContext.T_USER.SingleOrDefault(u => u.PhoneNumber == phonename);

                return model;
            }
        }


        public IEnumerable<T_USER> GetAll<TKey>(SearchExpertListInput input, Func<T_USER, TKey> orderBy, Func<T_USER, TKey> orderByDecending, int pageIndex, int pageSize, out long totalCount)
        {
            using (var ctx = GetDbContextInstance())
            {
                IQueryable<T_USER> query;
                if (input.RoleId > 0)
                {
                    query = from user in ctx.Set<T_USER>()
                            join roleRelation in ctx.Set<T_USER_ROLE_RELATION>() on user.Id equals roleRelation.UserID
                            where roleRelation.RoleID == input.RoleId
                            select user;
                }
                else
                {
                    query = ctx.Set<T_USER>();
                }

                //手机号过滤
                if (input.PhoneNumber != null)
                    query = query.Where(m => m.PhoneNumber == input.PhoneNumber);

                //省份过滤
                if (input.Province != 0)
                {
                    var strProvince = input.Province.ToString();
                    query = query.Where(m => m.Province == strProvince);
                }

                //城市过滤
                if (input.City != 0)
                {
                    var strCity = input.City.ToString();
                    query = query.Where(m => m.City == strCity);
                }

                //区县过滤
                if (input.Region != 0)
                {
                    var strRegion = input.Region.ToString();
                    query = query.Where(m => m.Region == strRegion);
                }

                //注册时间过滤
                if (!string.IsNullOrEmpty(input.StartTime) && !string.IsNullOrEmpty(input.EndTime))
                {
                    var startTime = DateTime.Parse(input.StartTime);
                    var endTime = DateTime.Parse(input.EndTime);
                    query = query.Where(m => m.CreateTime >= startTime && m.CreateTime <= endTime);
                }
                else
                {
                    if (!string.IsNullOrEmpty(input.StartTime))
                    {
                        var startTime = DateTime.Parse(input.StartTime);
                        query = query.Where(m => m.CreateTime >= startTime);
                    }
                    else if (!string.IsNullOrEmpty(input.EndTime))
                    {
                        var endTime = DateTime.Parse(input.EndTime);
                        query = query.Where(m => m.CreateTime <= endTime);
                    }
                }

                totalCount = query.Count();



                //排序
                if (orderBy != null)
                {
                    return query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                return query.OrderByDescending(orderByDecending).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }




        public int LockOrUnlock(bool isLock, params long[] userIds)
        {
            using (var ctx = GetDbContextInstance())
            {
                return ctx.Database.ExecuteSqlCommand("update " + typeof(T_USER).Name + " set IsDeleted=" + (isLock ? 1 : 0) + " where Id in(" + string.Join(",", userIds) + ")");
            }
        }
    }
}
