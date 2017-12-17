


using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuPont.Repository
{
    public class OperatorRepository : IOperator
    {
        private Func<T_SYS_DICTIONARY, string> getDisplayName = (dic) =>
        {
            if (dic != null)
            {
                return dic.DisplayName;
            }
            return "";
        };
        /// <summary>
        /// 农机手我的应答列表
        /// </summary>
        /// <add></add>
        /// <param PublishStateId>100507 待农机手评价</param>
        /// <param PublishStateId>100508 待大农户评价</param>
        /// <returns></returns>
        public List<ReplyModel> GetReplyList(int pageindex, int pagesize, int isclosed, long userid, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //应答列表中的地址是大农户发布需求时的地址
                List<ReplyModel> list = new List<ReplyModel>();
                //进行中
                if (isclosed == 0)
                {
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 where drr.UserId == userid && fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
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
                            join dic in dbContext.T_SYS_DICTIONARY on fpd.DemandTypeId equals dic.Code
                            join user in dbContext.T_USER on fpd.CreateUserId equals user.Id
                            where drr.UserId == userid && fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
                                       && (fpd.PublishStateId == 100502
                                        || fpd.PublishStateId == 100507
                                        || fpd.PublishStateId == 100508
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
                                fpd.CreateUserId,
                                DemandTypeName = dic.DisplayName
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                model =>
                                {
                                    var item = new ReplyModel
                                    {
                                        Id = model.Id,
                                        Requirement = model.DemandTypeName,
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
                else if (isclosed != 0)//已关闭
                {
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 join dic in dbContext.T_SYS_DICTIONARY on fpd.DemandTypeId equals dic.Code
                                 where drr.UserId == userid
                                        && fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
                                        && (
                                            fpd.PublishStateId == 100503
                                            || fpd.PublishStateId == 100504
                                            || fpd.PublishStateId == 100505
                                            || fpd.PublishStateId == 100506
                                        )
                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }

                    list = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                            join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                            join dic in dbContext.T_SYS_DICTIONARY on fpd.DemandTypeId equals dic.Code
                            join user in dbContext.T_USER on fpd.CreateUserId equals user.Id
                            where drr.UserId == userid
                                        && fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
                                        && (
                                            fpd.PublishStateId == 100503
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
                                fpd.CreateUserId,
                                DemandTypeName = dic.DisplayName
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                model =>
                                {
                                    var item = new ReplyModel
                                    {
                                        Id = model.Id,
                                        Requirement = model.DemandTypeName,
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
                    list = null;
                }
                return list;
            }

        }
        /// <summary>
        /// 应答详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BusinessReplyDetailModel GetReplyDetail(long id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //农机手应答详情包括订单详情和应答详情，订单详情中的地址是大农户发布需求的地址，应答详情中的地址是农机手注册时的地址
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
                        string namestring = name != null ? name.UserName != null ? name.UserName : "" : "";

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
                            UserId = tmodel.CreateUserId,
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

        #region 农机手我的应答
        /// <summary>
        /// 农机手我的应答列表
        /// </summary>
        /// <param name="ReceiveRoleId">应答人的角色编号</param>
        /// <author>ww</author>
        /// <returns></returns>
        public List<CommonReplyModel> GetOperatorReplayList(int ReceiveRoleId,int pageindex, int pagesize, int isclosed, long userid, out long TotalNums)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //应答列表中的地址是大农户发布需求时的地址
                List<CommonReplyModel> list = new List<CommonReplyModel>();
                //进行中
                if (isclosed == 0)
                {
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 where drr.UserId == userid && fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
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
                            join dic in dbContext.T_SYS_DICTIONARY on fpd.DemandTypeId equals dic.Code
                            join user in dbContext.T_USER on fpd.CreateUserId equals user.Id
                            where drr.UserId == userid && fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
                                       && (fpd.PublishStateId == 100502
                                        || fpd.PublishStateId == 100507
                                        || fpd.PublishStateId == 100508
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
                                fpd.CreateUserId,
                                DemandTypeName = dic.DisplayName
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                model =>
                                {
                                    var item = new CommonReplyModel
                                    {
                                        Id = model.Id,
                                        RequirementType = model.DemandTypeName,
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
                else if (isclosed != 0)//已关闭
                {
                    TotalNums = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                                 join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                                 join dic in dbContext.T_SYS_DICTIONARY on fpd.DemandTypeId equals dic.Code
                                 where drr.UserId == userid
                                        && fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
                                        && (
                                            fpd.PublishStateId == 100503
                                            || fpd.PublishStateId == 100504
                                            || fpd.PublishStateId == 100505
                                            || fpd.PublishStateId == 100506
                                        )
                                 select drr.Id).Count();

                    if (TotalNums == 0)
                    {
                        return null;
                    }

                    list = (from drr in dbContext.T_FARMER_DEMAND_RESPONSE_RELATION
                            join fpd in dbContext.T_FARMER_PUBLISHED_DEMAND on drr.DemandId equals fpd.Id
                            join dic in dbContext.T_SYS_DICTIONARY on fpd.DemandTypeId equals dic.Code
                            join user in dbContext.T_USER on fpd.CreateUserId equals user.Id
                            where drr.UserId == userid
                                        && fpd.DemandTypeId > 100100 && fpd.DemandTypeId < 100200
                                        && (
                                            fpd.PublishStateId == 100503
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
                                fpd.CreateUserId,
                                DemandTypeName = dic.DisplayName
                            }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList()
                            .Select(
                                model =>
                                {
                                    var item = new CommonReplyModel
                                    {
                                        Id = model.Id,
                                        RequirementType = model.DemandTypeName,
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
                                        ReceiveRoleId = ReceiveRoleId
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
    }
}
