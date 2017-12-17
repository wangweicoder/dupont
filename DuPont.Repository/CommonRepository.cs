using DuPont.Interface;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DuPont.Extensions;
using DuPont.Models.Models;
using DuPont.Entity.Enum;
using DuPont.Models.Dtos.Background.User;

namespace DuPont.Repository
{
    public class CommonRepository : ICommon
    {
        private static readonly Func<T_USER, string> getUserName = (user) =>
          {
              if (user != null)
              {
                  return user.UserName ?? "";
              }
              return "";
          };
        public bool CheckUserId(long userid, int key)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var num = dbContext.T_USER_ROLE_RELATION.Where(u => u.MemberType && u.UserID == userid).Select(r => r.RoleID).ToArray();
                if (num == null)
                {
                    return false;
                }
                else
                {
                    return Array.IndexOf(num, key) > -1;
                }
            }
        }
        public bool CheckTypeid<T>(Expression<Func<T, bool>> wherelambda) where T : class
        {
            using (var dbContext = new DuPont_TestContext())
            {

                return dbContext.Set<T>().Where(wherelambda).Count() > 0;
            }
        }

        public T GetById<T>(Expression<Func<T, bool>> wherelambda) where T : class
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.Set<T>().Where(wherelambda).FirstOrDefault();
            }
        }
        public int Add<T>(T entity) where T : class
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.Set<T>().Add(entity);
                return dbContext.SaveChanges();
            }
        }
        public int Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.Set<T>().SingleOrDefault(predicate);
                if (dbEntry != null)
                {
                    dbContext.Set<T>().Remove(dbEntry);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }
        public int Modify<T>(T entity, Expression<Func<T, bool>> wherelambda) where T : class
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.Set<T>().Where(wherelambda).FirstOrDefault();
                if (dbEntry != null)
                {
                    ClassValueCopyHelper.Copy(dbEntry, entity);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public List<CommentDetailModel> GetBusinessCommentDetail(long userid, int pageindex, int pagesize, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                List<CommentDetailModel> list = new List<CommentDetailModel>();


                TotalNums = dbContext.T_FARMER_PUBLISHED_DEMAND
                        .Join(dbContext.T_FARMER_DEMAND_RESPONSE_RELATION, f => f.Id, a => a.DemandId, (f, a) => new { f = f, a = a })
                                .Join(dbContext.T_SYS_DICTIONARY, temp0 => temp0.f.DemandTypeId, b => b.Code, (temp0, b) => new { temp0 = temp0, b = b })
                                .Where(temp1 => ((temp1.temp0.a.UserId == userid) && (temp1.b.ParentCode == 100800) && (temp1.temp0.a.Score > 0 || (temp1.temp0.a.Comments != null && temp1.temp0.a.Comments.Length > 0)))).Count();
                if (TotalNums != 0)
                {
                    //产业商应答大农户的需求
                    list = dbContext.T_FARMER_PUBLISHED_DEMAND
                        .Join(dbContext.T_FARMER_DEMAND_RESPONSE_RELATION, f => f.Id, a => a.DemandId, (f, a) => new { f = f, a = a })
                        .Join(dbContext.T_SYS_DICTIONARY, temp0 => temp0.f.DemandTypeId, b => b.Code, (temp0, b) => new { temp0 = temp0, b = b })
                        .Where(temp1 => ((temp1.temp0.a.UserId == userid) && (temp1.b.ParentCode == 100800) && (temp1.temp0.a.Score > 0 || (temp1.temp0.a.Comments != null && temp1.temp0.a.Comments.Length > 0))))
                        .OrderByDescending(temp1 => temp1.temp0.a.ReplyTime)
                        .Skip((pageindex - 1) * pagesize).Take(pagesize).ToList().Select(
                        temp1 =>
                        {
                            var model = new CommentDetailModel
                                {
                                    ExecuteUserId = temp1.temp0.a.UserId,
                                    ExcuteUserName = getUserName(dbContext.T_USER.Where(u => u.Id == temp1.temp0.a.UserId).FirstOrDefault()),//产业商的用户名
                                    Comments = temp1.temp0.a.Comments ?? "",
                                    Score = temp1.temp0.a.Score,
                                    CommentsTime = Utility.TimeHelper.GetMilliSeconds(temp1.temp0.a.ReplyTime)
                                };
                            var roleType = RoleType.Business;
                            model.ExecuteRoleId = (int)roleType;
                            model.ExecuteRoleName = roleType.GetDescription();
                            return model;
                        }).ToList();
                }


                return list;
            }
        }
        public List<CommentDetailModel> GetOperatorCommentDetail(long userid, int pageindex, int pagesize, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                List<CommentDetailModel> list = new List<CommentDetailModel>();
                TotalNums = dbContext.T_FARMER_PUBLISHED_DEMAND
                        .Join(dbContext.T_FARMER_DEMAND_RESPONSE_RELATION, f => f.Id, a => a.DemandId, (f, a) => new { f = f, a = a })
                                .Join(dbContext.T_SYS_DICTIONARY, temp0 => temp0.f.DemandTypeId, b => b.Code, (temp0, b) => new { temp0 = temp0, b = b })
                                .Where(temp1 => ((temp1.temp0.a.UserId == userid) && (temp1.b.ParentCode == 100100) && (temp1.temp0.a.Score > 0 || (temp1.temp0.a.Comments != null && temp1.temp0.a.Comments.Length > 0)))).Count();
                if (TotalNums != 0)
                {
                    list = dbContext.T_FARMER_PUBLISHED_DEMAND
                        .Join(dbContext.T_FARMER_DEMAND_RESPONSE_RELATION, f => f.Id, a => a.DemandId, (f, a) => new { f = f, a = a })
                        .Join(dbContext.T_SYS_DICTIONARY, temp0 => temp0.f.DemandTypeId, b => b.Code, (temp0, b) => new { temp0 = temp0, b = b })
                        .Where(temp1 => ((temp1.temp0.a.UserId == userid) && (temp1.b.ParentCode == 100100) && (temp1.temp0.a.Score > 0 || (temp1.temp0.a.Comments != null && temp1.temp0.a.Comments.Length > 0))))
                        .OrderByDescending(temp1 => temp1.temp0.a.ReplyTime)
                        .Skip((pageindex - 1) * pagesize).Take(pagesize).ToList().Select(
                        temp1 => new CommentDetailModel
                    {
                        ExcuteUserName = getUserName(dbContext.T_USER.Where(u => u.Id == temp1.temp0.f.CreateUserId).FirstOrDefault()),
                        Comments = temp1.temp0.a.Comments ?? "",
                        Score = temp1.temp0.a.Score,
                        CommentsTime = Utility.TimeHelper.GetMilliSeconds(temp1.temp0.a.ReplyTime),
                        ExecuteUserId = temp1.temp0.f.CreateUserId,
                        ExecuteRoleId = (int)RoleType.Farmer,
                        ExecuteRoleName = RoleType.Farmer.GetDescription()
                    }).ToList();
                }



                return list;
            }
        }
        public List<CommentDetailModel> GetFarmerCommentDetail(long userid, int pageindex, int pagesize, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                List<CommentDetailModel> list = new List<CommentDetailModel>();

                //大农户响应产业商的需求
                TotalNums = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                             join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                             where drr.UserId == userid
                                   && (drr.Score > 0 || (drr.Comments != null && drr.Comments.Length > 0))

                             select drr.Id).Concat(
                             from fpd in dbContext.T_FARMER_PUBLISHED_DEMAND
                             join drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION on fpd.Id equals drr.DemandId
                             where fpd.CreateUserId == userid
                                   && fpd.DemandTypeId > 100800 && fpd.DemandTypeId < 100900
                                   && fpd.PublishStateId == 100503
                                   && (drr.Score > 0 || (drr.Comments != null && drr.Comments.Length > 0))
                             select drr.Id
                          ).Count();

                if (TotalNums != 0)
                {
                    list = (//大农户响应产业商的需求
                        from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                        join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                        where drr.UserId == userid && (drr.Score > 0 || drr.Comments.Length > 0)
                        select new
                        {
                            ExecuteUserId = bpd.CreateUserId,
                            drr.Comments,
                            drr.Score,
                            drr.ReplyTime
                        }).Concat(
                        //产业商响应大农户的需求
                                 from fpd in dbContext.T_FARMER_PUBLISHED_DEMAND
                                 join drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION on fpd.Id equals drr.DemandId
                                 where fpd.CreateUserId == userid && fpd.DemandTypeId > 100800 && fpd.DemandTypeId < 100900
                                   && fpd.PublishStateId == 100503
                                   && (drr.Score > 0 || (drr.Comments != null && drr.Comments.Length > 0))
                                 select new
                                 {
                                     ExecuteUserId = drr.UserId,
                                     drr.Comments,
                                     drr.Score,
                                     drr.ReplyTime
                                 }
                                ).OrderByDescending(m => m.ReplyTime)
                                .Skip((pageindex - 1) * pagesize)
                                .Take(pagesize)
                                .ToList()
                                .Select(model =>
                                {
                                    var item = new CommentDetailModel
                                      {
                                          ExecuteUserId = model.ExecuteUserId,
                                          ExcuteUserName = getUserName(dbContext.T_USER.Where(u => u.Id == model.ExecuteUserId).FirstOrDefault()),
                                          ExecuteRoleId = (int)RoleType.Business,
                                          ExecuteRoleName = RoleType.Business.GetDescription(),
                                          Comments = model.Comments ?? "",
                                          Score = model.Score,
                                          CommentsTime = Utility.TimeHelper.GetMilliSeconds(model.ReplyTime)
                                      };

                                    return item;
                                }
                                ).ToList();
                }


                return list;
            }
        }
       /// <summary>
       /// 农机手评价大农户
       /// </summary>
       /// <param name="userid"></param>
       /// <param name="pageindex"></param>
       /// <param name="pagesize"></param>
       /// <param name="TotalNums"></param>
       /// <returns></returns>
        public List<CommentDetailModel> GetFarmerCommentList(long userid, int pageindex, int pagesize, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                List<CommentDetailModel> list = new List<CommentDetailModel>();
                #region 产业商评价大农户
                List<CommentDetailModel> blist = new List<CommentDetailModel>();

                //大农户响应产业商的需求
                TotalNums = (from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                             join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                             where drr.UserId == userid
                                   && (drr.Score > 0 || (drr.Comments != null && drr.Comments.Length > 0))

                             select drr.Id).Concat(
                             from fpd in dbContext.T_FARMER_PUBLISHED_DEMAND
                             join drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION on fpd.Id equals drr.DemandId
                             where fpd.CreateUserId == userid
                                   && fpd.DemandTypeId > 100800 && fpd.DemandTypeId < 100900
                                   && fpd.PublishStateId == 100503
                                   && (drr.Score > 0 || (drr.Comments != null && drr.Comments.Length > 0))
                             select drr.Id
                          ).Count();

                if (TotalNums != 0)
                {
                    blist = (//大农户响应产业商的需求
                        from drr in dbContext.T_BUSINESS_DEMAND_RESPONSE_RELATION
                        join bpd in dbContext.T_BUSINESS_PUBLISHED_DEMAND on drr.DemandId equals bpd.Id
                        where drr.UserId == userid && (drr.Score > 0 || drr.Comments.Length > 0)
                        select new
                        {
                            ExecuteUserId = bpd.CreateUserId,
                            drr.Comments,
                            drr.Score,
                            drr.ReplyTime
                        }).Concat(
                        //产业商响应大农户的需求
                                 from fpd in dbContext.T_FARMER_PUBLISHED_DEMAND
                                 join drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION on fpd.Id equals drr.DemandId
                                 where fpd.CreateUserId == userid && fpd.DemandTypeId > 100800 && fpd.DemandTypeId < 100900
                                   && fpd.PublishStateId == 100503
                                   && (drr.Score > 0 || (drr.Comments != null && drr.Comments.Length > 0))
                                 select new
                                 {
                                     ExecuteUserId = drr.UserId,
                                     drr.Comments,
                                     drr.Score,
                                     drr.ReplyTime
                                 }
                                ).OrderByDescending(m => m.ReplyTime)
                                .Skip((pageindex - 1) * pagesize)
                                .Take(pagesize)
                                .ToList()
                                .Select(model =>
                                {
                                    var item = new CommentDetailModel
                                    {
                                        ExecuteUserId = model.ExecuteUserId,
                                        ExcuteUserName = getUserName(dbContext.T_USER.Where(u => u.Id == model.ExecuteUserId).FirstOrDefault()),
                                        ExecuteRoleId = (int)RoleType.Business,
                                        ExecuteRoleName = RoleType.Business.GetDescription(),
                                        Comments = model.Comments ?? "",
                                        Score = model.Score,
                                        CommentsTime = Utility.TimeHelper.GetMilliSeconds(model.ReplyTime)
                                    };

                                    return item;
                                }
                                ).ToList();
                }

                #endregion
                #region 农机手评价大农户
                TotalNums += dbContext.T_FARMER_PUBLISHED_DEMAND
                         .Join(dbContext.T_FARMER_DEMAND_RESPONSE_RELATION, f => f.Id, a => a.DemandId, (f, a) => new { f = f, a = a })
                                 .Join(dbContext.T_SYS_DICTIONARY, temp0 => temp0.f.DemandTypeId, b => b.Code, (temp0, b) => new { temp0 = temp0, b = b })
                                 .Where(temp1 => ((temp1.temp0.f.CreateUserId == userid) && (temp1.b.ParentCode == 100100) && (temp1.temp0.a.ScoreFarmer > 0 || (temp1.temp0.a.CommentsFarmer != null && temp1.temp0.a.CommentsFarmer.Length > 0)))).Count();
                if (TotalNums != 0)
                {
                    list = dbContext.T_FARMER_PUBLISHED_DEMAND
                        .Join(dbContext.T_FARMER_DEMAND_RESPONSE_RELATION, f => f.Id, a => a.DemandId, (f, a) => new { f = f, a = a })
                        .Join(dbContext.T_SYS_DICTIONARY, temp0 => temp0.f.DemandTypeId, b => b.Code, (temp0, b) => new { temp0 = temp0, b = b })
                        .Where(temp1 => ((temp1.temp0.f.CreateUserId == userid) && (temp1.b.ParentCode == 100100) && (temp1.temp0.a.ScoreFarmer > 0 || (temp1.temp0.a.CommentsFarmer != null && temp1.temp0.a.CommentsFarmer.Length > 0))))
                        .OrderByDescending(temp1 => temp1.temp0.a.ReplyTimeFarmer)
                        .Skip((pageindex - 1) * pagesize).Take(pagesize).ToList().Select(
                        temp1 => new CommentDetailModel
                        {
                            ExcuteUserName = getUserName(dbContext.T_USER.Where(u => u.Id == temp1.temp0.a.UserId).FirstOrDefault()),
                            Comments = temp1.temp0.a.CommentsFarmer ?? "",
                            Score = temp1.temp0.a.ScoreFarmer,
                            CommentsTime = Utility.TimeHelper.GetMilliSeconds(temp1.temp0.a.ReplyTimeFarmer),
                            ExecuteUserId = temp1.temp0.a.UserId,
                            ExecuteRoleId = (int)RoleType.MachineryOperator,
                            ExecuteRoleName = RoleType.MachineryOperator.GetDescription()
                        }).ToList();
                }
                #endregion
               
                if (list.Count > 0)
                {
                    blist.AddRange(list);
                }
                else if(blist.Count>0){
                   list = blist;
                }
                return list;
            }
        }
        /// <summary>
        /// 获取登陆者的经纬度
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetCoordinate(long userId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                string coordinate = "0|0";
                var usermodel = dbContext.T_USER.FirstOrDefault(u => u.Id == userId);
                if (usermodel != null)
                {
                    string code = "";
                    string[] area = { usermodel.Province, usermodel.City, usermodel.Region, usermodel.Township, usermodel.Village };
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

        public int GetNumber(int roleType, long userid, string Province, string City, string Region)
        {
            int number = 0;
            using (var dbContext = new DuPont_TestContext())
            {
                if (!string.IsNullOrEmpty(Region))
                {
                    number = dbContext.T_USER.Join(dbContext.T_USER_ROLE_RELATION, u => u.Id, a => a.UserID, (u, a) => new { u = u, a = a })
                              .Where(temp0 => (temp0.u.Id != userid
                                  && temp0.u.Region == Region
                                  && temp0.a.RoleID == roleType
                                  && temp0.a.MemberType
                                  )).Count();
                }
                else
                {
                    if (!string.IsNullOrEmpty(City))
                    {
                        number = dbContext.T_USER.Join(dbContext.T_USER_ROLE_RELATION, u => u.Id, a => a.UserID, (u, a) => new { u = u, a = a })
                         .Where(temp0 => (temp0.u.Id != userid
                             && temp0.u.City == City
                             && temp0.a.RoleID == roleType
                             && temp0.a.MemberType
                             )).Count();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Province))
                        {
                            number = dbContext.T_USER.Join(dbContext.T_USER_ROLE_RELATION, u => u.Id, a => a.UserID, (u, a) => new { u = u, a = a })
                           .Where(temp0 => (temp0.u.Id != userid
                               && temp0.u.Province == Province
                               && temp0.a.RoleID == roleType
                               && temp0.a.MemberType
                               )).Count();
                        }
                        else
                        {
                            number = dbContext.T_USER.Join(dbContext.T_USER_ROLE_RELATION, u => u.Id, a => a.UserID, (u, a) => new { u = u, a = a })
                         .Where(temp0 => (temp0.u.Id != userid
                             && temp0.a.RoleID == roleType
                             && temp0.a.MemberType
                             )).Count();
                        }
                    }
                }
            }
            return number;
        }


        /// <summary>
        /// 获取附近农机手列表
        /// </summary>
        /// <param name="Province">The province.</param>
        /// <param name="City">The city.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List&lt;OperatorProfile&gt;.</returns>
        public List<OperatorProfile> GetOperatorsForFarmerRequirement(string Province, string City, int pageIndex, int pageSize, out long totalNums)
        {
            totalNums = 0;
            return new List<OperatorProfile>();
        }

        private static void GetOperatorsAroundFarmer(string Province, string City, int pageIndex, int pageSize, out long totalNums, out List<OperatorProfile> operatorUserList, int demandTypeId = 0)
        {
            operatorUserList = new List<OperatorProfile>();
            using (var dbContext = new DuPont_TestContext())
            {
                var predicate = PredicateBuilder.True<T_USER>();

                //省份过滤
                if (!Province.IsNullOrEmpty())
                    predicate = predicate.And(p => p.Province == Province);

                //城市过滤
                if (!City.IsNullOrEmpty())
                    predicate = predicate.And(p => p.City == City);

                //农机手角色编号
                var operatorRoleId = (int)RoleType.MachineryOperator;
                var query = (from u in dbContext.T_USER.Where(predicate)
                             join user_role_RL in dbContext.T_USER_ROLE_RELATION on u.Id equals user_role_RL.UserID
                             where user_role_RL.MemberType && user_role_RL.RoleID == operatorRoleId
                             select new
                             {
                                 UserId = u.Id,
                                 Name = u.UserName,
                                 Star = user_role_RL.Star
                             });

                if (demandTypeId != 0)
                {
                    query = query.Where(p =>
                                (from user in dbContext.T_USER_ROLE_DEMANDTYPELEVEL_RELATION
                                 where user.UserId == p.UserId && user.RoleId == operatorRoleId && user.DemandId == demandTypeId
                                 select user.UserId).Contains(p.UserId)
                            );
                }

                totalNums = query.Count();
                query = query.OrderByDescending(p => p.UserId).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                var dbEntry = query.ToList();

                if (dbEntry.Count() > 0)
                {
                    foreach (var item in dbEntry)
                    {
                        operatorUserList.Add(new OperatorProfile()
                        {
                            UserId = item.UserId,
                            Name = item.Name,
                            Star = item.Star ?? 0
                        });
                    }
                }

                if (operatorUserList.Count() > 0)
                {
                    //查询用户信息中最后一级地区数据
                    var sql_get_user_area_data = string.Format(@"select Id as UserId,  
case 
when Province IS NULL then null 
when Province ='' then null
when Province='0' then null
else
	case
	   when City is null then Province
	   when City='' then Province
	   when City='0' then Province
	   else
	   case
			when Region is null then City
			when Region='' then City
			when Region='0' then City
			else
			case
				when Township is null then Region
				when Township='' then Region
				when Township='0' then Region
				else
				case
					when Village is null then Township
					when Village ='' then Township
					when Village='0' then Township
					else
					Village
				end
			end
	   end
	end
end as AreaId
from t_user where id in({0})", string.Join(",", operatorUserList.Select(m => m.UserId).ToArray<long>()));

                    var userAreaInfoList = dbContext.Database.SqlQuery<UserAreaInfoRL>(sql_get_user_area_data).ToList();

                    //获取地区对应的经纬度数据
                    var sql_get_area_lnglat_data = string.Format(@"select AID,Lng,lat from [dbo].[T_AREA] where AID in(
  '{0}'
)", string.Join("','", userAreaInfoList.Select(m => m.AreaId).ToArray()));

                    var userAreaLngLatList = dbContext.Database.SqlQuery<AreaLngLat>(sql_get_area_lnglat_data).ToList();

                    foreach (var user in operatorUserList)
                    {
                        var area = userAreaInfoList.Where(m => m.UserId == user.UserId).FirstOrDefault();
                        if (area != null)
                        {
                            var lngLatInfo = userAreaLngLatList.Where(m => m.AID == area.AreaId).FirstOrDefault();
                            if (lngLatInfo != null)
                            {
                                user.Lng = lngLatInfo.Lng;
                                user.Lat = lngLatInfo.Lat;
                            }
                        }

                        var machineInfo = dbContext.T_MACHINERY_OPERATOR_VERIFICATION_INFO.Where(m => m.UserId == user.UserId)
                            .OrderByDescending(p => p.CreateTime).FirstOrDefault();
                        if (machineInfo != null)
                        {
                            if (!(string.IsNullOrWhiteSpace(machineInfo.Machinery) || !machineInfo.Machinery.StartsWith("[")))
                            {
                                user.Machinery = JsonHelper.FromJsonTo<List<ProductInfo>>(machineInfo.Machinery);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取最新版本信息
        /// </summary>
        /// <param name="platform">ios/android</param>
        /// <returns>T_APP_VERSION.</returns>
        public T_APP_VERSION GetLastVersion(string platform)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_APP_VERSION.Where(app => app.Platform == platform).OrderByDescending(p => p.CREATE_DATE).FirstOrDefault();
            }
        }

        /// <summary>
        /// 检测数据库的部署（100表示成功）
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int CheckDatabaseDeployment()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var result = dbContext.Database.SqlQuery<int>("select count(1) from sys.tables where name='T_USER'").Count();
                return result;
            }
        }


        /// <summary>
        /// 增加先锋币
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="description">描述</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="dpoints">先锋币数.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">description</exception>
        /// <exception cref="System.ArgumentException">auditUserId
        /// or
        /// userId</exception>
        /// <exception cref="System.NotImplementedException">description</exception>
        public bool AddDuPontPoint(long userId, string description, long auditUserId, int dpoints)
        {
            if (description == null)
            {
                throw new ArgumentNullException("description");
            }
            if (auditUserId <= 0)
            {
                throw new ArgumentException("auditUserId");
            }
            if (userId <= 0)
            {
                throw new ArgumentException("userId");
            }
            using (var dbContext = new DuPont_TestContext())
            {
                using (var tran = dbContext.Database.BeginTransaction())
                {
                    var user = dbContext.T_USER.Find(userId);
                    if (user == null)
                    {
                        throw new ArgumentException("userId");
                    }

                    dbContext.T_PIONEERCURRENCYHISTORY.Add(new T_PIONEERCURRENCYHISTORY
                    {
                        UserId = userId,
                        AuditUserId = 0,
                        CreateTime = Utility.TimeHelper.GetChinaLocalTime(),
                        Description = description,
                        DPoint = dpoints
                    });
                    if (user.DPoint == null)
                    {
                        user.DPoint = dpoints;
                    }
                    else
                    {
                        user.DPoint += dpoints;
                    }
                    dbContext.SaveChanges();
                    tran.Commit();
                }
            }

            return false;
        }


        public int Add<T>(List<T> entity) where T : class
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.Set<T>().AddRange(entity);
                return dbContext.SaveChanges();
            }
        }

        public List<OperatorProfile> GetOperatorsForFarmerRequirementWithDemandType(string Province, string City, int pageIndex, int pageSize, int demandTypeId, out long totalNums)
        {
            List<OperatorProfile> operatorList = new List<OperatorProfile>();
            GetOperatorsAroundFarmer(Province, City, pageIndex, pageSize, out totalNums, out operatorList, demandTypeId);
            return operatorList;
        }

        private class UserAreaInfoRL
        {
            public long UserId { get; set; }
            public string AreaId { get; set; }
        }

        private class AreaLngLat
        {
            public string AID { get; set; }
            public string Lng { get; set; }
            public string Lat { get; set; }
        }



    }


}
