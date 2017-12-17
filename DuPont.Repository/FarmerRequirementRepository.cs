// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 王宁
// Created          : 08-12-2015
//
// Last Modified By : 王宁
// Last Modified On : 08-12-2015
// ***********************************************************************
// <copyright file="RequirementRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************




using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Interface;
using DuPont.Models;
using DuPont.Models.Dtos.Background.Demand;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using System.Text;
using DuPont.Global.Common;

namespace DuPont.Repository
{
    public class FarmerRequirementRepository : IFarmerRequirement
    {
        protected static readonly int[] closedStateArray = new int[] { 100504, 100505, 100506 };
        protected static Func<int, bool> IsClosedState = (state) =>
        {
            return closedStateArray.Contains(state);
        };

        public int Insert(T_FARMER_PUBLISHED_DEMAND entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_FARMER_PUBLISHED_DEMAND.Add(entity);
                return dbContext.SaveChanges();
            }
        }

        #region "分页获取全部数据"
        /// <summary>
        /// 分页获取全部数据
        /// </summary>
        /// <param name="SeachModel"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<T_FARMER_PUBLISHED_DEMAND> GetAll(FarmerSeachModel model, out int totalCount)
        {
            using (var dbContext = new DuPont_TestContext())
            {

                var predicate = PredicateBuilder.True<T_FARMER_PUBLISHED_DEMAND>();
                //发布状态过滤
                if (model.PublishStateId.IsNullOrEmpty() == false && model.PublishStateId != "0")
                {
                    long stateId = Convert.ToInt64(model.PublishStateId);
                    predicate = predicate.And(p => p.PublishStateId == stateId);
                }

                //需求状态过滤
                if (model.DemandTypeId.IsNullOrEmpty() == false && model.DemandTypeId != "0")
                {
                    long demandTypeId = Convert.ToInt64(model.DemandTypeId);
                    predicate = predicate.And(p => p.DemandTypeId == demandTypeId);
                }

                //省份过滤
                if (model.ProvinceAid.IsNullOrEmpty() == false && model.ProvinceAid != "0")
                {
                    predicate = predicate.And(p => p.Province == model.ProvinceAid);
                }

                //城市过滤
                if (model.CityAid.IsNullOrEmpty() == false && model.CityAid != "0")
                {
                    predicate = predicate.And(p => p.City == model.CityAid);
                }

                //地区过滤
                if (model.RegionAid.IsNullOrEmpty() == false && model.RegionAid != "0")
                {
                    predicate = predicate.And(p => p.Region == model.RegionAid);
                }

                //删除状态过滤
                if (model.IsDeleted.HasValue)
                {
                    predicate = predicate.And(p => p.IsDeleted == model.IsDeleted.Value);
                }

                //符合条件的总记录数
                totalCount = dbContext.T_FARMER_PUBLISHED_DEMAND.Where(predicate).OrderByDescending(a => a.CreateTime).Count();

                //返回符合条件的记录
                return dbContext.T_FARMER_PUBLISHED_DEMAND.Where(predicate).OrderByDescending(a => a.CreateTime)
                    .Skip((model.pageIndex - 1) * model.pageSize)
                    .Take(model.pageSize).ToList();
            }
        }
        #endregion

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

        #region "GetAll"
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
        #endregion

        #region "获取大农户需求详情"
        /// <summary>
        /// 获取大农户需求详情
        /// </summary>
        /// <param name="id">需求id</param>
        /// <returns>FarmerPublishedDetailsModel.</returns>
        public FarmerPublishedDetailsModel GetFarmerDetail(long id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var replyUserRoleType = RoleType.Unknown;

                T_FARMER_PUBLISHED_DEMAND tmodel = dbContext.Set<T_FARMER_PUBLISHED_DEMAND>().Where(f => f.Id == id).FirstOrDefault();
                if (tmodel != null)
                {
                    //获取响应列表(产业商和农机手响应大农户的需求)
                    //应答列表中的地址是大农户发布需求的地址

                    //判断该需求是发布给农机手的还是产业商的
                    var demandTypeId = tmodel.DemandTypeId.ToString().Substring(0, 4);

                    //给农机手的
                    if (demandTypeId == "1001")
                        replyUserRoleType = RoleType.MachineryOperator;
                    //发布给产业商的
                    else if (demandTypeId == "1008")
                        replyUserRoleType = RoleType.Business;

                    List<ReplyDetailModel> rlist = dbContext.Set<T_FARMER_DEMAND_RESPONSE_RELATION>()
                        .Where(f => f.DemandId == id).ToList().Select(f => new ReplyDetailModel
                    {
                        ReplyUserId = f.UserId,
                        ReplyUserName = dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().UserName != null ? dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().UserName : "",
                        ReplyPhoneNumber = dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().PhoneNumber != null ? dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().PhoneNumber : "",
                        ReplyDetailedAddress = GetAreaName(dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().Province) + GetAreaName(dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().City) + GetAreaName(dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().Region) + GetAreaName(dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().Township) + GetAreaName(dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().Village),
                        ReplyTime = Utility.TimeHelper.GetMilliSeconds(f.CreateTime),
                        ReplyRemark = "",
                        Score = f.Score,
                        ReplyWeightId = 0,
                        ReplyWeight = "",
                        RoleId = (int)replyUserRoleType,
                        RoleName = replyUserRoleType.GetDescription()
                    }).ToList();

                    var type = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.DemandTypeId).FirstOrDefault();
                    string typestring = type != null ? type.DisplayName : "";

                    var publishstate = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.PublishStateId).FirstOrDefault();
                    string publishstatestring = publishstate != null ? publishstate.DisplayName : "";

                    var name = dbContext.Set<T_USER>().Where(u => u.Id == tmodel.CreateUserId).FirstOrDefault();
                    string names = name != null ? name.UserName : "";
                    string namestring = names != null ? names : "";

                    var level = dbContext.Set<T_USER_ROLE_RELATION>().Where(u => u.MemberType && u.UserID == tmodel.CreateUserId).FirstOrDefault();
                    long? levelstring = level != null ? level.Star : 0;

                    var crops = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.CropId).FirstOrDefault();
                    string cropstring = crops != null ? crops.DisplayName : "";
                    FarmerPublishedDetailsModel model = new FarmerPublishedDetailsModel()
                    {
                        UserId = tmodel.CreateUserId,
                        UserName = namestring,
                        UserLevel = levelstring,
                        Id = tmodel.Id,
                        CropId = tmodel.CropId,
                        Crop = cropstring,
                        TypeId = tmodel.DemandTypeId,
                        Type = typestring,
                        AcreageId = tmodel.AcresId,
                        Acreage = MyCommons.Get_SysDictionary_DisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.AcresId).FirstOrDefault()),
                        Remark = tmodel.Brief != null ? tmodel.Brief : "",
                        Dates = StringHelper.TransfeDate(tmodel.ExpectedDate),
                        PublishedDate = Utility.TimeHelper.GetMilliSeconds(tmodel.CreateTime),
                        Address = GetAreaName(tmodel.Province) + "" + GetAreaName(tmodel.City) + "" + GetAreaName(tmodel.Region) + "" + GetAreaName(tmodel.Township) + "" + GetAreaName(tmodel.Village),
                        DetailAddress = tmodel.DetailedAddress != null ? tmodel.DetailedAddress : "",
                        PhoneNumber = tmodel.PhoneNumber,
                        ExpectedStartPrice = Convert.ToDouble(tmodel.ExpectedStartPrice),
                        ExpectedEndPrice = Convert.ToDouble(tmodel.ExpectedEndPrice),
                        PublishStateId = tmodel.PublishStateId,
                        PublishState = publishstatestring,
                        ReplyList = rlist
                    };
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region "获取产业商需求详情"
        /// <summary>
        /// 获取产业商需求详情
        /// </summary>
        /// <param name="id">需求id</param>
        /// <returns>BusinessPublishedDetailsModel.</returns>
        public BusinessPublishedDetailsModel GetBusinessDetail(long id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                T_BUSINESS_PUBLISHED_DEMAND tmodel = dbContext.Set<T_BUSINESS_PUBLISHED_DEMAND>().Where(b => b.Id == id).FirstOrDefault();
                if (tmodel != null)
                {
                    //获取响应列表(大农户响应产业商的需求)
                    //应答列表中的地址是大农户响应产业商需求时填写的地址
                    List<ReplyDetailModel> rlist = dbContext.Set<T_BUSINESS_DEMAND_RESPONSE_RELATION>().Where(f => f.DemandId == id).ToList().Select(f => new ReplyDetailModel
                    {
                        ReplyUserId = f.UserId,
                        ReplyUserName = dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().UserName != null ? dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().UserName : "",
                        ReplyPhoneNumber = dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().PhoneNumber != null ? dbContext.Set<T_USER>().Where(u => u.Id == f.UserId).FirstOrDefault().PhoneNumber : "",
                        ReplyDetailedAddress = f.Address,
                        ReplyTime = Utility.TimeHelper.GetMilliSeconds(f.CreateTime),
                        ReplyRemark = f.Brief != null ? f.Brief : "",
                        Score = f.Score,
                        ReplyWeightId = f.WeightRangeTypeId,
                        ReplyWeight = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == f.WeightRangeTypeId).FirstOrDefault() != null ? dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == f.WeightRangeTypeId).FirstOrDefault().DisplayName : "",
                        RoleId = (int)RoleType.Farmer,
                    }).ToList();
                    var type = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.DemandTypeId).FirstOrDefault();
                    string typestring = type != null ? type.DisplayName : "";

                    var pw = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.AcquisitionWeightRangeTypeId).FirstOrDefault();
                    string pwstring = pw != null ? pw.DisplayName : "";

                    var cw = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.FirstWeight).FirstOrDefault();
                    string cwstring = cw != null ? cw.DisplayName : "";

                    var publishstate = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.PublishStateId).FirstOrDefault();
                    string publishstatestring = publishstate != null ? publishstate.DisplayName : "";

                    var name = dbContext.Set<T_USER>().Where(u => u.Id == tmodel.CreateUserId).FirstOrDefault();
                    string names = name != null ? name.UserName : "";
                    string namestring = names != null ? names : "";

                    var level = dbContext.Set<T_USER_ROLE_RELATION>().Where(u => u.MemberType && u.UserID == tmodel.CreateUserId).FirstOrDefault();
                    long? levelstring = level != null ? level.Star : 0;

                    var crops = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.CropId).FirstOrDefault();
                    string cropstring = crops != null ? crops.DisplayName : "";
                    BusinessPublishedDetailsModel model = new BusinessPublishedDetailsModel()
                    {
                        UserId = tmodel.CreateUserId,
                        UserName = namestring,
                        UserLevel = levelstring,
                        Id = tmodel.Id,
                        TypeId = tmodel.DemandTypeId,
                        Type = typestring,
                        Remark = tmodel.Brief != null ? tmodel.Brief : "",
                        Dates = StringHelper.TransfeDate(tmodel.ExpectedDate),
                        PublishedDate = Utility.TimeHelper.GetMilliSeconds(tmodel.CreateTime),
                        Address = GetAreaName(tmodel.Province) + "" + GetAreaName(tmodel.City) + "" + GetAreaName(tmodel.Region) + "" + GetAreaName(tmodel.Township) + "" + GetAreaName(tmodel.Village),
                        DetailAddress = tmodel.DetailedAddress != null ? tmodel.DetailedAddress : "",
                        PhoneNumber = tmodel.PhoneNumber,
                        ExpectedStartPrice = Convert.ToDouble(tmodel.ExpectedStartPrice),
                        ExpectedEndPrice = Convert.ToDouble(tmodel.ExpectedEndPrice),
                        PurchaseWeightId = tmodel.AcquisitionWeightRangeTypeId,
                        CommenceWeightId = tmodel.FirstWeight,
                        CommenceWeight = cwstring,
                        PurchaseWeight = pwstring,
                        PublishStateId = tmodel.PublishStateId,
                        PublishState = publishstatestring,
                        CropId = tmodel.CropId == null ? 0 : (int)tmodel.CropId,
                        Crop = cropstring,
                        ReplyList = rlist
                    };
                    return model;
                }
                else
                {
                    return null;
                }

            }
        }
        #endregion

        #region "获取大农户发给产业商（农机手）的信息"
        /// <summary>
        /// 获取大农户发给产业商（农机手）的信息
        /// </summary>
        /// <param name="Lat">登陆者的纬度值</param>
        /// <param name="Lng">登陆者的经度值</param>
        /// <param name="type">需求类型编号</param>
        /// <param name="pronvices">省份</param>
        /// <param name="city">城市</param>
        /// <param name="region">地区</param>
        /// <param name="orderfield">排序字段</param>
        /// <param name="PageIndex">每几页</param>
        /// <param name="PageSize">每页显示数据条数</param>
        /// <param name="TotalNums">符合条件的总记录数</param>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public List<PublishedModel> GetRequirementList(double businessLat, double businessLng, int type, string pronvices, string city, string region, string orderfield, int PageIndex, int PageSize, out long TotalNums, long userId = 0)
        {
            using (var ctx = new DuPont_TestContext())
            {
                var isAssignToMe = userId > 0;
                var param_Source_Lat = new SqlParameter("@source_lat", SqlDbType.VarChar, 200);
                var param_Source_Lng = new SqlParameter("@source_lng", SqlDbType.VarChar, 200);
                var param_Demand_TypeId = new SqlParameter("@demand_typeid", SqlDbType.Int);
                var param_PageIndex = new SqlParameter("@pageindex", SqlDbType.Int, 32);
                var param_PageSize = new SqlParameter("@pagesize", SqlDbType.Int, 32);
                var param_Distance_Limit = new SqlParameter("@distance_limit", SqlDbType.Int);
                var param_TotalCount = new SqlParameter("@totalcount", SqlDbType.Int);
                var param_ResponseUserId = new SqlParameter("@response_userid", SqlDbType.BigInt);
                param_TotalCount.Direction = ParameterDirection.Output;

                param_Source_Lat.Value = businessLat;
                param_Source_Lng.Value = businessLng;
                param_Demand_TypeId.Value = type;
                param_PageIndex.Value = PageIndex;
                param_PageSize.Value = PageSize;
                param_Distance_Limit.Value = 200000;


                var distanceLitmitSettingInfo = ctx.T_SYS_SETTING.Where(s => s.SETTING_ID == "001").FirstOrDefault();
                if (distanceLitmitSettingInfo != null)
                {
                    param_Distance_Limit.Value = Convert.ToInt32(distanceLitmitSettingInfo.SETTING_VALUE);
                }

                string sql = string.Empty;
                if (isAssignToMe)
                {
                    sql = SqlStrings.GetAssignToOperatorRequirementList;
                    param_ResponseUserId.Value = userId;
                }
                else sql = SqlStrings.GetBusinessOrOperatorRequirementList;


                //地区过滤
                if (region.IsNullOrEmpty() == false && region.Length > 5 && PageValidate.IsNumber(region))
                    sql = sql.Replace("{AreaFilter}", "Region='" + region + "'");
                else if (city.IsNullOrEmpty() == false && city.Length > 5 && PageValidate.IsNumber(city))
                    sql = sql.Replace("{AreaFilter}", "City='" + city + "'");
                else if (pronvices.IsNullOrEmpty() == false && pronvices.Length > 5 && PageValidate.IsNumber(pronvices))
                    sql = sql.Replace("{AreaFilter}", "Province='" + pronvices + "'");
                else
                    sql = sql.Replace("{AreaFilter}", "1=1");

                //设置排序
                switch (orderfield)
                {
                    case "time":
                        sql = sql.Replace("{OrderField}", "CreateTime desc ");

                        break;
                    case "number":
                        sql = sql.Replace("{OrderField}", "NumberSort desc");
                        break;
                    case "distance":
                        sql = sql.Replace("{OrderField}", "Distance asc");
                        break;
                    default:
                        sql = sql.Replace("{OrderField}", "Distance asc");
                        break;
                }

                List<DemandIdWithDistanceItem> demandWithDistanceList = new List<DemandIdWithDistanceItem>();
                if (!isAssignToMe)
                {
                    demandWithDistanceList = ctx.Database.SqlQuery<DemandIdWithDistanceItem>(sql,
                        param_Source_Lat,
                        param_Source_Lng,
                        param_PageIndex,
                        param_PageSize,
                        param_Distance_Limit,
                        param_TotalCount,
                        param_Demand_TypeId
                    ).ToList();
                }
                else
                {
                    demandWithDistanceList = ctx.Database.SqlQuery<DemandIdWithDistanceItem>(sql,
                        param_Source_Lat,
                        param_Source_Lng,
                        param_PageIndex,
                        param_PageSize,
                        param_Distance_Limit,
                        param_TotalCount,
                        param_Demand_TypeId,
                        param_ResponseUserId
                    ).ToList();
                }

                //符合条件的总记录数
                TotalNums = Convert.ToInt64(param_TotalCount.Value);
                if (demandWithDistanceList.Count > 0)
                {
                    var demandIdList = demandWithDistanceList.Select(p => p.DemandId).ToList();
                    var areaIdList = new List<string>();
                    demandWithDistanceList.Select(p =>
                    {
                        if (p.Province != null && p.Province.Length > 5 && !areaIdList.Contains(p.Province))
                            areaIdList.Add(p.Province);

                        if (p.City != null && p.City.Length > 5 && !areaIdList.Contains(p.City))
                            areaIdList.Add(p.City);

                        if (p.Region != null && p.Region.Length > 5 && !areaIdList.Contains(p.Region))
                            areaIdList.Add(p.Region);

                        if (p.Township != null && p.Township.Length > 5 && !areaIdList.Contains(p.Township))
                            areaIdList.Add(p.Township);

                        if (p.Village != null && p.Village.Length > 5 && !areaIdList.Contains(p.Village))
                            areaIdList.Add(p.Village);

                        return p;
                    }).Count();

                    var demandInfoList = ctx.T_FARMER_PUBLISHED_DEMAND.Where(p => demandIdList.Contains(p.Id)).ToList();
                    var orderedDemandInfoList = new List<T_FARMER_PUBLISHED_DEMAND>();
                    for (int i = 0; i < demandWithDistanceList.Count; i++)
                    {
                        var demandId = demandWithDistanceList[i].DemandId;
                        var demand = demandInfoList.First(p => p.Id == demandId);
                        orderedDemandInfoList.Add(demand);
                    }



                    var demandCreateUserIdList = demandWithDistanceList.Select(p => p.CreateUserId).Distinct().ToList();
                    var acreageIdList = demandInfoList.Select(p => p.AcresId).Distinct().ToList();
                    var cropIdList = demandInfoList.Select(p => p.CropId).Distinct().ToList();
                    var demandTypeIdList = demandInfoList.Select(p => p.DemandTypeId).Distinct().ToList();
                    var publishStateIdList = demandInfoList.Select(p => p.PublishStateId).Distinct().ToList();

                    demandInfoList.Clear();
                    demandInfoList = null;

                    var sysDictionaryIdList = new List<int>();
                    sysDictionaryIdList.AddRange(acreageIdList);
                    sysDictionaryIdList.AddRange(cropIdList);
                    sysDictionaryIdList.AddRange(demandTypeIdList);
                    sysDictionaryIdList.AddRange(publishStateIdList);

                    //获取用户角色级别信息
                    var getUserRoleLevelSql = SqlStrings.GetUserRoleLevel;
                    getUserRoleLevelSql = getUserRoleLevelSql.Replace("{RoleId}", ((int)RoleType.Farmer).ToString())
                                                             .Replace("{UserIdList}", string.Join(",", demandCreateUserIdList));

                    var dictionaryListSql = ctx.T_SYS_DICTIONARY.Where(p => sysDictionaryIdList.Contains(p.Code)).ToString();
                    var areaListSql = ctx.T_AREA.Where(p => areaIdList.Contains(p.AID)).ToString();

                    var dsResult = new DataSet();
                    var sqlListBuilder = new StringBuilder();
                    sqlListBuilder.AppendLine(getUserRoleLevelSql + Environment.NewLine);
                    sqlListBuilder.AppendLine(dictionaryListSql + Environment.NewLine);
                    sqlListBuilder.AppendLine(areaListSql + Environment.NewLine);


                    SqlDataAdapter dap = new SqlDataAdapter(sqlListBuilder.ToString(), (SqlConnection)ctx.Database.Connection);
                    dap.Fill(dsResult);

                    var userRoleLevelList = dsResult.Tables[0].ToList<UserRoleLevelItem>();
                    var sysDictionaryList = dsResult.Tables[1].ToList<T_SYS_DICTIONARY>();
                    var areaInfoList = dsResult.Tables[2].ToList<T_AREA>();

                    return orderedDemandInfoList.Select(demandItem =>
                      {
                          var demandWithDistanceInfo = demandWithDistanceList.First(p => p.DemandId == demandItem.Id);
                          var demandPublisherUserInfo = userRoleLevelList.FirstOrDefault(p => p.UserId == demandItem.CreateUserId);
                          var model = new PublishedModel
                          {
                              IsOpen = demandItem.IsOpen,
                              Credit = 0,
                              AcreageId = demandItem.AcresId,
                              Acreage = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.AcresId)),
                              CropId = demandItem.CropId,
                              Crop = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.CropId)),
                              Id = demandItem.Id,
                              RequirementTypeId = demandItem.DemandTypeId,
                              RequirementType = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.DemandTypeId)),
                              Remark = demandItem.Brief != null ? demandItem.Brief : "",
                              Dates = demandItem.ExpectedDate != null ? demandItem.ExpectedDate : "",
                              PublishedDate = Utility.TimeHelper.GetMilliSeconds(demandItem.CreateTime),
                              AddressCode = string.Format("{0}|{1}|{2}|{3}|{4}",
                                                  demandItem.Province,
                                                  demandItem.City,
                                                  demandItem.Region,
                                                  demandItem.Township,
                                                  demandItem.Village
                                            ),
                              Address = MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.Province)) +
                                        MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.City)) +
                                        MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.Region)) +
                                        MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.Township)) +
                                        MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.Village)),
                              DetailAddress = demandItem.DetailedAddress != null ? demandItem.DetailedAddress : "",
                              PhoneNumber = demandItem.PhoneNumber,
                              PublishStateId = demandItem.PublishStateId,
                              PublishState = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.PublishStateId)),
                              ExpectedStartPrice = Convert.ToDouble(demandItem.ExpectedStartPrice),
                              ExpectedEndPrice = Convert.ToDouble(demandItem.ExpectedEndPrice),
                              Distance = demandWithDistanceInfo.Distance,
                              Lat = demandWithDistanceInfo.Lat,
                              Lng = demandWithDistanceInfo.Lng,
                              PurchaseWeightId = 0,
                              PurchaseWeight = "",
                              CommenceWeightId = 0,
                              CommenceWeight = "",
                              CreateUserId = demandItem.CreateUserId
                          };

                          if (demandPublisherUserInfo != null)
                          {
                              model.Name = demandPublisherUserInfo.UserName;
                              model.Level = demandPublisherUserInfo.Level;
                          }

                          return model;
                      }).ToList();
                }
            }

            return new List<PublishedModel>();
        }
        #endregion

        #region "大农户我的应答"
        /// <summary>
        /// 大农户我的应答
        /// </summary>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="isclosed">发布状态标识（0进行中，1已关闭）</param>
        /// <param name="userid">大农户id</param>
        /// <param name="TotalNums">The total nums.</param>
        /// <returns>List&lt;ReplyModel&gt;.</returns>
        public List<ReplyModel> GetReplyList(int pageindex, int pagesize, int isclosed, long userid, out long TotalNums)
        {
            //大农户我的应答列表中的地址是产业商发布需求时的地址
            using (var dbContext = new DuPont_TestContext())
            {
                List<ReplyModel> list = new List<ReplyModel>();
                //进行中
                if (isclosed == 0)
                {
                    TotalNums = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                                 join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                                 where (bpd.PublishStateId == 100502 || bpd.PublishStateId == 100503) && drr.UserId == userid
                                 && drr.Comments == null && drr.Score == 0
                                 select drr.Id).Count();
                    if (TotalNums == 0)
                    {
                        return null;
                    }


                    list = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                            join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                            join user in dbContext.T_USER on bpd.CreateUserId equals user.Id
                            where (bpd.PublishStateId == 100502 || bpd.PublishStateId == 100503) && drr.UserId == userid
                            && drr.Comments == null && drr.Score == 0
                            orderby drr.CreateTime descending
                            select new
                            {
                                drr.Id,
                                bpd.DemandTypeId,
                                bpd.CreateTime,
                                bpd.Province,
                                bpd.City,
                                bpd.Region,
                                bpd.Township,
                                bpd.Village,
                                bpd.DetailedAddress,
                                bpd.PhoneNumber,
                                bpd.Brief,
                                bpd.PublishStateId,
                                bpd.FirstWeight,
                                user.UserName,
                                bpd.CreateUserId,
                                drr.Comments,
                                drr.Score
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                               .Select(model =>
                               {
                                   var item = new ReplyModel
                                   {
                                       Id = model.Id,
                                       PublisherUserName = model.UserName,
                                       Requirement = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                       PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                       Address = GetAreaName(model.Province)
                                                    + GetAreaName(model.City)
                                                    + GetAreaName(model.Region)
                                                    + GetAreaName(model.Township)
                                                    + GetAreaName(model.Village),
                                       DetailAddress = model.DetailedAddress != null ? model.DetailedAddress : "",
                                       PhoneNumber = model.PhoneNumber,
                                       Remark = model.Brief != null ? model.Brief : "",
                                       //Status = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                       Acreage = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.FirstWeight).Take(1).FirstOrDefault()),
                                       RoleId = (int)RoleType.Business,
                                       RoleName = RoleType.Business.GetDescription(),
                                       PublisherUserId = model.CreateUserId
                                   };
                                   var publishStateId = 0;
                                   if (string.IsNullOrEmpty(model.Comments) || model.Score == 0)
                                   {
                                       publishStateId = 100502;
                                   }
                                   else
                                   {
                                       publishStateId = 100503;
                                   }
                                   item.Status = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == publishStateId).FirstOrDefault());
                                   item.RequireTypeName = item.Requirement;
                                   return item;
                               }).ToList();
                }
                else if (isclosed != 0)//已关闭
                {
                    TotalNums = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                                 join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                                 where drr.UserId == userid && (drr.Score > 0
                                 || drr.Comments != null
                                 || bpd.PublishStateId == 100504
                                 || bpd.PublishStateId == 100505
                                 || bpd.PublishStateId == 100506)
                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }
                    list = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                            join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                            join user in dbContext.T_USER on bpd.CreateUserId equals user.Id
                            where drr.UserId == userid && (drr.Score > 0
                                 || drr.Comments != null
                                 || bpd.PublishStateId == 100504
                                 || bpd.PublishStateId == 100505
                                 || bpd.PublishStateId == 100506)
                            orderby drr.CreateTime descending
                            select new
                            {
                                Id = drr.Id,
                                bpd.DemandTypeId,
                                bpd.CreateTime,
                                bpd.Province,
                                bpd.City,
                                bpd.Region,
                                bpd.Township,
                                bpd.Village,
                                bpd.DetailedAddress,
                                bpd.PhoneNumber,
                                bpd.Brief,
                                bpd.PublishStateId,
                                bpd.FirstWeight,
                                user.UserName,
                                bpd.CreateUserId,
                                drr.Score,
                                drr.Comments
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                               .Select(model =>
                               {

                                   var item = new ReplyModel
                                       {
                                           Id = model.Id,
                                           PublisherUserName = model.UserName,
                                           Requirement = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                           PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                           Address = GetAreaName(model.Province)
                                                        + GetAreaName(model.City)
                                                        + GetAreaName(model.Region)
                                                        + GetAreaName(model.Township)
                                                        + GetAreaName(model.Village),
                                           DetailAddress = model.DetailedAddress ?? "",
                                           PhoneNumber = model.PhoneNumber,
                                           Remark = model.Brief ?? "",
                                           Acreage = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.FirstWeight).Take(1).FirstOrDefault()),
                                           RoleId = (int)RoleType.Business,
                                           RoleName = RoleType.Business.GetDescription(),
                                           PublisherUserId = model.CreateUserId
                                       };
                                   if (!IsClosedState(model.PublishStateId) && (model.Score > 0 || model.Comments != null))
                                   {
                                       item.Status = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == 100503).FirstOrDefault());
                                   }
                                   else
                                   {
                                       item.Status = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == model.PublishStateId).FirstOrDefault());
                                   }
                                   item.RequireTypeName = item.Requirement;
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

        #region "大农户应答详情"
        /// <summary>
        /// 大农户应答详情
        /// </summary>
        /// <param name="id">需求id</param>
        /// <returns>FarmerReplyDetailModel.</returns>
        public FarmerReplyDetailModel GetReplyDetail(long id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //大农户应答详情包括订单详情和应答详情，订单详情中的地址是产业商发布需求的地址，应答详情中的地址是大农户响应产业商需求时填写的地址
                var rmodel = dbContext.Set<T_BUSINESS_DEMAND_RESPONSE_RELATION>().Where(b => b.Id == id).FirstOrDefault();
                if (rmodel != null)
                {
                    T_BUSINESS_PUBLISHED_DEMAND tmodel = dbContext.Set<T_BUSINESS_PUBLISHED_DEMAND>().Where(b => b.Id == rmodel.DemandId).FirstOrDefault();
                    if (tmodel != null)
                    {
                        var type = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.DemandTypeId).FirstOrDefault();
                        string typestring = type != null ? type.DisplayName : "";

                        var pw = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.AcquisitionWeightRangeTypeId).FirstOrDefault();
                        string pwstring = pw != null ? pw.DisplayName : "";

                        var cw = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.FirstWeight).FirstOrDefault();
                        string cwstring = cw != null ? cw.DisplayName : "";


                        string publishstatestring = string.Empty;

                        if (!IsClosedState(tmodel.PublishStateId) && (rmodel.Score > 0 || rmodel.Comments != null))
                        {
                            publishstatestring = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == 100503).FirstOrDefault());
                        }
                        else
                        {
                            publishstatestring = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == tmodel.PublishStateId).FirstOrDefault());
                        }



                        var name = dbContext.Set<T_USER>().Where(u => u.Id == tmodel.CreateUserId).FirstOrDefault();
                        string names = name != null ? name.UserName : "";
                        string namestring = names != null ? names : "";

                        var level = dbContext.Set<T_USER_ROLE_RELATION>().Where(u => u.MemberType && u.UserID == tmodel.CreateUserId).FirstOrDefault();
                        long? levelstring = level != null ? level.Star : 0;

                        var replyusername = dbContext.Set<T_USER>().Where(u => u.Id == rmodel.UserId).FirstOrDefault();
                        string replynames = replyusername != null ? replyusername.UserName : "";
                        string replynamestring = replynames != null ? replynames : "";

                        var weight = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == rmodel.WeightRangeTypeId).FirstOrDefault();
                        string replyweight = weight != null ? weight.DisplayName : "";

                        FarmerReplyDetailModel model = new FarmerReplyDetailModel()
                        {
                            UserId = tmodel.CreateUserId,
                            UserName = namestring,
                            UserLevel = levelstring,
                            Id = tmodel.Id,
                            TypeId = tmodel.DemandTypeId,
                            Type = typestring,
                            Remark = tmodel.Brief != null ? tmodel.Brief : "",
                            Dates = StringHelper.TransfeDate(tmodel.ExpectedDate),
                            PublishedDate = Utility.TimeHelper.GetMilliSeconds(tmodel.CreateTime),
                            Address = GetAreaName(tmodel.Province) + "" + GetAreaName(tmodel.City) + "" + GetAreaName(tmodel.Region) + "" + GetAreaName(tmodel.Township) + "" + GetAreaName(tmodel.Village),
                            DetailAddress = tmodel.DetailedAddress != null ? tmodel.DetailedAddress : "",
                            PhoneNumber = tmodel.PhoneNumber,
                            ExpectedStartPrice = Convert.ToDouble(tmodel.ExpectedStartPrice),
                            ExpectedEndPrice = Convert.ToDouble(tmodel.ExpectedEndPrice),
                            PurchaseWeightId = tmodel.AcquisitionWeightRangeTypeId,
                            CommenceWeightId = tmodel.FirstWeight,
                            CommenceWeight = cwstring,
                            PurchaseWeight = pwstring,
                            PublishStateId = tmodel.PublishStateId,
                            PublishState = publishstatestring,
                            CropId = tmodel.CropId ?? 0,
                            Crop = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == tmodel.CropId).FirstOrDefault()),
                            ReplyUserId = rmodel.UserId,
                            ReplyUserName = replynamestring,
                            ReplyPhoneNumber = rmodel.PhoneNumber,
                            ReplyDetailedAddress = rmodel.Address,
                            ReplyTime = Utility.TimeHelper.GetMilliSeconds(rmodel.CreateTime),
                            ReplyRemark = rmodel.Brief != null ? rmodel.Brief : "",
                            Score = rmodel.Score,
                            ReplyWeightId = rmodel.WeightRangeTypeId,
                            ReplyWeight = replyweight,
                            PublisherRoleId = (int)RoleType.Business,
                            PublisherRoleName = RoleType.Business.GetDescription()
                        };
                        return model;

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region "根据编号获取区域名称"
        /// <summary>
        /// 根据编号获取区域名称
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns>System.String.</returns>
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

        #region "根据需求编号获取经纬度"
        /// <summary>
        /// 根据需求编号获取经纬度
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetCoordinateByid(long id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                string coordinate = "0|0";
                var model = dbContext.T_FARMER_PUBLISHED_DEMAND.FirstOrDefault(b => b.Id == id);
                if (model != null)
                {
                    string code = "";
                    string[] area = { model.Province, model.City, model.Region, model.Township, model.Village };
                    for (int i = 0; i < area.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(area[i]))
                        {
                            code = area[i];
                        }
                    }
                    var areamodel = dbContext.T_AREA.FirstOrDefault(a => a.AID == code);
                    if (areamodel != null)
                    {
                        if (!string.IsNullOrEmpty(areamodel.Lat) && !string.IsNullOrEmpty(areamodel.Lng))
                        {
                            coordinate = areamodel.Lat + "|" + areamodel.Lng;
                        }
                        else
                        {
                            coordinate = "0|0";
                        }
                    }
                }
                return coordinate;
            }
        }
        #endregion

        #region "获取需求应答列表（后台用）"
        /// <summary>
        /// 获取需求应答列表（后台用）
        /// </summary>
        /// <param name="demandId">The demand identifier.</param>
        /// <returns>List&lt;FarmerDemandReplyItem&gt;.</returns>
        public List<FarmerDemandReplyItem> GetDemandReplyList(long demandId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var sql = @"SELECT u.PhoneNumber
      ,[Comments]
      ,[ReplyTime]
      ,[UserId]
      ,[Score]
  FROM [T_FARMER_DEMAND_RESPONSE_RELATION]
  inner join T_USER as u on u.Id=UserId
  where DemandId=@DemandId";

                var parameter = new SqlParameter("@DemandId", demandId);
                return dbContext.Database.SqlQuery<FarmerDemandReplyItem>(sql, parameter).ToList();
            }
        }
        #endregion

        #region "分配需要给指定的农机手列表"
        /// <summary>
        /// 分配需要给指定的农机手列表
        /// </summary>
        /// <param name="operatorDemands">The operator demands.</param>
        /// <returns>System.Int32.</returns>
        public int AssignOperators(List<T_USER_FARMERDEMANDS> operatorDemands)
        {
            if (operatorDemands == null || operatorDemands.Count == 0)
                return 0;

            var data = new DataTable("T_USER_FARMERDEMANDS");
            data.Columns.AddRange(new DataColumn[]{
                new DataColumn("FarmerDemandId"),
                new DataColumn("UserId")
            });

            foreach (var demand in operatorDemands)
            {
                var dataRow = data.NewRow();
                dataRow["FarmerDemandId"] = demand.FarmerDemandId;
                dataRow["UserId"] = demand.UserId;
                data.Rows.Add(dataRow);
            }

            using (var dbContext = new DuPont_TestContext())
            {
                using (var sqlBulk = new SqlBulkCopy(dbContext.Database.Connection.ConnectionString))
                {
                    sqlBulk.DestinationTableName = "T_USER_FARMERDEMANDS";
                    sqlBulk.ColumnMappings.Add("FarmerDemandId", "FarmerDemandId");
                    sqlBulk.ColumnMappings.Add("UserId", "UserId");
                    sqlBulk.WriteToServer(data);
                }
            }

            return operatorDemands.Count();
        }
        #endregion

        #region "查询指定的农机手的数量"
        /// <summary>
        /// 查询指定的农机手的数量
        /// </summary>
        /// <param name="FarmerDemandid">需求id</param>
        /// <returns>System.Int32.</returns>
        public int SelectOperators(long FarmerDemandid)
        {
            if (FarmerDemandid != 0 && FarmerDemandid != null)
            {
                var param_farmerdemandid = new SqlParameter("@id", SqlDbType.Int, 32);
                var param_TotalCount = new SqlParameter("@totalcount", SqlDbType.Int);
                param_TotalCount.Direction = ParameterDirection.Output;
                param_farmerdemandid.Value = FarmerDemandid;
                string sql = "set @totalcount=(select count(1) from T_USER_FARMERDEMANDS where FarmerDemandId=@id)";
                using (var dbContext = new DuPont_TestContext())
                {
                    dbContext.Database.SqlQuery<T_USER_FARMERDEMANDS>(sql,
                            param_farmerdemandid,
                            param_TotalCount
                        ).ToList();
                }
                return (int)param_TotalCount.Value;
            }
            else
            {

                return 1;
            }
        }
        #endregion

        #region "获取大农户发给产业商（农机手）的信息"
        /// <summary>
        /// 获取大农户发给产业商（农机手）的信息
        /// </summary>
        /// <param name="PageIndex">每几页</param>
        /// <param name="PageSize">每页显示数据条数</param>
        /// <param name="type">需求类型编号</param>        
        /// <param name="orderfield">排序字段</param>       
        /// <param name="TotalNums">符合条件的总记录数</param>
        /// <author>ww</author>
        /// <returns></returns>
        public List<PublishedModel> GetRequirementListForOperatorAndBusiness(int PageIndex, int PageSize, int type, string orderfield, out long TotalNums)
        {
            using (var ctx = new DuPont_TestContext())
            {
                var param_Demand_TypeId = new SqlParameter("@demand_typeid", SqlDbType.Int);
                var param_PageIndex = new SqlParameter("@pageindex", SqlDbType.Int, 32);
                var param_PageSize = new SqlParameter("@pagesize", SqlDbType.Int, 32);
                var param_Distance_Limit = new SqlParameter("@distance_limit", SqlDbType.Int);
                var param_TotalCount = new SqlParameter("@totalcount", SqlDbType.Int);
                param_TotalCount.Direction = ParameterDirection.Output;

                param_Demand_TypeId.Value = type;
                param_PageIndex.Value = PageIndex;
                param_PageSize.Value = PageSize;
                param_Distance_Limit.Value = 200000;


                var distanceLitmitSettingInfo = ctx.T_SYS_SETTING.Where(s => s.SETTING_ID == "001").FirstOrDefault();
                if (distanceLitmitSettingInfo != null)
                {
                    param_Distance_Limit.Value = Convert.ToInt32(distanceLitmitSettingInfo.SETTING_VALUE);
                }

                string sql = string.Empty;
                //获得产业商或大农户的需求列表
                sql = SqlStrings.GetRequirementListForOperatorAndBusiness;

                sql = sql.Replace("{AreaFilter}", "1=1");

                //设置排序
                switch (orderfield)
                {
                    case "time":
                        sql = sql.Replace("{OrderField}", "CreateTime desc ");

                        break;
                    case "number":
                        sql = sql.Replace("{OrderField}", "NumberSort desc");
                        break;
                    case "distance":
                        sql = sql.Replace("{OrderField}", "Distance asc");
                        break;
                    default:
                        sql = sql.Replace("{OrderField}", "Distance asc");
                        break;
                }

                List<DemandIdWithDistanceItem> demandWithDistanceList = new List<DemandIdWithDistanceItem>();

                demandWithDistanceList = ctx.Database.SqlQuery<DemandIdWithDistanceItem>(sql,
                      param_PageIndex,
                      param_PageSize,
                      param_Distance_Limit,
                      param_TotalCount,
                      param_Demand_TypeId
                  ).ToList();


                //符合条件的总记录数
                TotalNums = Convert.ToInt64(param_TotalCount.Value);
                if (demandWithDistanceList.Count > 0)
                {
                    var demandIdList = demandWithDistanceList.Select(p => p.DemandId).ToList();
                    var areaIdList = new List<string>();
                    demandWithDistanceList.Select(p =>
                    {
                        if (p.Province != null && p.Province.Length > 5 && !areaIdList.Contains(p.Province))
                            areaIdList.Add(p.Province);

                        if (p.City != null && p.City.Length > 5 && !areaIdList.Contains(p.City))
                            areaIdList.Add(p.City);

                        if (p.Region != null && p.Region.Length > 5 && !areaIdList.Contains(p.Region))
                            areaIdList.Add(p.Region);

                        if (p.Township != null && p.Township.Length > 5 && !areaIdList.Contains(p.Township))
                            areaIdList.Add(p.Township);

                        if (p.Village != null && p.Village.Length > 5 && !areaIdList.Contains(p.Village))
                            areaIdList.Add(p.Village);

                        return p;
                    }).Count();

                    var demandInfoList = ctx.T_FARMER_PUBLISHED_DEMAND.Where(p => demandIdList.Contains(p.Id)).ToList();
                    var orderedDemandInfoList = new List<T_FARMER_PUBLISHED_DEMAND>();
                    for (int i = 0; i < demandWithDistanceList.Count; i++)
                    {
                        var demandId = demandWithDistanceList[i].DemandId;
                        var demand = demandInfoList.First(p => p.Id == demandId);
                        orderedDemandInfoList.Add(demand);
                    }



                    var demandCreateUserIdList = demandWithDistanceList.Select(p => p.CreateUserId).Distinct().ToList();
                    var acreageIdList = demandInfoList.Select(p => p.AcresId).Distinct().ToList();
                    var cropIdList = demandInfoList.Select(p => p.CropId).Distinct().ToList();
                    var demandTypeIdList = demandInfoList.Select(p => p.DemandTypeId).Distinct().ToList();
                    var publishStateIdList = demandInfoList.Select(p => p.PublishStateId).Distinct().ToList();

                    demandInfoList.Clear();
                    demandInfoList = null;

                    var sysDictionaryIdList = new List<int>();
                    sysDictionaryIdList.AddRange(acreageIdList);
                    sysDictionaryIdList.AddRange(cropIdList);
                    sysDictionaryIdList.AddRange(demandTypeIdList);
                    sysDictionaryIdList.AddRange(publishStateIdList);

                    //获取用户角色级别信息
                    var getUserRoleLevelSql = SqlStrings.GetUserRoleLevel;
                    getUserRoleLevelSql = getUserRoleLevelSql.Replace("{RoleId}", ((int)RoleType.Farmer).ToString())
                                                             .Replace("{UserIdList}", string.Join(",", demandCreateUserIdList));

                    var dictionaryListSql = ctx.T_SYS_DICTIONARY.Where(p => sysDictionaryIdList.Contains(p.Code)).ToString();
                    var areaListSql = ctx.T_AREA.Where(p => areaIdList.Contains(p.AID)).ToString();

                    var dsResult = new DataSet();
                    var sqlListBuilder = new StringBuilder();
                    sqlListBuilder.AppendLine(getUserRoleLevelSql + Environment.NewLine);
                    sqlListBuilder.AppendLine(dictionaryListSql + Environment.NewLine);
                    sqlListBuilder.AppendLine(areaListSql + Environment.NewLine);


                    SqlDataAdapter dap = new SqlDataAdapter(sqlListBuilder.ToString(), (SqlConnection)ctx.Database.Connection);
                    dap.Fill(dsResult);

                    var userRoleLevelList = dsResult.Tables[0].ToList<UserRoleLevelItem>();
                    var sysDictionaryList = dsResult.Tables[1].ToList<T_SYS_DICTIONARY>();
                    var areaInfoList = dsResult.Tables[2].ToList<T_AREA>();

                    return orderedDemandInfoList.Select(demandItem =>
                    {
                        var demandWithDistanceInfo = demandWithDistanceList.First(p => p.DemandId == demandItem.Id);
                        var demandPublisherUserInfo = userRoleLevelList.FirstOrDefault(p => p.UserId == demandItem.CreateUserId);
                        var model = new PublishedModel
                        {
                            IsOpen = demandItem.IsOpen,
                            Credit = 0,
                            AcreageId = demandItem.AcresId,
                            Acreage = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.AcresId)),
                            CropId = demandItem.CropId,
                            Crop = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.CropId)),
                            Id = demandItem.Id,
                            RequirementTypeId = demandItem.DemandTypeId,
                            RequirementType = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.DemandTypeId)),
                            Remark = demandItem.Brief != null ? demandItem.Brief : "",
                            Dates = demandItem.ExpectedDate != null ? demandItem.ExpectedDate : "",
                            PublishedDate = Utility.TimeHelper.GetMilliSeconds(demandItem.CreateTime),
                            AddressCode = string.Format("{0}|{1}|{2}|{3}|{4}",
                                                demandItem.Province,
                                                demandItem.City,
                                                demandItem.Region,
                                                demandItem.Township,
                                                demandItem.Village
                                          ),
                            Address = MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.Province)) +
                                      MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.City)) +
                                      MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.Region)) +
                                      MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.Township)) +
                                      MyCommons.Get_Area_DisplayName(areaInfoList.FirstOrDefault(p => p.AID == demandItem.Village)),
                            DetailAddress = demandItem.DetailedAddress != null ? demandItem.DetailedAddress : "",
                            PhoneNumber = demandItem.PhoneNumber,
                            PublishStateId = demandItem.PublishStateId,
                            PublishState = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.PublishStateId)),
                            ExpectedStartPrice = Convert.ToDouble(demandItem.ExpectedStartPrice),
                            ExpectedEndPrice = Convert.ToDouble(demandItem.ExpectedEndPrice),
                            Distance = demandWithDistanceInfo.Distance,
                            Lat = demandWithDistanceInfo.Lat,
                            Lng = demandWithDistanceInfo.Lng,
                            PurchaseWeightId = 0,
                            PurchaseWeight = "",
                            CommenceWeightId = 0,
                            CommenceWeight = "",
                            CreateUserId = demandItem.CreateUserId
                        };

                        if (demandPublisherUserInfo != null)
                        {
                            model.Name = demandPublisherUserInfo.UserName;
                            model.Level = demandPublisherUserInfo.Level;
                        }

                        return model;
                    }).ToList();
                }
            }

            return new List<PublishedModel>();
        }
        #endregion

        #region "大农户我的应答增加返回值的"
        /// <summary>
        /// 大农户我的应答
        /// </summary>
        /// <param name="ReceiveRoleId">应答人的角色编号</param>
        /// <param name="pageindex">The pageindex.</param>
        /// <param name="pagesize">The pagesize.</param>
        /// <param name="isclosed">发布状态标识（0进行中，1已关闭）</param>
        /// <param name="userid">大农户id</param>
        /// <author>ww</author>
        /// <param name="TotalNums">The total nums.</param>
        /// <returns>List&lt;ReplyModel&gt;.</returns>
        public List<CommonReplyModel> GetFarmerReplyList(int ReceiveRoleId, int pageindex, int pagesize, int isclosed, long userid, out long TotalNums)
        {
            //大农户我的应答列表中的地址是产业商发布需求时的地址
            using (var dbContext = new DuPont_TestContext())
            {
                List<CommonReplyModel> list = new List<CommonReplyModel>();
                //进行中
                if (isclosed == 0)
                {
                    TotalNums = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                                 join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                                 where (bpd.PublishStateId == 100502 || bpd.PublishStateId == 100503) && drr.UserId == userid
                                 && drr.Comments == null && drr.Score == 0
                                 select drr.Id).Count();
                    if (TotalNums == 0)
                    {
                        return null;
                    }


                    list = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                            join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                            join user in dbContext.T_USER on bpd.CreateUserId equals user.Id
                            where (bpd.PublishStateId == 100502 || bpd.PublishStateId == 100503) && drr.UserId == userid
                            && drr.Comments == null && drr.Score == 0
                            orderby drr.CreateTime descending
                            select new
                            {
                                drr.Id,
                                bpd.DemandTypeId,
                                bpd.CreateTime,
                                bpd.Province,
                                bpd.City,
                                bpd.Region,
                                bpd.Township,
                                bpd.Village,
                                bpd.DetailedAddress,
                                bpd.PhoneNumber,
                                bpd.Brief,
                                bpd.PublishStateId,
                                bpd.FirstWeight,
                                user.UserName,
                                bpd.CreateUserId,
                                drr.Comments,
                                drr.Score
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                               .Select(model =>
                               {
                                   var item = new CommonReplyModel
                                   {
                                       Id = model.Id,
                                       PublisherUserName = model.UserName,
                                       RequirementType = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                       PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                       Address = GetAreaName(model.Province)
                                                    + GetAreaName(model.City)
                                                    + GetAreaName(model.Region)
                                                    + GetAreaName(model.Township)
                                                    + GetAreaName(model.Village),
                                       DetailAddress = model.DetailedAddress != null ? model.DetailedAddress : "",
                                       PhoneNumber = model.PhoneNumber,
                                       Remark = model.Brief != null ? model.Brief : "",
                                       //Status = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                       Acreage = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.FirstWeight).Take(1).FirstOrDefault()),
                                       RoleId = (int)RoleType.Business,
                                       RoleName = RoleType.Business.GetDescription(),
                                       PublisherUserId = model.CreateUserId
                                   };
                                   var publishStateId = 0;
                                   if (string.IsNullOrEmpty(model.Comments) || model.Score == 0)
                                   {
                                       publishStateId = 100502;
                                   }
                                   else
                                   {
                                       publishStateId = 100503;
                                   }
                                   item.PublishState = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == publishStateId).FirstOrDefault());
                                   item.RequirementTypeId = model.DemandTypeId;
                                   item.PublishStateId = publishStateId;
                                   item.ReceiveRoleId = ReceiveRoleId;
                                   return item;
                               }).ToList();
                }
                else if (isclosed != 0)//已关闭
                {
                    TotalNums = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                                 join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                                 where drr.UserId == userid && (drr.Score > 0
                                 || drr.Comments != null
                                 || bpd.PublishStateId == 100504
                                 || bpd.PublishStateId == 100505
                                 || bpd.PublishStateId == 100506)
                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }
                    list = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                            join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                            join user in dbContext.T_USER on bpd.CreateUserId equals user.Id
                            where drr.UserId == userid && (drr.Score > 0
                                 || drr.Comments != null
                                 || bpd.PublishStateId == 100504
                                 || bpd.PublishStateId == 100505
                                 || bpd.PublishStateId == 100506)
                            orderby drr.CreateTime descending
                            select new
                            {
                                Id = drr.Id,
                                bpd.DemandTypeId,
                                bpd.CreateTime,
                                bpd.Province,
                                bpd.City,
                                bpd.Region,
                                bpd.Township,
                                bpd.Village,
                                bpd.DetailedAddress,
                                bpd.PhoneNumber,
                                bpd.Brief,
                                bpd.PublishStateId,
                                bpd.FirstWeight,
                                user.UserName,
                                bpd.CreateUserId,
                                drr.Score,
                                drr.Comments
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                               .Select(model =>
                               {

                                   var item = new CommonReplyModel
                                   {
                                       Id = model.Id,
                                       PublisherUserName = model.UserName,
                                       RequirementType = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                       PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                       Address = GetAreaName(model.Province)
                                                    + GetAreaName(model.City)
                                                    + GetAreaName(model.Region)
                                                    + GetAreaName(model.Township)
                                                    + GetAreaName(model.Village),
                                       DetailAddress = model.DetailedAddress ?? "",
                                       PhoneNumber = model.PhoneNumber,
                                       Remark = model.Brief ?? "",
                                       Acreage = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.FirstWeight).Take(1).FirstOrDefault()),
                                       RoleId = (int)RoleType.Business,
                                       RoleName = RoleType.Business.GetDescription(),
                                       PublisherUserId = model.CreateUserId
                                   };
                                   if (!IsClosedState(model.PublishStateId) && (model.Score > 0 || model.Comments != null))
                                   {
                                       item.PublishState = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == 100503).FirstOrDefault());
                                       item.PublishStateId = 100503;
                                   }
                                   else
                                   {
                                       item.PublishState = MyCommons.Get_SysDictionary_DisplayName(dbContext.T_SYS_DICTIONARY.Where(s => s.Code == model.PublishStateId).FirstOrDefault());
                                       item.PublishStateId = model.PublishStateId;
                                   }
                                   item.RequirementTypeId = model.DemandTypeId;
                                   item.ReceiveRoleId = ReceiveRoleId;
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
    }
}
