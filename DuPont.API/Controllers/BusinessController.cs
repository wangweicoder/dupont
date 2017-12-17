using DuPont.API.Filters;
using DuPont.Entity.Enum;
using DuPont.Interface;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DuPont.API.Controllers
{
#if(!DEBUG)
    [AccessAuthorize]
#endif
    public class BusinessController : Controller
    {
        //产业商进行中
        public readonly int[] businessing = { 100501, 100502, 100503 };
        //产业商已关闭
        public readonly int[] businessed = { 100504, 100505 };
        private readonly IBusiness repository;
        private readonly ICommon _commonRepository;
        private readonly ISysSetting _sysSettingRepository;
        private readonly IUser _userRepository;
        private readonly INotification _notificationService;
        private readonly IUser_Role _userRoleService;
        public BusinessController(IBusiness _repository,
            ICommon commonRepository,
            ISysSetting settingRepository,
            IUser userRepository,
            INotification notificationService,
            IUser_Role userRoleService
            )
        {
            this.repository = _repository;
            _commonRepository = commonRepository;
            _sysSettingRepository = settingRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _userRoleService = userRoleService;
        }

        /// <summary>
        /// 产业商发布需求
        /// </summary>
        /// <param name="id">0表示执行添加操作</param>
        /// <param name="userid">产业商id</param>
        /// <param name="Type">需求类型编号</param>
        /// <param name="Dates">预期时间</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <param name="PurchaseWeight">收购区间类型编号</param>
        /// <param name="CommenceWeight">起购重量</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="Remark">摘要</param>
        /// <param name="cropId">农作物Id</param>
        /// <param name="PurchaseStartPrice">预期最低价格</param>
        /// <param name="PurchaseEndPrice">预期最高价格</param>
        /// <param name="?">The ?.</param>
        /// <returns>JsonResult.</returns>
        public JsonResult SaveRequirement(long id, long userid, int Type, string Dates, string Address, string DetailAddress,
            int PurchaseWeight, int CommenceWeight, string PhoneNumber, string Remark, int cropId, double PurchaseStartPrice = 0, double PurchaseEndPrice = 0)
        {
            using (ResponseResult<T_BUSINESS_PUBLISHED_DEMAND> result = new ResponseResult<T_BUSINESS_PUBLISHED_DEMAND>())
            {

                int pwnumber = 0;
                int conumber = 0;
                //收粮：100201
                //产业商收购粮食重量区间：100400
                //产业商收购粮食起购重量：100600
                //收青贮：100202
                //产业商收购青贮重量区间：101100
                //产业商收购青贮起购亩数：101200
                if (string.IsNullOrEmpty(Dates))
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.DateNotNull;
                    return Json(result);
                }
                if (!Utility.RegexHelper.IsMatch(Address, @"^\d*\|\d*\|\d*\|\d*\|\d*$"))
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.AddressError;
                    return Json(result);
                }
                if (string.IsNullOrEmpty(PhoneNumber))
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.PhonenumberNotNull;
                    return Json(result);
                }
                if (cropId < 0)
                {
                    cropId = 0;
                }
                switch (Type)
                {
                    case 100201:
                        if (!_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == PurchaseWeight && s.ParentCode == 100400)))
                        {
                            result.IsSuccess = false;
                            result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, PurchaseWeight.ToString());
                            return Json(result);
                        }
                        if (!_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == CommenceWeight && s.ParentCode == 100600)))
                        {
                            result.IsSuccess = false;
                            result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, CommenceWeight.ToString());
                            return Json(result);
                        }
                        var pwmodel = _commonRepository.GetById<T_SYS_DICTIONARY>(s => s.Code == PurchaseWeight);
                        var comodel = _commonRepository.GetById<T_SYS_DICTIONARY>(s => s.Code == CommenceWeight);
                        pwnumber = pwmodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(pwmodel.DisplayName.Replace("吨以上", "")) + 1 : Convert.ToInt32(pwmodel.DisplayName.Replace("吨", "").Split('-')[1]);
                        conumber = comodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(comodel.DisplayName.Replace("吨以上", "")) + 1 : Convert.ToInt32(comodel.DisplayName.Replace("吨", ""));
                        if (conumber > pwnumber)
                        {
                            result.IsSuccess = false;
                            result.Message = ResponeString.WeightIsError;
                            return Json(result);
                        }
                        break;
                    case 100202:
                        if (!_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == PurchaseWeight && s.ParentCode == 101100)))
                        {
                            result.IsSuccess = false;
                            result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, PurchaseWeight.ToString());
                            return Json(result);
                        }
                        if (!_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == CommenceWeight && s.ParentCode == 101200)))
                        {
                            result.IsSuccess = false;
                            result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, CommenceWeight.ToString());
                            return Json(result);
                        }
                        pwmodel = _commonRepository.GetById<T_SYS_DICTIONARY>(s => s.Code == PurchaseWeight);
                        comodel = _commonRepository.GetById<T_SYS_DICTIONARY>(s => s.Code == CommenceWeight);
                        pwnumber = pwmodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(pwmodel.DisplayName.Replace("亩以上", "")) + 1 : Convert.ToInt32(pwmodel.DisplayName.Replace("亩", "").Split('-')[1]);
                        conumber = comodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(comodel.DisplayName.Replace("亩以上", "")) + 1 : Convert.ToInt32(comodel.DisplayName.Replace("亩", ""));
                        if (conumber > pwnumber)
                        {
                            result.IsSuccess = false;
                            result.Message = ResponeString.WeightIsError;
                            return Json(result);
                        }
                        break;
                    default:
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, Type.ToString());
                        return Json(result);
                }
                //验证userid是否有操作权限
                if (_commonRepository.CheckUserId(userid, (int)RoleType.Business))
                {
                    //执行添加操作
                    if (id == 0)
                    {
                        T_BUSINESS_PUBLISHED_DEMAND newRequirement = new T_BUSINESS_PUBLISHED_DEMAND();
                        newRequirement.CreateUserId = userid;
                        newRequirement.DemandTypeId = Type;
                        newRequirement.ExpectedDate = Dates;
                        newRequirement.PublishStateId = 100501;
                        Dictionary<string, string> adc = StringHelper.GetAddress(Address);

                        newRequirement.Province = adc["Province"];
                        newRequirement.City = adc["City"];
                        newRequirement.Region = adc["Region"];
                        newRequirement.Township = adc["Township"];
                        newRequirement.Village = adc["Village"];

                        newRequirement.DetailedAddress = DetailAddress != null ? DetailAddress : "";
                        newRequirement.ExpectedStartPrice = Convert.ToDecimal(PurchaseStartPrice);
                        newRequirement.ExpectedEndPrice = Convert.ToDecimal(PurchaseEndPrice);
                        newRequirement.AcquisitionWeightRangeTypeId = PurchaseWeight;
                        newRequirement.FirstWeight = CommenceWeight;
                        newRequirement.PhoneNumber = PhoneNumber;
                        newRequirement.Brief = Remark != null ? Remark : "";
                        newRequirement.CreateTime = Utility.TimeHelper.GetChinaLocalTime();
                        newRequirement.CropId = cropId;
                        repository.Insert(newRequirement);

                        result.Entity = newRequirement;
                        result.IsSuccess = true;
                        result.Message = ResponeString.SaveSuccessfullyMessage;

                        return Json(result);
                    }
                    //执行修改操作
                    T_BUSINESS_PUBLISHED_DEMAND updateRequirement = repository.GetByKey(id);
                    if (!updateRequirement.Equals(null))
                    {
                        //当前需求已被响应不能在修改
                        if (updateRequirement.PublishStateId == 100501)
                        {
                            updateRequirement.ModifiedUserId = userid;
                            updateRequirement.ModifiedTime = Utility.TimeHelper.GetChinaLocalTime();
                            updateRequirement.DemandTypeId = Type;
                            updateRequirement.ExpectedDate = Dates;
                            updateRequirement.PublishStateId = 100501;
                            Dictionary<string, string> adc = StringHelper.GetAddress(Address);

                            updateRequirement.Province = adc["Province"];
                            updateRequirement.City = adc["City"];
                            updateRequirement.Region = adc["Region"];
                            updateRequirement.Township = adc["Township"];
                            updateRequirement.Village = adc["Village"];

                            updateRequirement.DetailedAddress = DetailAddress;
                            updateRequirement.ExpectedStartPrice = Convert.ToDecimal(PurchaseStartPrice);
                            updateRequirement.ExpectedEndPrice = Convert.ToDecimal(PurchaseEndPrice);
                            updateRequirement.AcquisitionWeightRangeTypeId = PurchaseWeight;
                            updateRequirement.FirstWeight = CommenceWeight;
                            updateRequirement.PhoneNumber = PhoneNumber;
                            updateRequirement.Brief = Remark;
                            updateRequirement.CropId = cropId;
                            _commonRepository.Modify<T_BUSINESS_PUBLISHED_DEMAND>(updateRequirement, b => b.Id == id);
                            result.Entity = updateRequirement;
                            result.IsSuccess = true;
                            result.Message = ResponeString.SaveSuccessfullyMessage;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = ResponeString.NotUpdate;
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, id.ToString());
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = String.Format("{0} -" + ResponeString.NoJurisdiction, userid.ToString());
                }
                return Json(result);
            }
        }

        /// <summary>
        /// 删除指定需求信息
        /// </summary>
        /// <param name="id">需求信息id</param>
        /// <returns>JsonResult.</returns>
        public JsonResult RemoveRequirement(long id)
        {
            using (ResponseResult<T_BUSINESS_PUBLISHED_DEMAND> result = new ResponseResult<T_BUSINESS_PUBLISHED_DEMAND>())
            {
                T_BUSINESS_PUBLISHED_DEMAND req = repository.GetByKey(id);

                if (req == null)
                {
                    result.Message = ResponeString.DeleteWithNonMessage;
                    result.IsSuccess = false;
                }
                else
                {
                    //进行中
                    if (businessing.Contains(req.PublishStateId))
                    {
                        req.IsDeleted = true;
                        req.PublishStateId = 100504;//已取消
                        _commonRepository.Modify<T_BUSINESS_PUBLISHED_DEMAND>(req, b => b.Id == id);
                        result.IsSuccess = true;
                        result.Entity = req;
                        result.Message = ResponeString.DeleteSuccessfullyMessage;

                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = ResponeString.NotDelete;
                    }
                }

                return Json(result);
            }
        }

        /// <summary>
        /// 产业商进行订单评价
        /// </summary>
        /// <param name="isOwn">0表示产业商的需求，1表示大农户的需求</param>
        /// <param name="id">需求编号</param>
        /// <param name="userid">响应者id（接受当前需求）</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CommentRequirement(int isOwn, long id, long userid, string commentString, int score)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            {

                //产业商的需求
                if (isOwn == 0)
                {
                    T_BUSINESS_PUBLISHED_DEMAND model = _commonRepository.GetById<T_BUSINESS_PUBLISHED_DEMAND>(b => b.Id == id);
                    //验证当前需求编号是否存在
                    if (model != null)
                    {
                        T_BUSINESS_DEMAND_RESPONSE_RELATION responseRelation = _commonRepository.GetById<T_BUSINESS_DEMAND_RESPONSE_RELATION>(b => b.DemandId == id && b.UserId == userid);
                        //验证ueserid是否有效
                        if (responseRelation != null)
                        {
                            responseRelation.Score = score;
                            responseRelation.Comments = commentString != null ? commentString : "";
                            responseRelation.ReplyTime = Utility.TimeHelper.GetChinaLocalTime();
                            var bonusDPoint = int.Parse(this._sysSettingRepository.GetSetting(DataKey.BonusDPointByCommentSettingID).SETTING_VALUE);
                            responseRelation.BonusDPoint = bonusDPoint;

                            //执行评价操作
                            _commonRepository.Modify<T_BUSINESS_DEMAND_RESPONSE_RELATION>(responseRelation, b => b.DemandId == id && b.UserId == userid);
                            if (score > 0)
                            {
                                var dpointTotal = score * bonusDPoint;
                                //给评价人自己添加系统默认先锋币
                                _commonRepository.AddDuPontPoint(model.CreateUserId, "评价添加先锋币", (int)SysCfg.SysUserId, bonusDPoint);
                                //给被评价人添加先锋币
                                _commonRepository.AddDuPontPoint(responseRelation.UserId, "被评价添加先锋币", (int)SysCfg.SysUserId, dpointTotal);

                                //计算被评价者的角色等级
                                var farmerRoleId = (int)RoleType.Farmer;
                                var userRoleInfo = _userRoleService.GetAll(p => p.MemberType && p.UserID == userid && p.RoleID == farmerRoleId).FirstOrDefault();
                                if (userRoleInfo != null)
                                {
                                    if (userRoleInfo.TotalReplyCount == null)
                                        userRoleInfo.TotalReplyCount = 0;

                                    if (userRoleInfo.TotalStarCount == null)
                                        userRoleInfo.TotalStarCount = 0;

                                    userRoleInfo.TotalReplyCount++;
                                    userRoleInfo.TotalStarCount += score;
                                    var averageStar = userRoleInfo.TotalStarCount / userRoleInfo.TotalReplyCount;
                                    _userRoleService.Update(p => p.UserID == userid && p.RoleID == farmerRoleId, t => new T_USER_ROLE_RELATION
                                    {
                                        Star = averageStar,
                                        TotalStarCount = userRoleInfo.TotalStarCount,
                                        TotalReplyCount = userRoleInfo.TotalReplyCount
                                    });
                                }
                            }

                            //评价成功后更改需求状态为已评价（100503）
                            T_BUSINESS_PUBLISHED_DEMAND fmodel = _commonRepository.GetById<T_BUSINESS_PUBLISHED_DEMAND>(f => f.Id == id);
                            fmodel.PublishStateId = 100503;
                            _commonRepository.Modify<T_BUSINESS_PUBLISHED_DEMAND>(fmodel, f => f.Id == id);
                            result.IsSuccess = true;
                            result.Entity = responseRelation;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = string.Format("{0} - " + ResponeString.NoJurisdiction, userid.ToString());
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = string.Format("{0} - " + ResponeString.ParmetersInvalidMessage, id.ToString());
                    }
                }
                else
                {
                    T_FARMER_PUBLISHED_DEMAND model = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(b => b.Id == id);
                    //验证当前需求编号是否存在于产业商需求表中且当前需求没有被删除且是excuteuserId创建的
                    if (model != null)
                    {
                        T_FARMER_DEMAND_RESPONSE_RELATION responseRelation = _commonRepository.GetById<T_FARMER_DEMAND_RESPONSE_RELATION>(b => b.DemandId == id && b.UserId == userid);
                        //验证ueserid是否有效
                        if (responseRelation != null)
                        {
                            responseRelation.Score = score;
                            responseRelation.Comments = commentString != null ? commentString : "";
                            responseRelation.ReplyTime = Utility.TimeHelper.GetChinaLocalTime();
                            var bonusDPoint = int.Parse(this._sysSettingRepository.GetSetting(DataKey.BonusDPointByCommentSettingID).SETTING_VALUE);
                            responseRelation.BonusDPoint = bonusDPoint;
                            //执行评价操作
                            _commonRepository.Modify<T_FARMER_DEMAND_RESPONSE_RELATION>(responseRelation, b => b.DemandId == id && b.UserId == userid);
                            //评价成功后更改需求状态为已评价（100503）
                            T_FARMER_PUBLISHED_DEMAND fmodel = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id);
                            fmodel.PublishStateId = 100503;
                            _commonRepository.Modify<T_FARMER_PUBLISHED_DEMAND>(fmodel, f => f.Id == id);
                            if (score > 0)
                            {
                                var dpointTotal = score * bonusDPoint;
                                //给评价人自己添加系统默认先锋币
                                _commonRepository.AddDuPontPoint(responseRelation.UserId, "评价添加先锋币", (int)SysCfg.SysUserId, bonusDPoint);
                                //给被评价人添加先锋币
                                _commonRepository.AddDuPontPoint(model.CreateUserId, "被评价添加先锋币", (int)SysCfg.SysUserId, dpointTotal);

                                //计算被评价者的角色等级
                                var farmerRoleId = (int)RoleType.Farmer;
                                var userRoleInfo = _userRoleService.GetAll(p => p.MemberType && p.UserID == userid && p.RoleID == farmerRoleId).FirstOrDefault();
                                if (userRoleInfo != null)
                                {
                                    if (userRoleInfo.TotalReplyCount == null)
                                        userRoleInfo.TotalReplyCount = 0;

                                    if (userRoleInfo.TotalStarCount == null)
                                        userRoleInfo.TotalStarCount = 0;

                                    userRoleInfo.TotalReplyCount++;
                                    userRoleInfo.TotalStarCount += score;
                                    var averageStar = userRoleInfo.TotalStarCount / userRoleInfo.TotalReplyCount;
                                    _userRoleService.Update(p => p.UserID == userid && p.RoleID == farmerRoleId, t => new T_USER_ROLE_RELATION
                                    {
                                        Star = averageStar,
                                        TotalStarCount = userRoleInfo.TotalStarCount,
                                        TotalReplyCount = userRoleInfo.TotalReplyCount
                                    });
                                }
                            }
                            result.IsSuccess = true;
                            result.Entity = responseRelation;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = string.Format("{0} - " + ResponeString.NoJurisdiction, userid.ToString());
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = string.Format("{0} - " + ResponeString.ParmetersInvalidMessage, id.ToString());
                    }
                }
                return Json(result);
            }

        }

        /// <summary>
        /// 产业商响应大农户的需求（向大农户需求响应表添加记录）
        /// </summary>
        /// <param name="id">需求id</param>
        /// <param name="userId">产业商id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult ReplyRequirement(long id, long userId)
        {
            using (ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> result = new ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>())
            {

                T_FARMER_PUBLISHED_DEMAND firstmodel = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id);
                //验证需求是否存在于大农户需求表中 
                if (firstmodel != null)
                {
                    //判断用户是否在骗积分
                    if (userId == firstmodel.CreateUserId)
                    {
                        result.IsSuccess = false;
                        result.Message = ResponeString.YourSelfRequirement;
                        return Json(result);
                    }
                    //验证是否是发布给产业商的需求
                    if (_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => s.Code == firstmodel.DemandTypeId && s.ParentCode == 100800))
                    {
                        //验证userid是否是产业商id
                        if (_commonRepository.CheckUserId(userId, (int)RoleType.Business))
                        {
                            //验证需求为待响应
                            if (firstmodel.PublishStateId == 100501)
                            {
                                //验证当前用户是否接受过此订单
                                if (!_commonRepository.CheckTypeid<T_FARMER_DEMAND_RESPONSE_RELATION>(f => f.DemandId == id && f.UserId == userId))
                                {
                                    T_FARMER_DEMAND_RESPONSE_RELATION model = new T_FARMER_DEMAND_RESPONSE_RELATION()
                                    {  DemandId = id,
                                        UserId = userId,
                                        CreateTime = Utility.TimeHelper.GetChinaLocalTime(), 
                                        ReplyTime = Utility.TimeHelper.GetChinaLocalTime() ,
                                        //增加新增字段ww
                                       ReplyTimeFarmer = Utility.TimeHelper.GetChinaLocalTime()
                                    };
                                    //添加响应记录
                                    _commonRepository.Add<T_FARMER_DEMAND_RESPONSE_RELATION>(model);
                                    //响应成功后更改需求状态为待评价（100502）
                                    firstmodel.PublishStateId = 100502;
                                    _commonRepository.Modify<T_FARMER_PUBLISHED_DEMAND>(firstmodel, f => f.Id == id);
                                    result.IsSuccess = true;
                                    result.Entity = model;

                                    //给产业商发送一个通知
                                    _notificationService.Insert(new T_NOTIFICATION
                                    {
                                        MsgContent = "您的需求有人响应啦,快去看看吧!",
                                        IsPublic = false,
                                        TargetUserId = firstmodel.CreateUserId,
                                        NotificationType = 3,
                                        NotificationSource = "",
                                        NotificationSourceId = firstmodel.Id
                                    });
                                }
                                else
                                {
                                    result.IsSuccess = false;
                                    result.Message = ResponeString.NotRepeatApplication;
                                }
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Message = ResponeString.RequirementOver;
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = string.Format("{0} - " + ResponeString.NoJurisdiction, userId.ToString());
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = ResponeString.NotBAccept;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.NoRequirement;
                }
                return Json(result);
            }
        }

        /// <summary>
        /// 产业商获取我的应答列表
        /// </summary>
        /// <param name="pageindex">页码数</param>
        /// <param name="pagesize">第页要显示的数据条数</param>
        /// <param name="isclosed">需求发布状态（0进行中，1已关闭）</param>
        /// <param name="userid">产业商id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult MyReply(int pageindex, int pagesize, int isclosed, long userid)
        {
            long TotalNums = 0;
            pageindex = pageindex == 0 ? 1 : pageindex;
            using (ResponseResult<List<ReplyModel>> result = new ResponseResult<List<ReplyModel>>())
            {

                //验证是否是产业商id
                if (_commonRepository.CheckUserId(userid, (int)RoleType.Business))
                {
                    List<ReplyModel> list = repository.GetReplyList(pageindex, pagesize, isclosed, userid, out TotalNums);
                    if (list != null)
                    {
                        result.Entity = list;
                    }
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = string.Format("{0} - " + ResponeString.NoJurisdiction, userid.ToString());
                }

                result.TotalNums = TotalNums;
                result.PageIndex = pageindex;
                result.PageSize = pagesize;
                return Json(result);
            }
        }

        /// <summary>
        /// 应答详情
        /// </summary>
        /// <param name="requirementid">需求id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult AcceptRequirement(int requirementid)
        {
            using (ResponseResult<BusinessReplyDetailModel> result = new ResponseResult<BusinessReplyDetailModel>())
            {

                BusinessReplyDetailModel model = repository.GetReplyDetail(requirementid);
                if (model != null)
                {
                    result.Entity = model;
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = string.Format("{0} - " + ResponeString.ParmetersInvalidMessage, requirementid.ToString());
                }

                return Json(result);
            }
        }

        /// <summary>
        /// 产业商发布给大农户的需求
        /// </summary>
        /// <param name="userId">大农户id(当前登陆者id)</param>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的数据条数</param>
        /// <param name="type">需求类型编号</param>
        /// <param name="region">区县编号</param>
        /// <param name="orderfield">排序标识</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult PublishedForFarmer(long userId, int pageIndex, int pageSize, int type, string region, string orderfield)
        {
            using (ResponseResult<List<PublishedModel>> result = new ResponseResult<List<PublishedModel>>())
            {
                pageIndex = pageIndex == 0 ? 1 : pageIndex;
                long TotalNums = 0;

                if (Utility.RegexHelper.IsMatch(region, @"^\d*\|\d*\|\d*\|\d*\|\d*$"))
                {
                    Dictionary<string, string> adc = StringHelper.GetAddress(region);

                    string coordinate = _commonRepository.GetCoordinate(userId);
                    string[] coordinatearry = coordinate.Split('|');
                    double farmerLat = Convert.ToDouble(coordinatearry[0]);
                    double farmerLng = Convert.ToDouble(coordinatearry[1]);
                    if (_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => s.Code == type && s.ParentCode == 100200))
                    {
                        //获取发布列表
                        List<PublishedModel> list = repository.GetRequirementList(userId, farmerLat, farmerLng, type, adc["Province"].ToString(), adc["City"].ToString(), adc["Region"].ToString(), orderfield, pageIndex, pageSize, out  TotalNums);
                        if (list != null)
                        {
                            result.Entity = list;
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} - " + ResponeString.ParmetersInvalidMessage, type.ToString());
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.AddressError;
                }

                result.TotalNums = TotalNums;
                result.PageIndex = pageIndex;
                result.PageSize = pageSize;
                return Json(result);
            }
        }

        /// <summary>
        /// 关闭需求
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <returns>System.Web.Mvc.JsonResult.</returns>
        public JsonResult CloseRequirement(long id)
        {
            using (ResponseResult<T_BUSINESS_PUBLISHED_DEMAND> result = new ResponseResult<T_BUSINESS_PUBLISHED_DEMAND>())
            {

                var model = _commonRepository.GetById<T_BUSINESS_PUBLISHED_DEMAND>(b => b.Id == id);
                if (model != null)
                {

                    if (model.PublishStateId == 100501 || repository.GetScore(id))
                    {
                        model.PublishStateId = 100506;//已关闭
                    }
                    else if (model.PublishStateId != 100501 && !repository.GetScore(id))
                    {
                        model.PublishStateId = 100504;//已取消
                    }
                    _commonRepository.Modify<T_BUSINESS_PUBLISHED_DEMAND>(model, b => b.Id == id);
                    result.IsSuccess = true;
                    result.Entity = model;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = String.Format("{0} - " + ResponeString.ParmetersInvalidMessage, id.ToString());
                }

                return Json(result);
            }

        }

        #region 产业商发布给大农户的需求列表(未登录)
        /// <summary>
        /// 产业商发布给大农户的需求
        /// </summary>       
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的数据条数</param>
        /// <param name="type">需求类型编号</param>
        /// <param >收粮100201，收青贮100202</param>
        /// <param name="orderfield">排序标识</param>
        /// <returns>JsonResult.</returns>
        /// <author>ww</author>
        [HttpPost]
        public JsonResult PublishedForFarmerbyTime(int pageIndex, int pageSize, int type, string orderfield)
        {
            using (ResponseResult<List<PublishedModel>> result = new ResponseResult<List<PublishedModel>>())
            {
                pageIndex = pageIndex == 0 ? 1 : pageIndex;
                long TotalNums = 0;

                if (type!=0)
                {
                    if (_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => s.Code == type && s.ParentCode == 100200))
                    {
                        //获取发布列表
                        List<PublishedModel> list = repository.GetRequirementListByTime( type,orderfield, pageIndex, pageSize, out  TotalNums);
                        if (list != null)
                        {
                            result.Entity = list;
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} - " + ResponeString.ParmetersInvalidMessage, type.ToString());
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.AddressError;
                }

                result.TotalNums = TotalNums;
                result.PageIndex = pageIndex;
                result.PageSize = pageSize;
                return Json(result);
            }
        }
        #endregion
    }
}