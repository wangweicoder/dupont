// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 李伟
// Created          : 08-11-2015
//
// Last Modified By : 李伟
// Last Modified On : 08-11-2015
// ***********************************************************************
// <copyright file="Farmer_Published_DemandRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DuPont.Interface;
using DuPont.Utility;
using DuPont.Models.Models;




namespace DuPont.Repository
{
    public class Published_DemandRepository : IPublished_Demand
    {
        private Func<T_SYS_DICTIONARY, string> getDisplayName = (dic) =>
        {
            if (dic != null)
            {
                return dic.DisplayName;
            }
            return "";
        };

        private static readonly Func<T_USER, string> getUserName = (user) =>
        {
            if (user != null)
            {
                return user.UserName ?? "";
            }
            return "";
        };
        /// <summary>
        /// 需求发布状态数组
        /// </summary>
        //产业商进行中
        public readonly int[] businessing = { 100501, 100502, 100503 };
        //产业商已关闭
        public readonly int[] businessed = { 100504, 100505, 1005056 };

        //大农户进行中
        public readonly int[] farmering = { 100501, 100502 };
        //大农户已关闭
        public readonly int[] farmered = { 100503, 100504, 100505, 1005056 };

        //大农户我的发布状态（进行中）
        public readonly int[] farmerDemandOnline = { 100501, 100502 };//待响应、待评价
        //大农户我的发布状态（已关闭）
        public readonly int[] farmerDemandOffline = { 100503, 100504, 100505, 100506 };//已评价、已取消、系统关闭、已关闭


        public int Insert(T_FARMER_PUBLISHED_DEMAND entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_FARMER_PUBLISHED_DEMAND.Add(entity);
                return dbContext.SaveChanges();
            }
        }

        public int Insert(IEnumerable<T_FARMER_PUBLISHED_DEMAND> entities)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_FARMER_PUBLISHED_DEMAND.AddRange(entities);
                return dbContext.SaveChanges();
            }
        }

        public int Delete(object id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_FARMER_PUBLISHED_DEMAND.Find(id);
                if (dbEntry != null)
                {
                    dbContext.T_FARMER_PUBLISHED_DEMAND.Remove(dbEntry);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Delete(T_FARMER_PUBLISHED_DEMAND entity)
        {
            if (entity != null)
            {
                using (var dbContext = new DuPont_TestContext())
                {
                    dbContext.T_FARMER_PUBLISHED_DEMAND.Remove(entity);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Delete(IEnumerable<T_FARMER_PUBLISHED_DEMAND> entities)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_FARMER_PUBLISHED_DEMAND.RemoveRange(entities);
                return dbContext.SaveChanges();
            }
        }

        public int Delete(Expression<Func<T_FARMER_PUBLISHED_DEMAND, bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_FARMER_PUBLISHED_DEMAND.SingleOrDefault(predicate);
                if (dbEntry != null)
                {
                    dbContext.T_FARMER_PUBLISHED_DEMAND.Remove(dbEntry);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Update(T_FARMER_PUBLISHED_DEMAND entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_FARMER_PUBLISHED_DEMAND.Find(entity.Id);
                if (dbEntry != null)
                {
                    ClassValueCopyHelper.Copy(dbEntry, entity);
                    return dbContext.SaveChanges();
                }
            }
            return 0;

        }

        public T_FARMER_PUBLISHED_DEMAND GetByKey(object key)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_FARMER_PUBLISHED_DEMAND.Find(key);
            }
        }

        public IList<T_FARMER_PUBLISHED_DEMAND> GetAll()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_FARMER_PUBLISHED_DEMAND.ToList();
            }
        }


        public IList<T_FARMER_PUBLISHED_DEMAND> GetAll(System.Linq.Expressions.Expression<Func<T_FARMER_PUBLISHED_DEMAND, bool>> predicate)
        {

            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntryList = dbContext.T_FARMER_PUBLISHED_DEMAND.Where(predicate);
                if (dbEntryList != null)
                {
                    return dbEntryList.ToList();
                }
            }
            return null;
        }
        #region 我的发布

        /// <summary>
        /// 产业商我的发布
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="IsClosed"></param>
        /// <param name="roletype"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        public List<PublishedModel> GetBusinessPublishedRequirement(long userid, int IsClosed, int roletype, int PageIndex, int PageSize, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //我的发布--进行中
                if (IsClosed == 0)
                {
                    TotalNums = (from bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND
                                 where bpd.CreateUserId == userid
                                       && (bpd.PublishStateId == 100501
                                       || bpd.PublishStateId == 100502
                                       || bpd.PublishStateId == 100503
                                 )
                                 select bpd.Id).Count();

                    var myPublishedList = (from bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND
                                           where bpd.CreateUserId == userid
                                                     && (bpd.PublishStateId == 100501
                                                     || bpd.PublishStateId == 100502
                                                     || bpd.PublishStateId == 100503
                                               )
                                           orderby bpd.CreateTime descending
                                           select bpd)
                             .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                             .ToList()
                             .Select(
                                model => new PublishedModel
                                {
                                    Name = dbContext.Set<T_USER>().Where(u => u.Id == model.CreateUserId).FirstOrDefault().UserName != null ? dbContext.Set<T_USER>().Where(u => u.Id == model.CreateUserId).FirstOrDefault().UserName : "",
                                    Id = model.Id,
                                    RequirementTypeId = model.DemandTypeId,
                                    RequirementType = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                    Remark = model.Brief ?? "",
                                    PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                    Dates = model.ExpectedDate != null ? model.ExpectedDate : "",
                                    AddressCode = model.Province + "|" + model.City + "|" + model.Region + "|" + model.Township + "|" + model.Village,
                                    Address = GetAreaName(model.Province) + "" + GetAreaName(model.City) + "" + GetAreaName(model.Region) + "" + GetAreaName(model.Township) + "" + GetAreaName(model.Village),
                                    DetailAddress = model.DetailedAddress ?? "",
                                    PhoneNumber = model.PhoneNumber,
                                    PublishStateId = model.PublishStateId,
                                    PublishState = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                    ExpectedStartPrice = Convert.ToDouble(model.ExpectedStartPrice),
                                    ExpectedEndPrice = Convert.ToDouble(model.ExpectedEndPrice),
                                    PurchaseWeight = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.AcquisitionWeightRangeTypeId).FirstOrDefault()),
                                    PurchaseWeightId = model.AcquisitionWeightRangeTypeId,
                                    CommenceWeight = "",
                                    CommenceWeightId = model.FirstWeight,
                                    Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.FirstWeight).FirstOrDefault()),
                                    AcreageId = 0,
                                    Crop = "",
                                    CropId = 0,
                                    Credit = 0,
                                    Distance = 0,
                                    Lat = "0",
                                    Lng = "0"
                                }).ToList();

                    return myPublishedList;
                }
                //我的发布--已关闭
                else if (IsClosed != 0)
                {
                    TotalNums = (from bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND
                                 where bpd.CreateUserId == userid
                                       && (bpd.PublishStateId == 100504
                                       || bpd.PublishStateId == 100505
                                       || bpd.PublishStateId == 100506
                                 )
                                 select bpd.Id).Count();
                    var myPublishedList = (from bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND
                                           where bpd.CreateUserId == userid
                                                  && (bpd.PublishStateId == 100504
                                                  || bpd.PublishStateId == 100505
                                                  || bpd.PublishStateId == 100506
                                               )
                                           orderby bpd.CreateTime descending
                                           select bpd)
                             .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                             .ToList()
                             .Select(
                                model => new PublishedModel
                                {
                                    Name = dbContext.Set<T_USER>().Where(u => u.Id == model.CreateUserId).FirstOrDefault().UserName != null ? dbContext.Set<T_USER>().Where(u => u.Id == model.CreateUserId).FirstOrDefault().UserName : "",
                                    Id = model.Id,
                                    RequirementTypeId = model.DemandTypeId,
                                    RequirementType = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                    Remark = model.Brief ?? "",
                                    PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                    Dates = model.ExpectedDate != null ? model.ExpectedDate : "",
                                    AddressCode = model.Province + "|" + model.City + "|" + model.Region + "|" + model.Township + "|" + model.Village,
                                    Address = GetAreaName(model.Province) + "" + GetAreaName(model.City) + "" + GetAreaName(model.Region) + "" + GetAreaName(model.Township) + "" + GetAreaName(model.Village),
                                    DetailAddress = model.DetailedAddress ?? "",
                                    PhoneNumber = model.PhoneNumber,
                                    PublishStateId = model.PublishStateId,
                                    PublishState = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                    ExpectedStartPrice = Convert.ToDouble(model.ExpectedStartPrice),
                                    ExpectedEndPrice = Convert.ToDouble(model.ExpectedEndPrice),
                                    PurchaseWeight = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.AcquisitionWeightRangeTypeId).FirstOrDefault()),
                                    PurchaseWeightId = model.AcquisitionWeightRangeTypeId,
                                    CommenceWeight = "",
                                    CommenceWeightId = model.FirstWeight,
                                    Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.FirstWeight).FirstOrDefault()),
                                    AcreageId = 0,
                                    Crop = "",
                                    CropId = 0,
                                    Credit = 0,
                                    Distance = 0,
                                    Lat = "0",
                                    Lng = "0"
                                }).ToList();

                    return myPublishedList;
                }

                TotalNums = 0;
                return null;
            }
        }

        /// <summary>
        /// 大农户我的发布
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="IsClosed"></param>
        /// <param name="roletype"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        public List<PublishedModel> GetFarmerPublishedRequirement(long userid, int IsClosed, int roletype, int PageIndex, int PageSize, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //我的发布--进行中 //ww增加大农户已评价100507、农机手已评价100508

                if (IsClosed == 0)
                {
                    TotalNums = (from fpd in dbContext.T_FARMER_PUBLISHED_DEMAND
                                 where fpd.CreateUserId == userid
                                     && (
                                         fpd.PublishStateId == 100501
                                         || fpd.PublishStateId == 100502
                                         || fpd.PublishStateId== 100507
                                         || fpd.PublishStateId == 100508
                                     )
                                 select fpd.Id).Count();

                    var myPublishedList = (from fpd in dbContext.T_FARMER_PUBLISHED_DEMAND
                                           where fpd.CreateUserId == userid
                                               && (
                                                   fpd.PublishStateId == 100501
                                                   || fpd.PublishStateId == 100502
                                                   || fpd.PublishStateId == 100507
                                                   || fpd.PublishStateId == 100508
                                               )
                                           orderby fpd.CreateTime descending
                                           select fpd)
                                         .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                                         .ToList()
                                         .Select(
                                                model =>
                                                {
                                                    var item = new PublishedModel
                                                    {
                                                        Name = getUserName(dbContext.Set<T_USER>().Where(u => u.Id == model.CreateUserId).FirstOrDefault()),
                                                        Id = model.Id,
                                                        RequirementTypeId = model.DemandTypeId,
                                                        RequirementType = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                                        Remark = model.Brief ?? "",
                                                        Dates = model.ExpectedDate ?? "",
                                                        PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),

                                                        AddressCode = model.Province + "|" + model.City + "|" + model.Region + "|" + model.Township + "|" + model.Village,

                                                        Address = GetAreaName(model.Province) + "" + GetAreaName(model.City) + "" + GetAreaName(model.Region) + "" + GetAreaName(model.Township) + "" + GetAreaName(model.Village),
                                                        DetailAddress = model.DetailedAddress ?? "",

                                                        PhoneNumber = model.PhoneNumber,
                                                        PublishStateId = model.PublishStateId,
                                                        PublishState = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                                        ExpectedStartPrice = Convert.ToDouble(model.ExpectedStartPrice),
                                                        ExpectedEndPrice = Convert.ToDouble(model.ExpectedEndPrice),
                                                        Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.AcresId).FirstOrDefault()),
                                                        AcreageId = model.AcresId,
                                                        Crop = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.CropId).FirstOrDefault()),
                                                        CropId = model.CropId,
                                                        CommenceWeightId = 0,
                                                        CommenceWeight = "",
                                                        PurchaseWeightId = 0,
                                                        PurchaseWeight = "",
                                                        Credit = 0,
                                                        Distance = 0,
                                                        Lat = "0",
                                                        Lng = "0"
                                                    };
                                                    return item;
                                                }).ToList();

                    return myPublishedList;
                }
                //我的发布--已关闭
                else if (IsClosed != 0)
                {
                    TotalNums = (from fpd in dbContext.T_FARMER_PUBLISHED_DEMAND
                                 join fdrr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION on fpd.Id equals fdrr.DemandId into joined_fdrr_fpt
                                 from fdrr in joined_fdrr_fpt.DefaultIfEmpty()
                                 where fpd.CreateUserId == userid
                                     && (
                                         fpd.PublishStateId == 100503
                                         || fpd.PublishStateId == 100504
                                         || fpd.PublishStateId == 100505
                                         || fpd.PublishStateId == 100506
                                         //|| fdrr.Score > 0
                                         //|| fdrr.Comments != null ww 有状态值判断了，没必要用评价农机手内容和几级来判断了
                                     )
                                 select fpd.Id).Count();

                    var myPublishedList = (from fpd in dbContext.T_FARMER_PUBLISHED_DEMAND
                                           join fdrr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION on fpd.Id equals fdrr.DemandId into joined_fdrr_fpt
                                           from fdrr in joined_fdrr_fpt.DefaultIfEmpty()
                                           where fpd.CreateUserId == userid
                                               && (
                                                   fpd.PublishStateId == 100503
                                                   || fpd.PublishStateId == 100504
                                                   || fpd.PublishStateId == 100505
                                                   || fpd.PublishStateId == 100506
                                                 //|| fdrr.Score > 0
                                                 //|| fdrr.Comments != null ww 有状态值判断了，没必要用评价农机手内容和几级来判断了
                                               )
                                           orderby fpd.CreateTime descending
                                           select fpd)
                                         .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                                         .ToList()
                                         .Select(
                                                model =>
                                                {
                                                    var item = new PublishedModel
                                                    {
                                                        Name = getUserName(dbContext.Set<T_USER>().Where(u => u.Id == model.CreateUserId).FirstOrDefault()),
                                                        Id = model.Id,
                                                        RequirementTypeId = model.DemandTypeId,
                                                        RequirementType = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                                        Remark = model.Brief ?? "",
                                                        Dates = model.ExpectedDate ?? "",
                                                        PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),

                                                        AddressCode = model.Province + "|" + model.City + "|" + model.Region + "|" + model.Township + "|" + model.Village,

                                                        Address = GetAreaName(model.Province) + "" + GetAreaName(model.City) + "" + GetAreaName(model.Region) + "" + GetAreaName(model.Township) + "" + GetAreaName(model.Village),
                                                        DetailAddress = model.DetailedAddress ?? "",

                                                        PhoneNumber = model.PhoneNumber,
                                                        PublishStateId = model.PublishStateId,
                                                        PublishState = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                                        ExpectedStartPrice = Convert.ToDouble(model.ExpectedStartPrice),
                                                        ExpectedEndPrice = Convert.ToDouble(model.ExpectedEndPrice),
                                                        Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.AcresId).FirstOrDefault()),
                                                        AcreageId = model.AcresId,
                                                        Crop = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.CropId).FirstOrDefault()),
                                                        CropId = model.CropId,
                                                        CommenceWeightId = 0,
                                                        CommenceWeight = "",
                                                        PurchaseWeightId = 0,
                                                        PurchaseWeight = "",
                                                        Credit = 0,
                                                        Distance = 0,
                                                        Lat = "0",
                                                        Lng = "0"
                                                    };
                                                    return item;
                                                }).ToList();

                    return myPublishedList;
                }

                TotalNums = 0;
                return null;
            }
        }

        public string GetAreaName(string code)
        {
            string areaName = "";
            if (!string.IsNullOrEmpty(code))
            {
                using (var dbContext = new DuPont_TestContext())
                {
                    var model = dbContext.T_AREA.Where(a => a.AID == code).FirstOrDefault();
                    if (model != null)
                    {
                        areaName = model.DisplayName;
                    }
                }
            }
            return areaName;
        }
        #endregion

        #region 需要系统服务30天修改订单状态为已完成
        /// <summary>
        /// 大农户的需求列表
        /// </summary>
        /// <author>ww</author>
        /// <returns></returns>
        public List<FarmerDemand> GetFarmerDemandList(int pageindex, int pagesize, int isclosed, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //大农户的需求
                List<FarmerDemand> list = new List<FarmerDemand>();
                //进行中
                if (isclosed == 0)
                {
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 where  fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
                                       &&
                                       (fpd.PublishStateId == 100502
                                        || fpd.PublishStateId == 100507
                                        || fpd.PublishStateId == 100508
                                       )
                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }

                    list = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                            join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id                            
                            where  fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
                                       && (fpd.PublishStateId == 100502
                                        || fpd.PublishStateId == 100507
                                        || fpd.PublishStateId == 100508
                                       )
                            orderby drr.CreateTime 
                            select new
                            {
                                drr.Id,                                
                                fpd.DemandTypeId,
                                drr.DemandId,
                                drr.CreateTime,
                                fpd.Province,
                                fpd.City,
                                fpd.Region,
                                fpd.Township,
                                fpd.Village,
                                fpd.DetailedAddress,
                                fpd.PhoneNumber,
                                fpd.Brief,
                                fpd.PublishStateId,
                                fpd.AcresId,                                
                                fpd.CreateUserId                                
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                model =>
                                {
                                    var item = new FarmerDemand
                                    {
                                        Id = model.Id,
                                        DemandId=model.DemandId,
                                        PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                        Address = GetAreaName(model.Province)
                                                    + GetAreaName(model.City)
                                                    + GetAreaName(model.Region)
                                                    + GetAreaName(model.Township)
                                                    + GetAreaName(model.Village),
                                        DetailAddress = model.DetailedAddress ?? "",
                                        PhoneNumber = model.PhoneNumber,
                                        Remark = model.Brief ?? "",      
                                        PublisherUserId = model.CreateUserId,
                                        RequirementTypeId = model.DemandTypeId,
                                        PublishStateId = model.PublishStateId,
                                        ReceiveDate=model.CreateTime,                                       
                                    };
                                    return item;
                                }).ToList();
                }               
                else
                {
                    TotalNums = 0;
                    list = null;
                }
                return list;
            }

        }
        #endregion

        #region 需要系统服务根据订单时间修改状态的产业商需求列表
        /// <summary>
        /// 产业商的需求列表
        /// </summary>
        /// <author>ww</author>
        /// <returns></returns>
        public List<PublishedModel> GetBusinessDemandList(int pageindex, int pagesize, int isclosed, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //产业商的需求
                List<PublishedModel> list = new List<PublishedModel>();
               //我的发布--进行中
                if (isclosed == 0)
                {
                    TotalNums = (from bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND
                                 where bpd.IsDeleted==false
                                       && (bpd.PublishStateId == 100501
                                       || bpd.PublishStateId == 100502
                                       || bpd.PublishStateId == 100503
                                 )
                                 select bpd.Id).Count();

                   list= (from bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND
                                           where bpd.IsDeleted == false
                                                     && (bpd.PublishStateId == 100501
                                                     || bpd.PublishStateId == 100502
                                                     || bpd.PublishStateId == 100503
                                               )
                                           orderby bpd.CreateTime ascending
                                           select bpd)
                             .Skip((pageindex - 1) * pagesize).Take(pagesize)
                             .ToList()
                             .Select(
                                model => new PublishedModel
                                {
                                    Name = "",//dbContext.Set<T_USER>().Where(u => u.Id == model.CreateUserId).FirstOrDefault().UserName != null ? dbContext.Set<T_USER>().Where(u => u.Id == model.CreateUserId).FirstOrDefault().UserName : "",
                                    Id = model.Id,
                                    RequirementTypeId = model.DemandTypeId,
                                    RequirementType = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                    Remark = model.Brief ?? "",
                                    PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                    Dates = model.ExpectedDate != null ? model.ExpectedDate : "",
                                    AddressCode = model.Province + "|" + model.City + "|" + model.Region + "|" + model.Township + "|" + model.Village,
                                    Address = GetAreaName(model.Province) + "" + GetAreaName(model.City) + "" + GetAreaName(model.Region) + "" + GetAreaName(model.Township) + "" + GetAreaName(model.Village),
                                    DetailAddress = model.DetailedAddress ?? "",
                                    PhoneNumber = model.PhoneNumber,
                                    PublishStateId = model.PublishStateId,
                                    PublishState = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                    ExpectedStartPrice = Convert.ToDouble(model.ExpectedStartPrice),
                                    ExpectedEndPrice = Convert.ToDouble(model.ExpectedEndPrice),
                                    PurchaseWeight = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.AcquisitionWeightRangeTypeId).FirstOrDefault()),
                                    PurchaseWeightId = model.AcquisitionWeightRangeTypeId,
                                    CommenceWeight = "",
                                    CommenceWeightId = model.FirstWeight,
                                    Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.FirstWeight).FirstOrDefault()),
                                    AcreageId = 0,
                                    Crop = "",
                                    CropId = 0,
                                    Credit = 0,
                                    Distance = 0,
                                    Lat = "0",
                                    Lng = "0"
                                }).ToList();

                    return list;
                }
                else
                {
                    TotalNums = 0;
                    list = null;
                }
                return list;
            }

        }
        #endregion
    }
}
