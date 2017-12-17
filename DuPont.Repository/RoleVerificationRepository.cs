// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 08-12-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-12-2015
// ***********************************************************************
// <copyright file="RoleVerificationRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using EntityFramework.Extensions;
using DuPont.Entity.Enum;
using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DuPont.Repository
{
    public class RoleVerificationRepository : IRoleVerification
    {
        public int Insert(VM_GET_PENDING_AUDIT_LIST entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<VM_GET_PENDING_AUDIT_LIST> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        public int Delete(VM_GET_PENDING_AUDIT_LIST entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<VM_GET_PENDING_AUDIT_LIST> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<VM_GET_PENDING_AUDIT_LIST, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(VM_GET_PENDING_AUDIT_LIST entity)
        {
            throw new NotImplementedException();
        }

        public VM_GET_PENDING_AUDIT_LIST GetByKey(object key)
        {
            throw new NotImplementedException();
        }

        public IList<VM_GET_PENDING_AUDIT_LIST> GetAll()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var query = from c in dbContext.VM_GET_PENDING_AUDIT_LIST
                            orderby c.CreateTime ascending
                            select c;
                return query.ToList();
            }
        }

        public IList<VM_GET_PENDING_AUDIT_LIST> GetAll(System.Linq.Expressions.Expression<Func<VM_GET_PENDING_AUDIT_LIST, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IList<VM_GET_PENDING_AUDIT_LIST> GetAll(int pageIndex, int pageSize, out long reocrdCount)
        {

            using (var dbContext = new DuPont_TestContext())
            {
                var result = (from m in dbContext.VM_GET_PENDING_AUDIT_LIST
                              orderby m.CreateTime ascending

                              select new
                              {
                                  Id = m.Id,
                                  AuditState = m.AuditState,
                                  City = m.City,
                                  CreateTime = m.CreateTime,
                                  PhoneNumber = m.PhoneNumber,
                                  Province = m.Province,
                                  Region = m.Region,
                                  RoleId = m.RoleId,
                                  RoleName = m.RoleName,
                                  UserId = m.UserId,
                                  UserName = m.UserName
                              }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                reocrdCount = dbContext.VM_GET_PENDING_AUDIT_LIST.Count();

                if (result != null)
                {
                    IList<VM_GET_PENDING_AUDIT_LIST> targetResultList = new List<VM_GET_PENDING_AUDIT_LIST>();
                    foreach (var model in result)
                    {
                        targetResultList.Add(new VM_GET_PENDING_AUDIT_LIST
                        {
                            Id = model.Id,
                            AuditState = model.AuditState,
                            City = model.City,
                            CreateTime = model.CreateTime,
                            UserName = model.UserName,
                            UserId = model.UserId,
                            RoleName = model.RoleName,
                            RoleId = model.RoleId,
                            Region = model.Region,
                            Province = model.Province,
                            PhoneNumber = model.PhoneNumber
                        });
                    }

                    return targetResultList;
                }

                return null;
            }
        }

        public long GetRecordCount(System.Linq.Expressions.Expression<Func<VM_GET_PENDING_AUDIT_LIST, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 同意大农户的角色申请
        /// </summary>
        /// <param name="verificationId">角色验证记录编号</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="roleLevel">角色级别（星数）</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        public bool ApproveFarmerVerification(long verificationId, long auditUserId, byte roleLevel)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                using (var dbTransaction = dbContext.Database.BeginTransaction())
                {
                    var farmerVerifyInfo = dbContext.T_FARMER_VERIFICATION_INFO.Find(verificationId);
                    farmerVerifyInfo.AuditState = 1;

                    dbContext.T_USER_ROLE_RELATION.Add(new T_USER_ROLE_RELATION
                    {
                        UserID = farmerVerifyInfo.UserId,
                        AuditUserId = auditUserId,
                        RoleID = (int)RoleType.Farmer,
                        MemberType = true,
                        CreateTime = TimeHelper.GetChinaLocalTime(),
                        Star = roleLevel
                    });
                    var userInfo = dbContext.T_USER.Find(farmerVerifyInfo.UserId);
                    userInfo.Province = farmerVerifyInfo.Province;
                    userInfo.City = farmerVerifyInfo.City;
                    userInfo.Region = farmerVerifyInfo.Region;
                    userInfo.Township = farmerVerifyInfo.Township;
                    userInfo.Village = farmerVerifyInfo.Village;
                    userInfo.UserName = farmerVerifyInfo.RealName;//将用户的真实姓名更新过去
                    if (userInfo.Type == (int)UserTypes.QQUser || userInfo.Type == (int)UserTypes.WeChatUser)
                    {
                        userInfo.PhoneNumber = farmerVerifyInfo.PhoneNumber;//将用户的最新的手机号更新过去 
                    }
                    userInfo.DetailedAddress = farmerVerifyInfo.DetailAddress;
                    dbContext.Entry<T_USER>(userInfo).State = EntityState.Modified;

                    dbContext.SaveChanges();
                    dbTransaction.Commit();
                    return true;
                }
            }
        }

        /// <summary>
        /// 同意农机手的角色申请
        /// </summary>
        /// <param name="verificationId">角色验证记录编号</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="demandLevelInfoList">服务类别技能信息列表</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        public bool ApproveOperatorVerification(long verificationId, long auditUserId, Dictionary<int, int> demandLevelInfoList)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                using (var dbTransaction = dbContext.Database.BeginTransaction())
                {
                    var machineVerifyInfo = dbContext.T_MACHINERY_OPERATOR_VERIFICATION_INFO.Find(verificationId);
                    machineVerifyInfo.AuditState = 1;

                    dbContext.T_USER_ROLE_RELATION.Add(new T_USER_ROLE_RELATION
                    {
                        UserID = machineVerifyInfo.UserId,
                        AuditUserId = auditUserId,
                        RoleID = (int)RoleType.MachineryOperator,
                        MemberType = true,
                        CreateTime = TimeHelper.GetChinaLocalTime()
                    });
                    var userInfo = dbContext.T_USER.Find(machineVerifyInfo.UserId);
                    userInfo.Province = machineVerifyInfo.Province;
                    userInfo.City = machineVerifyInfo.City;
                    userInfo.Region = machineVerifyInfo.Region;
                    userInfo.Township = machineVerifyInfo.Township;
                    userInfo.Village = machineVerifyInfo.Village;
                    userInfo.UserName = machineVerifyInfo.RealName;//将用户的真实姓名更新过去
                    if (userInfo.Type == (int)UserTypes.QQUser || userInfo.Type == (int)UserTypes.WeChatUser)
                    {
                        userInfo.PhoneNumber = machineVerifyInfo.PhoneNumber;//将用户的最新的手机号更新过去 
                    }
                    userInfo.DetailedAddress = machineVerifyInfo.DetailAddress;
                    dbContext.Entry<T_USER>(userInfo).State = EntityState.Modified;
                    if (demandLevelInfoList != null && demandLevelInfoList.Count > 0)
                    {
                        demandLevelInfoList.Select(d =>
                        {
                            dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Add(new T_USER_ROLE_DEMANDTYPELEVEL_RELATION
                            {
                                UserId = machineVerifyInfo.UserId,
                                DemandId = d.Key,
                                CreateTime = TimeHelper.GetChinaLocalTime(),
                                RoleId = (int)RoleType.MachineryOperator,
                                Star = d.Value
                            });
                            return d;
                        }).Count();
                    }


                    dbContext.SaveChanges();
                    dbTransaction.Commit();
                    return true;
                }
            }
        }


        /// <summary>
        /// 同意产业商的角色申请
        /// </summary>
        /// <param name="verificationId">角色验证记录编号</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="demandLevelInfoList">服务类别技能信息列表</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        public bool ApproveBusinessVerification(long verificationId, long auditUserId, Dictionary<int, int> demandLevelInfoList)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                using (var dbTransaction = dbContext.Database.BeginTransaction())
                {
                    var businessVerifyInfo = dbContext.T_BUSINESS_VERIFICATION_INFO.Find(verificationId);
                    businessVerifyInfo.AuditState = 1;

                    dbContext.T_USER_ROLE_RELATION.Add(new T_USER_ROLE_RELATION
                    {
                        UserID = businessVerifyInfo.UserId,
                        AuditUserId = auditUserId,
                        RoleID = (int)RoleType.Business,
                        MemberType = true,
                        CreateTime = TimeHelper.GetChinaLocalTime()
                    });
                    var userInfo = dbContext.T_USER.Find(businessVerifyInfo.UserId);
                    userInfo.Province = businessVerifyInfo.Province;
                    userInfo.City = businessVerifyInfo.City;
                    userInfo.Region = businessVerifyInfo.Region;
                    userInfo.Township = businessVerifyInfo.Township;
                    userInfo.Village = businessVerifyInfo.Village;
                    userInfo.UserName = businessVerifyInfo.RealName;//将用户的真实姓名更新过去
                    //只有当用户类别为QQ用户或微信用户才可以更新手机号
                    if (userInfo.Type == (int)UserTypes.QQUser || userInfo.Type == (int)UserTypes.WeChatUser)
                    {
                        userInfo.PhoneNumber = businessVerifyInfo.PhoneNumber;//将用户的最新的手机号更新过去 
                    }
                    userInfo.DetailedAddress = businessVerifyInfo.DetailAddress;
                    dbContext.Entry<T_USER>(userInfo).State = EntityState.Modified;
                    if (demandLevelInfoList != null && demandLevelInfoList.Count > 0)
                    {
                        demandLevelInfoList.Select(d =>
                        {
                            dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Add(new T_USER_ROLE_DEMANDTYPELEVEL_RELATION
                            {
                                UserId = businessVerifyInfo.UserId,
                                DemandId = d.Key,
                                CreateTime = TimeHelper.GetChinaLocalTime(),
                                RoleId = (int)RoleType.Business,
                                Star = d.Value
                            });
                            return d;
                        }).Count();
                    }

                    dbContext.SaveChanges();
                    dbTransaction.Commit();
                    return true;
                }
            }
        }
       
        /// <summary>
        /// 拒绝角色申请操作
        /// </summary>
        /// <param name="verificationId">角色验证记录编号</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="roleType">角色类型</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool RejectVerification(long verificationId, long auditUserId, RoleType roleType)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                switch (roleType)
                {
                    case RoleType.Farmer:
                        var farmerVerificationInfo = dbContext.T_FARMER_VERIFICATION_INFO.Find(verificationId);
                        if (farmerVerificationInfo == null)
                            throw new InvalidOperationException("verificationId");

                        farmerVerificationInfo.AuditState = 2;
                        farmerVerificationInfo.AuditUserId = auditUserId;
                        break;
                    case RoleType.MachineryOperator:
                        var operatorVerificationInfo = dbContext.T_MACHINERY_OPERATOR_VERIFICATION_INFO.Find(verificationId);
                        if (operatorVerificationInfo == null)
                            throw new InvalidOperationException("verificationId");

                        operatorVerificationInfo.AuditState = 2;
                        operatorVerificationInfo.AuditUserId = auditUserId;
                        break;
                    case RoleType.Business:
                        var businessVerificationInfo = dbContext.T_BUSINESS_VERIFICATION_INFO.Find(verificationId);
                        if (businessVerificationInfo == null)
                            throw new InvalidOperationException("verificationId");

                        businessVerificationInfo.AuditState = 2;
                        businessVerificationInfo.AuditUserId = auditUserId;
                        break;
                    default:
                        throw new InvalidOperationException("roleType");
                }

                return dbContext.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 获得审核列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="reocrdCount"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<VM_GET_PENDING_AUDIT_LIST> GetAll(int pageIndex, int pageSize, out long reocrdCount, WhereModel model)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                IQueryable<VM_GET_PENDING_AUDIT_LIST> query = dbContext.VM_GET_PENDING_AUDIT_LIST.Where(vm => 1 == 1);
                if (model != null)
                {
                    if (model.SuppliersWhere != null && model.SuppliersWhere.Count > 0)
                    {
                        var predicate = PredicateBuilder.False<VM_GET_PENDING_AUDIT_LIST>();

                        foreach (var code in model.SuppliersWhere)
                        {
                            predicate = predicate.Or(vm => vm.Province == code || vm.Region == code || vm.City == code);
                        }
                        query = query.AsExpandable().Where(predicate);
                    }

                    if (model.RoleId != 0)
                    {
                        query = query.Where(vm => vm.RoleId == model.RoleId);
                    }
                    if (!string.IsNullOrEmpty(model.PhoneNumber))
                    {
                        query = query.Where(vm => vm.PhoneNumber == model.PhoneNumber);
                    }

                    if (model.Province != "0")
                    {
                        query = query.Where(vm => vm.Province == model.Province);
                    }
                    if (model.City != "0")
                    {
                        query = query.Where(vm => vm.City == model.City);
                    }
                    if (model.Region != "0")
                    {
                        query = query.Where(vm => vm.Region == model.Region);
                    }
                    if (model.StartTime.ToString() != "00001/1/1 0:00:00" && model.EndTime.ToString() != "0001/1/1 0:00:00")
                    {
                        query = query.Where(d => d.CreateTime >= model.StartTime && d.CreateTime <= model.EndTime);
                    }
                }
                //IEnumerable<VM_GET_PENDING_AUDIT_LIST> items = dbContext.VM_GET_PENDING_AUDIT_LIST.Where(model);


                //var result = (from m in items
                //              orderby m.CreateTime ascending

                var result = (from m in query
                              select new
                              {
                                  Id = m.Id,
                                  AuditState = m.AuditState,
                                  City = m.City,
                                  CreateTime = m.CreateTime,
                                  PhoneNumber = m.PhoneNumber,
                                  Province = m.Province,
                                  Region = m.Region,
                                  RoleId = m.RoleId,
                                  RoleName = m.RoleName,
                                  UserId = m.UserId,
                                  UserName = m.UserName
                              }).OrderByDescending(d => d.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                reocrdCount = query.Count();

                if (result != null)
                {
                    IList<VM_GET_PENDING_AUDIT_LIST> targetResultList = new List<VM_GET_PENDING_AUDIT_LIST>();
                    foreach (var items in result)
                    {
                        targetResultList.Add(new VM_GET_PENDING_AUDIT_LIST
                        {
                            Id = items.Id,
                            AuditState = items.AuditState,
                            City = items.City,
                            CreateTime = items.CreateTime,
                            UserName = items.UserName,
                            UserId = items.UserId,
                            RoleName = items.RoleName,
                            RoleId = items.RoleId,
                            Region = items.Region,
                            Province = items.Province,
                            PhoneNumber = items.PhoneNumber
                        });
                    }

                    return targetResultList;
                }

                return null;
            }
        }

        /// <summary>
        /// 修改农机手农机与需求对应关系
        /// </summary>        /// 
        /// <param name="UserId">审核人用户编号</param>
        /// <param name="demandLevelInfoList">服务类别技能信息列表</param>
        /// <returns>操作成功返回true,操作失败返回false</returns>
        public bool UpdateOperatorVerification(long UserId, Dictionary<int, int> demandLevelInfoList)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                using (var dbTransaction = dbContext.Database.BeginTransaction())
                {
                   //删除原有的农机与需求对应关系
                    var dbEntrys = dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Where(t=>t.UserId==UserId).ToList();
                    if (dbEntrys!= null)
                    {
                        foreach (var item in dbEntrys)
                        {
                            dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Remove(item);    
                        }                                           
                    }
                    if (demandLevelInfoList != null && demandLevelInfoList.Count > 0)
                    {
                        demandLevelInfoList.Select(d =>
                        {
                            dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION.Add(new T_USER_ROLE_DEMANDTYPELEVEL_RELATION
                            {
                                UserId = UserId,
                                DemandId = d.Key,
                                CreateTime = TimeHelper.GetChinaLocalTime(),
                                RoleId = (int)RoleType.MachineryOperator,
                                Star = d.Value
                            });
                            return d;
                        }).Count();
                    }
                    dbContext.SaveChanges();
                    dbTransaction.Commit();
                    return true;
                }
            }
        }
    }
}
