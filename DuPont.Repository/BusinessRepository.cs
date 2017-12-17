



using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Global.Common;
using DuPont.Interface;
using DuPont.Models.Dtos.Background.Demand;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DuPont.Repository
{
    public class BusinessRepository : IBusiness
    {
        //产业商进行中
        public readonly int[] businessing = { 100501, 100502, 100503 };
        private Func<T_SYS_DICTIONARY, string> getDisplayName = (dic) =>
        {
            if (dic != null)
            {
                return dic.DisplayName;
            }
            return "";
        };

        private Func<T_USER_ROLE_RELATION, long?> getUserRoleStar = (data) =>
        {
            if (data == null || data.Star == null)
            {
                return (byte)0;
            }
            return data.Star;
        };


        public int Insert(T_BUSINESS_PUBLISHED_DEMAND entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_BUSINESS_PUBLISHED_DEMAND.Add(entity);
                return dbContext.SaveChanges();
            }
        }

        public int Insert(IEnumerable<T_BUSINESS_PUBLISHED_DEMAND> entities)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_BUSINESS_PUBLISHED_DEMAND.AddRange(entities);
                return dbContext.SaveChanges();
            }
        }

        public int Delete(object id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_BUSINESS_PUBLISHED_DEMAND.Find(id);
                if (dbEntry != null)
                {
                    dbContext.T_BUSINESS_PUBLISHED_DEMAND.Remove(dbEntry);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Delete(T_BUSINESS_PUBLISHED_DEMAND entity)
        {
            if (entity != null)
            {
                using (var dbContext = new DuPont_TestContext())
                {
                    dbContext.T_BUSINESS_PUBLISHED_DEMAND.Remove(entity);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Delete(IEnumerable<T_BUSINESS_PUBLISHED_DEMAND> entities)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_BUSINESS_PUBLISHED_DEMAND.RemoveRange(entities);
                return dbContext.SaveChanges();
            }
        }

        public int Delete(Expression<Func<T_BUSINESS_PUBLISHED_DEMAND, bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_BUSINESS_PUBLISHED_DEMAND.SingleOrDefault(predicate);
                if (dbEntry != null)
                {
                    dbContext.T_BUSINESS_PUBLISHED_DEMAND.Remove(dbEntry);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }
        public int Update(T_BUSINESS_PUBLISHED_DEMAND entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_BUSINESS_PUBLISHED_DEMAND.Find(entity.Id);
                if (dbEntry != null)
                {
                    ClassValueCopyHelper.Copy(dbEntry, entity);
                    return dbContext.SaveChanges();
                }

            }
            return 0;

        }

        public T_BUSINESS_PUBLISHED_DEMAND GetByKey(object key)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_BUSINESS_PUBLISHED_DEMAND.Find(key);
            }
        }

        public IList<T_BUSINESS_PUBLISHED_DEMAND> GetAll()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_BUSINESS_PUBLISHED_DEMAND.ToList();
            }
        }
        /// <summary>
        /// 分页获取全部数据
        /// </summary>
        /// <param name="SeachModel"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<T_BUSINESS_PUBLISHED_DEMAND> GetAll(BusinessSeachModel model, out int totalCount)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var predicate = PredicateBuilder.True<T_BUSINESS_PUBLISHED_DEMAND>();
                
                //需求类型过滤
                if (model.DemandTypeId.IsNullOrEmpty() == false && PageValidate.IsNumber(model.DemandTypeId) && model.DemandTypeId != "0")
                {
                    var demandTypeId = int.Parse(model.DemandTypeId);
                    predicate = predicate.And(p => p.DemandTypeId == demandTypeId);
                }

                //需求状态过滤
                if (model.PublishStateId.IsNullOrEmpty() == false && PageValidate.IsNumber(model.PublishStateId) && model.PublishStateId != "0")
                {
                    var publishStateId = int.Parse(model.PublishStateId);
                    predicate = predicate.And(p => p.PublishStateId == publishStateId);
                }

                //删除状态过滤
                if (model.IsDeleted.HasValue)
                {
                    predicate = predicate.And(p => p.IsDeleted == model.IsDeleted.Value);
                }

                #region "地区过滤"
                if (!model.ProvinceAid.IsNullOrEmpty() && model.ProvinceAid.Length > 1)
                    predicate = predicate.And(p => p.Province == model.ProvinceAid);

                if (!model.CityAid.IsNullOrEmpty() && model.CityAid.Length > 1)
                    predicate = predicate.And(p => p.City == model.CityAid);

                if (!model.RegionAid.IsNullOrEmpty() && model.RegionAid.Length > 1)
                    predicate = predicate.And(p => p.Region == model.RegionAid);
                #endregion

                totalCount = dbContext.T_BUSINESS_PUBLISHED_DEMAND.Count(predicate);

                return dbContext.T_BUSINESS_PUBLISHED_DEMAND.Where(predicate)
                        .OrderByDescending(a => a.CreateTime)
                        .Skip((model.pageIndex - 1) * model.pageSize)
                        .Take(model.pageSize)
                        .ToList();
            }
        }

        public IList<T_BUSINESS_PUBLISHED_DEMAND> GetAll(System.Linq.Expressions.Expression<Func<T_BUSINESS_PUBLISHED_DEMAND, bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntryList = dbContext.T_BUSINESS_PUBLISHED_DEMAND.Where(predicate);
                if (dbEntryList != null)
                {
                    return dbEntryList.ToList();
                }
            }
            return null;
        }


        public List<PublishedModel> GetRequirementList(long userid, double farmerLat, double farmerLng, int type, string pronvices, string city, string region, string orderfield, int PageIndex, int PageSize, out long TotalNums)
        {
            using (var ctx = new DuPont_TestContext())
            {
                var param_Source_Lat = new SqlParameter("@source_lat", SqlDbType.VarChar, 200);
                var param_Source_Lng = new SqlParameter("@source_lng", SqlDbType.VarChar, 200);
                var param_Demand_TypeId = new SqlParameter("@demand_typeid", SqlDbType.Int);
                var param_PageIndex = new SqlParameter("@pageindex", SqlDbType.Int, 32);
                var param_PageSize = new SqlParameter("@pagesize", SqlDbType.Int, 32);
                var param_Distance_Limit = new SqlParameter("@distance_limit", SqlDbType.Int);
                var param_TotalCount = new SqlParameter("@totalcount", SqlDbType.Int);
                param_TotalCount.Direction = ParameterDirection.Output;

                param_Source_Lat.Value = farmerLat;
                param_Source_Lng.Value = farmerLng;
                param_Demand_TypeId.Value = type;
                param_PageIndex.Value = PageIndex;
                param_PageSize.Value = PageSize;
                param_Distance_Limit.Value = 200000;


                var distanceLitmitSettingInfo = ctx.T_SYS_SETTING.Where(s => s.SETTING_ID == "001").FirstOrDefault();
                if (distanceLitmitSettingInfo != null)
                {
                    param_Distance_Limit.Value = Convert.ToInt32(distanceLitmitSettingInfo.SETTING_VALUE);
                }

                var sql = SqlStrings.Get_Farmer_RequirementList;
                sql = sql.Replace("{ValidPublishStateIds}", string.Join(",", businessing));

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
                        var orderCondition="CreateTime desc ";
                        sql = sql.Replace("{OrderField}", orderCondition);
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
                    param_Source_Lat,
                    param_Source_Lng,
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

                    var demandInfoList = ctx.T_BUSINESS_PUBLISHED_DEMAND.Where(p => demandIdList.Contains(p.Id)).ToList();
                    var orderedDemandInfoList = new List<T_BUSINESS_PUBLISHED_DEMAND>();
                    for (int i = 0; i < demandWithDistanceList.Count; i++)
                    {
                        var demandId = demandWithDistanceList[i].DemandId;
                        var demand = demandInfoList.First(p => p.Id == demandId);
                        orderedDemandInfoList.Add(demand);
                    }



                    var demandCreateUserIdList = demandWithDistanceList.Select(p => p.CreateUserId).Distinct().ToList();
                    var firstWeightIdList = demandInfoList.Select(p => p.FirstWeight).Distinct().ToList();
                    var demandTypeIdList = demandInfoList.Select(p => p.DemandTypeId).Distinct().ToList();
                    var publishStateIdList = demandInfoList.Select(p => p.PublishStateId).Distinct().ToList();
                    var purchaseWeightIdList = demandInfoList.Select(p => p.AcquisitionWeightRangeTypeId).Distinct().ToList();

                    demandInfoList.Clear();
                    demandInfoList = null;

                    var sysDictionaryIdList = new List<int>();
                    sysDictionaryIdList.AddRange(firstWeightIdList);
                    sysDictionaryIdList.AddRange(demandTypeIdList);
                    sysDictionaryIdList.AddRange(publishStateIdList);
                    sysDictionaryIdList.AddRange(purchaseWeightIdList);

                    //获取用户角色级别信息
                    var getUserRoleLevelSql = SqlStrings.GetUserRoleLevel;
                    getUserRoleLevelSql = getUserRoleLevelSql.Replace("{RoleId}", ((int)RoleType.Business).ToString())
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
                            IsOpen = true,
                            Credit = 0,
                            AcreageId = 0,
                            Acreage = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.FirstWeight)),
                            CropId = 0,
                            Crop = string.Empty,
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
                            PurchaseWeightId = demandItem.AcquisitionWeightRangeTypeId,
                            PurchaseWeight = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.AcquisitionWeightRangeTypeId)),
                            CommenceWeightId = demandItem.FirstWeight,
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


            //using (var dbContext = new DuPont_TestContext())
            //{
            //    int bigdistance = 200000;
            //    var tancemodel = dbContext.T_SYS_SETTING.Where(s => s.SETTING_ID == "001").FirstOrDefault();
            //    if (tancemodel != null)
            //    {
            //        bigdistance = Convert.ToInt32(tancemodel.SETTING_VALUE);
            //    }
            //    System.Linq.Expressions.Expression<Func<T_BUSINESS_PUBLISHED_DEMAND, bool>> wherelambda = null;
            //    var overrequirementlist = (from b in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION where b.UserId == userid select b.DemandId).ToList();
            //    if (overrequirementlist.Count > 0)
            //    {
            //        //确认是否添加区县检索条件
            //        if (!string.IsNullOrEmpty(region))
            //        {
            //            wherelambda = d => !overrequirementlist.Contains(d.Id) && d.Region == region && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(city))
            //            {
            //                wherelambda = d => !overrequirementlist.Contains(d.Id) && d.City == city && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //            }
            //            else
            //            {
            //                if (!string.IsNullOrEmpty(pronvices))
            //                {
            //                    wherelambda = d => !overrequirementlist.Contains(d.Id) && d.Province == pronvices && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //                }
            //                else
            //                {
            //                    wherelambda = d => !overrequirementlist.Contains(d.Id) && d.IsDeleted == false && d.DemandTypeId == type && businessing.Contains(d.PublishStateId);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //确认是否添加区县检索条件
            //        if (!string.IsNullOrEmpty(region))
            //        {
            //            wherelambda = d => d.Region == region && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(city))
            //            {
            //                wherelambda = d => d.City == city && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //            }
            //            else
            //            {
            //                if (!string.IsNullOrEmpty(pronvices))
            //                {
            //                    wherelambda = d => d.Province == pronvices && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //                }
            //                else
            //                {
            //                    wherelambda = d => d.IsDeleted == false && d.DemandTypeId == type && businessing.Contains(d.PublishStateId);
            //                }
            //            }
            //        }
            //    }
            //    //TotalNums = dbContext.Set<T_BUSINESS_PUBLISHED_DEMAND>().Where(wherelambda).Count();
            //    //if (TotalNums==0)
            //    //{
            //    //    return null;
            //    //}.Skip((PageIndex - 1) * PageSize).Take(PageSize)
            //    var query = dbContext.Set<T_BUSINESS_PUBLISHED_DEMAND>().Where(wherelambda).OrderByDescending(b => b.CreateTime);
            //    List<PublishedModel> demandList = query.ToList().Select(d =>
            //    {
            //        var user = dbContext.Set<T_USER>().Where(u => u.Id == d.CreateUserId).FirstOrDefault();
            //        var userRoleInfo = dbContext.T_USER_ROLE_RELATION.Where(u => u.MemberType && u.UserID == d.CreateUserId).FirstOrDefault();
            //        var model = new PublishedModel
            //        {
            //            CreateUserId = d.CreateUserId,
            //            Credit = getUserRoleStar(userRoleInfo),
            //            Id = d.Id,
            //            RequirementTypeId = d.DemandTypeId,
            //            RequirementType = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.DemandTypeId).FirstOrDefault() != null ? dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.DemandTypeId).FirstOrDefault().DisplayName : "",
            //            Remark = d.Brief != null ? d.Brief : "",
            //            Dates = d.ExpectedDate != null ? d.ExpectedDate : "",
            //            PublishedDate = Utility.TimeHelper.GetMilliSeconds(d.CreateTime),
            //            AddressCode = d.Province + "|" + d.City + "|" + d.Region + "|" + d.Township + "|" + d.Village,
            //            Address = GetAreaName(d.Province) + "" + GetAreaName(d.City) + "" + GetAreaName(d.Region) + "" + GetAreaName(d.Township) + "" + GetAreaName(d.Village),
            //            DetailAddress = d.DetailedAddress != null ? d.DetailedAddress : "",
            //            PhoneNumber = d.PhoneNumber,
            //            PublishStateId = d.PublishStateId,
            //            PublishState = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.PublishStateId).FirstOrDefault() != null ? dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.PublishStateId).FirstOrDefault().DisplayName : "",
            //            ExpectedStartPrice = Convert.ToDouble(d.ExpectedStartPrice),
            //            ExpectedEndPrice = Convert.ToDouble(d.ExpectedEndPrice),
            //            PurchaseWeightId = d.AcquisitionWeightRangeTypeId,
            //            PurchaseWeight = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.AcquisitionWeightRangeTypeId).FirstOrDefault() != null ? dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.AcquisitionWeightRangeTypeId).FirstOrDefault().DisplayName : "",
            //            Distance = GetCoordinateByid(d.Id) != "" ? GISHelper.GetDistance(farmerLat, farmerLng, Convert.ToDouble(GetCoordinateByid(d.Id).Split('|')[0]), Convert.ToDouble(GetCoordinateByid(d.Id).Split('|')[1])) : 0,
            //            CommenceWeightId = d.FirstWeight,
            //            CommenceWeight = "",
            //            Lat = GetCoordinateByid(d.Id).Split('|')[0],
            //            Lng = GetCoordinateByid(d.Id).Split('|')[1],
            //            AcreageId = 0,
            //            Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == d.FirstWeight).FirstOrDefault()),
            //            CropId = 0,
            //            Crop = "",

            //        };
            //        if (user != null)
            //        {
            //            model.Name = user.UserName;
            //        }
            //        if (userRoleInfo != null)
            //        {
            //            model.Level = userRoleInfo.Star ?? 1;
            //        }
            //        return model;
            //    }).ToList();

            //    //获取200公里以内的数据
            //    demandList = demandList.Where(d => d.Distance <= bigdistance).ToList();
            //    //计算总条数
            //    TotalNums = demandList.Count();
            //    //分页
            //    demandList = demandList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            //    //根据排序字段对集合排序
            //    switch (orderfield)
            //    {
            //        case "time":
            //            demandList = demandList.OrderByDescending(d => d.PublishedDate).ToList();
            //            break;
            //        case "number":
            //            demandList = demandList.OrderByDescending(d => d.CommenceWeightId).ToList();
            //            break;
            //        case "distance":
            //            demandList = demandList.OrderBy(d => d.Distance).ToList();
            //            break;
            //        default:
            //            demandList = demandList.OrderBy(d => d.Distance).ToList();
            //            break;
            //    }
            //    return demandList;
            //}
        }
        public List<ReplyModel> GetReplyList(int pageindex, int pagesize, int isclosed, long userid, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //应答列表中的地址是大农户发布需求时的地址
                List<ReplyModel> myReplyList = new List<ReplyModel>();
                //我的应答--进行中
                if (isclosed == 0)
                {
                    //由于产业商角色只能单独存在，所以只需判断用户Id即可
                    //发起的需求为待评价的都视为进行中
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 where drr.UserId == userid && fpd.PublishStateId == 100502
                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }

                    myReplyList = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                   join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                   join user in dbContext.T_USER on fpd.CreateUserId equals user.Id
                                   where drr.UserId == userid && fpd.PublishStateId == 100502
                                   orderby drr.CreateTime descending
                                   select new
                                   {
                                       drr.Id,
                                       fpd.DemandTypeId,
                                       fpd.CreateTime,
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
                                       user.UserName,
                                       fpd.CreateUserId
                                   }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                model =>
                                {
                                    var item = new ReplyModel
                                    {
                                        Id = model.Id,
                                        Requirement = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                        PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                        Address = GetAreaName(model.Province)
                                                    + GetAreaName(model.City)
                                                    + GetAreaName(model.Region)
                                                    + GetAreaName(model.Township)
                                                    + GetAreaName(model.Village),
                                        DetailAddress = model.DetailedAddress ?? "",
                                        PhoneNumber = model.PhoneNumber,
                                        Remark = model.Brief ?? "",
                                        Status = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                        Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.AcresId).FirstOrDefault()),
                                        PublisherUserName = model.UserName,
                                        RoleId = (int)RoleType.Farmer,
                                        RoleName = RoleType.Farmer.GetDescription(),
                                        PublisherUserId = model.CreateUserId
                                    };
                                    item.RequireTypeName = item.Requirement;
                                    return item;
                                }).ToList();
                }
                //我的应答--已关闭
                else if (isclosed != 0)
                {
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 where drr.UserId == userid
                                       && (fpd.PublishStateId == 100503
                                        || fpd.PublishStateId == 100504
                                        || fpd.PublishStateId == 100505
                                        || fpd.PublishStateId == 100506
                                 )

                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }

                    myReplyList = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                   join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                   join user in dbContext.T_USER on fpd.CreateUserId equals user.Id
                                   where drr.UserId == userid
                                         && (fpd.PublishStateId == 100503
                                         || fpd.PublishStateId == 100504
                                         || fpd.PublishStateId == 100505
                                         || fpd.PublishStateId == 100506
                                        )
                                   orderby drr.CreateTime descending
                                   select new
                                   {
                                       drr.Id,
                                       fpd.DemandTypeId,
                                       fpd.CreateTime,
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
                                       user.UserName,
                                       fpd.CreateUserId
                                   }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                 model =>
                                 {
                                     var item = new ReplyModel
                                     {
                                         Id = model.Id,
                                         Requirement = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                         PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                         Address = GetAreaName(model.Province)
                                                     + GetAreaName(model.City)
                                                     + GetAreaName(model.Region)
                                                     + GetAreaName(model.Township)
                                                     + GetAreaName(model.Village),
                                         DetailAddress = model.DetailedAddress ?? "",
                                         PhoneNumber = model.PhoneNumber,
                                         Remark = model.Brief ?? "",
                                         Status = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                         Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.AcresId).FirstOrDefault()),
                                         PublisherUserName = model.UserName,
                                         RoleId = (int)RoleType.Farmer,
                                         RoleName = RoleType.Farmer.GetDescription(),
                                         PublisherUserId = model.CreateUserId
                                     };
                                     item.RequireTypeName = item.Requirement;
                                     return item;
                                 }).ToList();
                }
                else
                {
                    TotalNums = 0;
                    myReplyList = null;
                }

                return myReplyList;
            }
        }
        public BusinessReplyDetailModel GetReplyDetail(long id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //产业商应答详情包括订单详情和应答详情，订单详情中的地址是大农户发布需求的地址，应答详情中的地址是产业商注册时的地址
                var rmodel = dbContext.Set<T_FARMER_DEMAND_RESPONSE_RELATION>().Where(b => b.Id == id).FirstOrDefault();
                if (rmodel != null)
                {
                    T_FARMER_PUBLISHED_DEMAND tmodel = dbContext.Set<T_FARMER_PUBLISHED_DEMAND>().Where(f => f.Id == rmodel.DemandId).FirstOrDefault();
                    if (tmodel != null)
                    {

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

                        string replynamestring = "";
                        string addressstring = "";
                        string replyphonenumber = "";
                        var replymodel = dbContext.Set<T_USER>().Where(u => u.Id == rmodel.UserId).FirstOrDefault();
                        if (replymodel != null)
                        {
                            replynamestring = replymodel.UserName != null ? replymodel.UserName : "";
                            addressstring = GetAreaName(replymodel.Province) + GetAreaName(replymodel.City) + GetAreaName(replymodel.Region) + GetAreaName(replymodel.Township) + GetAreaName(replymodel.Village);
                            replyphonenumber = replymodel.PhoneNumber;
                        }

                        BusinessReplyDetailModel model = new BusinessReplyDetailModel()
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
                            Acreage = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == tmodel.AcresId).FirstOrDefault()),
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

                            ReplyUserId = rmodel.UserId,
                            ReplyUserName = replynamestring,
                            ReplyPhoneNumber = replyphonenumber,
                            ReplyDetailedAddress = addressstring,
                            ReplyTime = Utility.TimeHelper.GetMilliSeconds(rmodel.CreateTime),
                            ReplyRemark = "",
                            Score = rmodel.Score,
                            PublisherRoleId = (int)RoleType.Farmer,
                            PublisherRoleName = RoleType.Farmer.GetDescription()


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

        public bool GetScore(long id)
        {
            bool valid = false;
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntryList = dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION.Where(b => b.DemandId == id).ToList();
                if (dbEntryList != null)
                {
                    for (int i = 0; i < dbEntryList.Count; i++)
                    {
                        if (dbEntryList[i].Score > 0)
                        {
                            valid = true;
                            break;
                        }
                    }
                }
            }
            return valid;
        }
        /// <summary>
        /// 获取地区编号对应的名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
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
                var model = dbContext.T_BUSINESS_PUBLISHED_DEMAND.FirstOrDefault(b => b.Id == id);
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


        public List<DemandReplyItem> GetDemandReplyList(long demandId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var sql = @"SELECT [PhoneNumber]
      ,[Comments]
      ,d.DisplayName as 'WeightRange'
	  ,[ReplyTime]
      ,[UserId]
      ,[PhoneNumber]
      ,[Score]
  FROM [T_BUSINESS_DEMAND_RESPONSE_RELATION]
  inner join T_SYS_DICTIONARY as d on d.Code=WeightRangeTypeId
  where DemandId=@DemandId";
                var parameter = new SqlParameter("@DemandId", demandId);
                return dbContext.Database.SqlQuery<DemandReplyItem>(sql, parameter).ToList();
            }
        }
        /// <summary>
        /// 产业商发布给大农户的需求
        /// </summary>
        /// <author>ww</author>
        public List<PublishedModel> GetRequirementListByTime(int type, string orderfield, int PageIndex, int PageSize, out long TotalNums)
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

                var sql = SqlStrings.Get_Farmer_RequirementListByTime;
                sql = sql.Replace("{ValidPublishStateIds}", string.Join(",", businessing));

                sql = sql.Replace("{AreaFilter}", "1=1");

                //设置排序
                switch (orderfield)
                {
                    case "time":
                        var orderCondition = "CreateTime desc ";
                        sql = sql.Replace("{OrderField}", orderCondition);
                        break;
                    case "number":
                        sql = sql.Replace("{OrderField}", "NumberSort desc");
                        break;
                    case "distance":
                        sql = sql.Replace("{OrderField}", "Distance asc");
                        break;
                    default:
                        sql = sql.Replace("{OrderField}", "CreateTime asc");
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

                    var demandInfoList = ctx.T_BUSINESS_PUBLISHED_DEMAND.Where(p => demandIdList.Contains(p.Id)).ToList();
                    var orderedDemandInfoList = new List<T_BUSINESS_PUBLISHED_DEMAND>();
                    for (int i = 0; i < demandWithDistanceList.Count; i++)
                    {
                        var demandId = demandWithDistanceList[i].DemandId;
                        var demand = demandInfoList.First(p => p.Id == demandId);
                        orderedDemandInfoList.Add(demand);
                    }



                    var demandCreateUserIdList = demandWithDistanceList.Select(p => p.CreateUserId).Distinct().ToList();
                    var firstWeightIdList = demandInfoList.Select(p => p.FirstWeight).Distinct().ToList();
                    var demandTypeIdList = demandInfoList.Select(p => p.DemandTypeId).Distinct().ToList();
                    var publishStateIdList = demandInfoList.Select(p => p.PublishStateId).Distinct().ToList();
                    var purchaseWeightIdList = demandInfoList.Select(p => p.AcquisitionWeightRangeTypeId).Distinct().ToList();

                    demandInfoList.Clear();
                    demandInfoList = null;

                    var sysDictionaryIdList = new List<int>();
                    sysDictionaryIdList.AddRange(firstWeightIdList);
                    sysDictionaryIdList.AddRange(demandTypeIdList);
                    sysDictionaryIdList.AddRange(publishStateIdList);
                    sysDictionaryIdList.AddRange(purchaseWeightIdList);

                    //获取用户角色级别信息
                    var getUserRoleLevelSql = SqlStrings.GetUserRoleLevel;
                    getUserRoleLevelSql = getUserRoleLevelSql.Replace("{RoleId}", ((int)RoleType.Business).ToString())
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
                            IsOpen = true,
                            Credit = 0,
                            AcreageId = 0,
                            Acreage = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.FirstWeight)),
                            CropId = 0,
                            Crop = string.Empty,
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
                            PurchaseWeightId = demandItem.AcquisitionWeightRangeTypeId,
                            PurchaseWeight = MyCommons.Get_SysDictionary_DisplayName(sysDictionaryList.FirstOrDefault(s => s.Code == demandItem.AcquisitionWeightRangeTypeId)),
                            CommenceWeightId = demandItem.FirstWeight,
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


            //using (var dbContext = new DuPont_TestContext())
            //{
            //    int bigdistance = 200000;
            //    var tancemodel = dbContext.T_SYS_SETTING.Where(s => s.SETTING_ID == "001").FirstOrDefault();
            //    if (tancemodel != null)
            //    {
            //        bigdistance = Convert.ToInt32(tancemodel.SETTING_VALUE);
            //    }
            //    System.Linq.Expressions.Expression<Func<T_BUSINESS_PUBLISHED_DEMAND, bool>> wherelambda = null;
            //    var overrequirementlist = (from b in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION where b.UserId == userid select b.DemandId).ToList();
            //    if (overrequirementlist.Count > 0)
            //    {
            //        //确认是否添加区县检索条件
            //        if (!string.IsNullOrEmpty(region))
            //        {
            //            wherelambda = d => !overrequirementlist.Contains(d.Id) && d.Region == region && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(city))
            //            {
            //                wherelambda = d => !overrequirementlist.Contains(d.Id) && d.City == city && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //            }
            //            else
            //            {
            //                if (!string.IsNullOrEmpty(pronvices))
            //                {
            //                    wherelambda = d => !overrequirementlist.Contains(d.Id) && d.Province == pronvices && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //                }
            //                else
            //                {
            //                    wherelambda = d => !overrequirementlist.Contains(d.Id) && d.IsDeleted == false && d.DemandTypeId == type && businessing.Contains(d.PublishStateId);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        //确认是否添加区县检索条件
            //        if (!string.IsNullOrEmpty(region))
            //        {
            //            wherelambda = d => d.Region == region && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(city))
            //            {
            //                wherelambda = d => d.City == city && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //            }
            //            else
            //            {
            //                if (!string.IsNullOrEmpty(pronvices))
            //                {
            //                    wherelambda = d => d.Province == pronvices && d.DemandTypeId == type && d.IsDeleted == false && businessing.Contains(d.PublishStateId);
            //                }
            //                else
            //                {
            //                    wherelambda = d => d.IsDeleted == false && d.DemandTypeId == type && businessing.Contains(d.PublishStateId);
            //                }
            //            }
            //        }
            //    }
            //    //TotalNums = dbContext.Set<T_BUSINESS_PUBLISHED_DEMAND>().Where(wherelambda).Count();
            //    //if (TotalNums==0)
            //    //{
            //    //    return null;
            //    //}.Skip((PageIndex - 1) * PageSize).Take(PageSize)
            //    var query = dbContext.Set<T_BUSINESS_PUBLISHED_DEMAND>().Where(wherelambda).OrderByDescending(b => b.CreateTime);
            //    List<PublishedModel> demandList = query.ToList().Select(d =>
            //    {
            //        var user = dbContext.Set<T_USER>().Where(u => u.Id == d.CreateUserId).FirstOrDefault();
            //        var userRoleInfo = dbContext.T_USER_ROLE_RELATION.Where(u => u.MemberType && u.UserID == d.CreateUserId).FirstOrDefault();
            //        var model = new PublishedModel
            //        {
            //            CreateUserId = d.CreateUserId,
            //            Credit = getUserRoleStar(userRoleInfo),
            //            Id = d.Id,
            //            RequirementTypeId = d.DemandTypeId,
            //            RequirementType = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.DemandTypeId).FirstOrDefault() != null ? dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.DemandTypeId).FirstOrDefault().DisplayName : "",
            //            Remark = d.Brief != null ? d.Brief : "",
            //            Dates = d.ExpectedDate != null ? d.ExpectedDate : "",
            //            PublishedDate = Utility.TimeHelper.GetMilliSeconds(d.CreateTime),
            //            AddressCode = d.Province + "|" + d.City + "|" + d.Region + "|" + d.Township + "|" + d.Village,
            //            Address = GetAreaName(d.Province) + "" + GetAreaName(d.City) + "" + GetAreaName(d.Region) + "" + GetAreaName(d.Township) + "" + GetAreaName(d.Village),
            //            DetailAddress = d.DetailedAddress != null ? d.DetailedAddress : "",
            //            PhoneNumber = d.PhoneNumber,
            //            PublishStateId = d.PublishStateId,
            //            PublishState = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.PublishStateId).FirstOrDefault() != null ? dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.PublishStateId).FirstOrDefault().DisplayName : "",
            //            ExpectedStartPrice = Convert.ToDouble(d.ExpectedStartPrice),
            //            ExpectedEndPrice = Convert.ToDouble(d.ExpectedEndPrice),
            //            PurchaseWeightId = d.AcquisitionWeightRangeTypeId,
            //            PurchaseWeight = dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.AcquisitionWeightRangeTypeId).FirstOrDefault() != null ? dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == d.AcquisitionWeightRangeTypeId).FirstOrDefault().DisplayName : "",
            //            Distance = GetCoordinateByid(d.Id) != "" ? GISHelper.GetDistance(farmerLat, farmerLng, Convert.ToDouble(GetCoordinateByid(d.Id).Split('|')[0]), Convert.ToDouble(GetCoordinateByid(d.Id).Split('|')[1])) : 0,
            //            CommenceWeightId = d.FirstWeight,
            //            CommenceWeight = "",
            //            Lat = GetCoordinateByid(d.Id).Split('|')[0],
            //            Lng = GetCoordinateByid(d.Id).Split('|')[1],
            //            AcreageId = 0,
            //            Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == d.FirstWeight).FirstOrDefault()),
            //            CropId = 0,
            //            Crop = "",

            //        };
            //        if (user != null)
            //        {
            //            model.Name = user.UserName;
            //        }
            //        if (userRoleInfo != null)
            //        {
            //            model.Level = userRoleInfo.Star ?? 1;
            //        }
            //        return model;
            //    }).ToList();

            //    //获取200公里以内的数据
            //    demandList = demandList.Where(d => d.Distance <= bigdistance).ToList();
            //    //计算总条数
            //    TotalNums = demandList.Count();
            //    //分页
            //    demandList = demandList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            //    //根据排序字段对集合排序
            //    switch (orderfield)
            //    {
            //        case "time":
            //            demandList = demandList.OrderByDescending(d => d.PublishedDate).ToList();
            //            break;
            //        case "number":
            //            demandList = demandList.OrderByDescending(d => d.CommenceWeightId).ToList();
            //            break;
            //        case "distance":
            //            demandList = demandList.OrderBy(d => d.Distance).ToList();
            //            break;
            //        default:
            //            demandList = demandList.OrderBy(d => d.Distance).ToList();
            //            break;
            //    }
            //    return demandList;
            //}
        }
        #region "产业商我的应答"
        /// <summary>
        ///  产业商获取我的应答列表
        /// </summary>
        /// <param name="ReceiveRoleId">应答人的角色编号</param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="isclosed"></param>
        /// <param name="userid"></param>
        /// <param name="TotalNums"></param>
        /// <author>ww</author>
        /// <returns></returns>
        public List<CommonReplyModel> GetBusinessReplyList(int ReceiveRoleId, int pageindex, int pagesize, int isclosed, long userid, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //应答列表中的地址是大农户发布需求时的地址
                List<CommonReplyModel> myReplyList = new List<CommonReplyModel>();
                //我的应答--进行中
                if (isclosed == 0)
                {
                    //由于产业商角色只能单独存在，所以只需判断用户Id即可
                    //发起的需求为待评价的都视为进行中
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 where drr.UserId == userid && fpd.PublishStateId == 100502
                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }

                    myReplyList = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                   join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                   join user in dbContext.T_USER on fpd.CreateUserId equals user.Id
                                   where drr.UserId == userid && fpd.PublishStateId == 100502
                                   orderby drr.CreateTime descending
                                   select new
                                   {
                                       drr.Id,
                                       fpd.DemandTypeId,
                                       fpd.CreateTime,
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
                                       user.UserName,
                                       fpd.CreateUserId
                                   }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                model =>
                                {
                                    var item = new CommonReplyModel
                                    {
                                        Id = model.Id,
                                        RequirementType= getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                        PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                        Address = GetAreaName(model.Province)
                                                    + GetAreaName(model.City)
                                                    + GetAreaName(model.Region)
                                                    + GetAreaName(model.Township)
                                                    + GetAreaName(model.Village),
                                        DetailAddress = model.DetailedAddress ?? "",
                                        PhoneNumber = model.PhoneNumber,
                                        Remark = model.Brief ?? "",
                                        PublishState = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                        Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.AcresId).FirstOrDefault()),
                                        PublisherUserName = model.UserName,
                                        RoleId = (int)RoleType.Farmer,
                                        RoleName = RoleType.Farmer.GetDescription(),
                                        PublisherUserId = model.CreateUserId,
                                        RequirementTypeId=model.DemandTypeId,
                                        PublishStateId=model.PublishStateId,
                                        ReceiveRoleId=ReceiveRoleId
                                    };                                   
                                    return item;
                                }).ToList();
                }
                //我的应答--已关闭
                else if (isclosed != 0)
                {
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 where drr.UserId == userid
                                       && (fpd.PublishStateId == 100503
                                        || fpd.PublishStateId == 100504
                                        || fpd.PublishStateId == 100505
                                        || fpd.PublishStateId == 100506
                                 )

                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }

                    myReplyList = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                   join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                   join user in dbContext.T_USER on fpd.CreateUserId equals user.Id
                                   where drr.UserId == userid
                                         && (fpd.PublishStateId == 100503
                                         || fpd.PublishStateId == 100504
                                         || fpd.PublishStateId == 100505
                                         || fpd.PublishStateId == 100506
                                        )
                                   orderby drr.CreateTime descending
                                   select new
                                   {
                                       drr.Id,
                                       fpd.DemandTypeId,
                                       fpd.CreateTime,
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
                                       user.UserName,
                                       fpd.CreateUserId
                                   }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                 model =>
                                 {
                                     var item = new CommonReplyModel
                                     {
                                         Id = model.Id,
                                         RequirementType = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.DemandTypeId).FirstOrDefault()),
                                         PublishedDate = Utility.TimeHelper.GetMilliSeconds(model.CreateTime),
                                         Address = GetAreaName(model.Province)
                                                     + GetAreaName(model.City)
                                                     + GetAreaName(model.Region)
                                                     + GetAreaName(model.Township)
                                                     + GetAreaName(model.Village),
                                         DetailAddress = model.DetailedAddress ?? "",
                                         PhoneNumber = model.PhoneNumber,
                                         Remark = model.Brief ?? "",
                                         PublishState = getDisplayName(dbContext.Set<T_SYS_DICTIONARY>().Where(s => s.Code == model.PublishStateId).FirstOrDefault()),
                                         Acreage = getDisplayName(dbContext.T_SYS_DICTIONARY.Where(dic => dic.Code == model.AcresId).FirstOrDefault()),
                                         PublisherUserName = model.UserName,
                                         RoleId = (int)RoleType.Farmer,
                                         RoleName = RoleType.Farmer.GetDescription(),
                                         PublisherUserId = model.CreateUserId,
                                         RequirementTypeId = model.DemandTypeId,
                                         PublishStateId = model.PublishStateId,
                                         ReceiveRoleId=ReceiveRoleId
                                     };                                     
                                     return item;
                                 }).ToList();
                }
                else
                {
                    TotalNums = 0;
                    myReplyList = null;
                }

                return myReplyList;
            }
        }
        #endregion
    }
}
