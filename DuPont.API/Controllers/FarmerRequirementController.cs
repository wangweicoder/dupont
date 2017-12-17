using DuPont.API.Filters;
using DuPont.Entity.Enum;
using DuPont.Interface;
using DuPont.Models;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using DuPont.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DuPont.API.Controllers
{
#if(!DEBUG)
    [AccessAuthorize]
#endif
    public class FarmerRequirementController : BaseController
    {
        #region "Constants"
        //大农户进行中
        public readonly int[] farmering = { 100501, 100502 };
        //大农户已关闭
        public readonly int[] farmered = { 100503, 100504, 100505 };
        //产业商进行中
        public readonly int[] businessing = { 100501, 100502, 100503 };
        #endregion

        private readonly IFarmerRequirement _farmerRequirementService;
        private readonly ISys_Dictionary _sysDictionary;
        private readonly ICommon _commonrepository;
        private readonly IArea _areaRepository;
        private readonly INotification _notificationService;
        private readonly ISysSetting _sysSettingRepository;
        private readonly IUser_Role _userRoleService;
        private readonly IUserRoleDemandTypeLevelRL _userRoleDemandTypeLevelRLService;

        private IUser _userRepository;
        public FarmerRequirementController(IFarmerRequirement repository,
            ISys_Dictionary sysDictionary,
            ICommon commonrepository,
            IArea areaRepository,
            ISysSetting sysSettingRepository,
            IUser userRepository,
            INotification notificationService,
            IUser_Role userRoleService,
            IUserRoleDemandTypeLevelRL userRoleDemandTypeLevelRLService
            )
        {
            _farmerRequirementService = repository;
            _sysDictionary = sysDictionary;
            _commonrepository = commonrepository;
            _areaRepository = areaRepository;
            _sysSettingRepository = sysSettingRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _userRoleService = userRoleService;
            _userRoleDemandTypeLevelRLService = userRoleDemandTypeLevelRLService;
        }

        #region "发布需求（添加/修改）"
        /// <summary>
        /// 发布需求
        /// </summary>
        /// <param name="id">0表示执行添加操作,非0执行修改操作</param>
        /// <param name="userId">发布者id</param>
        /// <param name="type">需求类型</param>
        /// <param name="cropId">农作物类别</param>
        /// <param name="acreage">亩数</param>
        /// <param name="description">摘要</param>
        /// <param name="date">The date.</param>
        /// <param name="address">基本地址</param>
        /// <param name="detailAddress">详细地址</param>
        /// <param name="PhoneNumber">发布需求者的手机号</param>
        /// <param name="ExpectedStartPrice">期望的最低价格</param>
        /// <param name="ExpectedEndPrice">期望的最高价格</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult SaveRequirement(long id, long userId, int type, int cropId, int acreage, string description, string date,
            string address, string detailAddress, string PhoneNumber, double ExpectedStartPrice = 0, double ExpectedEndPrice = 0)
        {
            using (ResponseResult<T_FARMER_PUBLISHED_DEMAND> result = new ResponseResult<T_FARMER_PUBLISHED_DEMAND>())
            {

                if (string.IsNullOrEmpty(date))
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.DateNotNull;
                    return Json(result);
                }
                if (!Utility.RegexHelper.IsMatch(address, @"^\d*\|\d*\|\d*\|\d*\|\d*$"))
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
                //验证参数是否有效
                string[] param = { cropId.ToString(), acreage.ToString(), type.ToString() };
                bool[] valid = { _commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == cropId && s.ParentCode == 100300))
                                       ,_commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == acreage && s.ParentCode == 100900))
                                       , _commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == type && (s.ParentCode == 100100 || s.ParentCode == 100800)))};
                for (int i = 0; i < valid.Length; i++)
                {
                    if (valid[i])
                    {
                        continue;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, param[i]);
                        return Json(result);
                    }
                }
                //验证userid是否有操作权限
                if (_commonrepository.CheckUserId(userId, (int)RoleType.Farmer))
                {
                    //执行添加操作
                    if (id == 0)
                    {
                        T_FARMER_PUBLISHED_DEMAND newRequirement = new T_FARMER_PUBLISHED_DEMAND();

                        newRequirement.DemandTypeId = type;
                        newRequirement.CropId = cropId;
                        newRequirement.VarietyId = 0;
                        newRequirement.AcresId = acreage;
                        newRequirement.Brief = description != null ? description : "";
                        newRequirement.ExpectedDate = date;
                        newRequirement.PublishStateId = 100501;
                        Dictionary<string, string> adc = StringHelper.GetAddress(address);

                        newRequirement.Province = adc["Province"];
                        newRequirement.City = adc["City"];
                        newRequirement.Region = adc["Region"];
                        newRequirement.Township = adc["Township"];
                        newRequirement.Village = adc["Village"];

                        newRequirement.DetailedAddress = detailAddress != null ? detailAddress : "";
                        newRequirement.PhoneNumber = PhoneNumber;
                        newRequirement.ExpectedStartPrice = Convert.ToDecimal(ExpectedStartPrice);
                        newRequirement.ExpectedEndPrice = Convert.ToDecimal(ExpectedEndPrice);

                        newRequirement.CreateUserId = userId;
                        newRequirement.CreateTime = Utility.TimeHelper.GetChinaLocalTime();
                        newRequirement.IsDeleted = false;

                        _farmerRequirementService.Insert(newRequirement);

                        result.Entity = newRequirement;
                        result.IsSuccess = true;
                        result.Message = ResponeString.SaveSuccessfullyMessage;
                        #region 调用E田接口,传订单数据

                        //干活需求单
                        if (newRequirement.DemandTypeId != (int)DuPont.Entity.Enum.FarmerDemandType.SellGrain && newRequirement.DemandTypeId != (int)DuPont.Entity.Enum.FarmerDemandType.SellSilage)
                        {
                            string Address = null;
                            if (!string.IsNullOrWhiteSpace(newRequirement.Township))
                            {
                                Address = newRequirement.Township;
                            }
                            else if (!string.IsNullOrWhiteSpace(newRequirement.Region))
                            {
                                Address = newRequirement.Region;
                            }
                            else if (!string.IsNullOrWhiteSpace(newRequirement.City))
                            {
                                Address = newRequirement.City;
                            }
                            else if (!string.IsNullOrWhiteSpace(newRequirement.Province))
                            {
                                Address = newRequirement.Province;
                            }
                            Task taskasync = new Task(() => ReturnOrderModel(newRequirement.AcresId, Address, newRequirement.CreateUserId, newRequirement));
                            taskasync.Start();
                        }

                        #endregion
                        return Json(result);
                    }

                    T_FARMER_PUBLISHED_DEMAND updateRequirement = _farmerRequirementService.GetByKey(id);
                    if (!updateRequirement.Equals(null))
                    {
                        //当前需求已被响应不能在修改
                        if (updateRequirement.PublishStateId == 100501)
                        {
                            updateRequirement.DemandTypeId = type;
                            updateRequirement.CropId = cropId;
                            updateRequirement.VarietyId = 0;
                            updateRequirement.AcresId = acreage;
                            updateRequirement.Brief = description;
                            updateRequirement.ExpectedDate = date;
                            updateRequirement.PublishStateId = 100501;
                            Dictionary<string, string> adc = StringHelper.GetAddress(address);

                            updateRequirement.Province = adc["Province"];
                            updateRequirement.City = adc["City"];
                            updateRequirement.Region = adc["Region"];
                            updateRequirement.Township = adc["Township"];
                            updateRequirement.Village = adc["Village"];

                            updateRequirement.DetailedAddress = detailAddress;
                            updateRequirement.PhoneNumber = PhoneNumber;
                            updateRequirement.ExpectedStartPrice = Convert.ToDecimal(ExpectedStartPrice);
                            updateRequirement.ExpectedEndPrice = Convert.ToDecimal(ExpectedEndPrice);


                            updateRequirement.ModifiedUserId = userId;
                            updateRequirement.ModifiedTime = Utility.TimeHelper.GetChinaLocalTime();
                            updateRequirement.IsDeleted = false;
                            _commonrepository.Modify<T_FARMER_PUBLISHED_DEMAND>(updateRequirement, f => f.Id == id);
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
                    result.Message = String.Format("{0} -" + ResponeString.NoJurisdiction, userId.ToString());
                }

                return Json(result);
            }
        }
        #endregion

        /// <summary>
        /// 指定农机手的需求
        /// </summary>
        /// <param name="id">0表示执行添加操作,非0执行修改操作</param>
        /// <param name="userId">发布者id</param>
        /// <param name="type">需求类型</param>
        /// <param name="cropId">农作物类别</param>
        /// <param name="acreage">亩数</param>
        /// <param name="description">摘要</param>
        /// <param name="date">The date.</param>
        /// <param name="address">基本地址</param>
        /// <param name="detailAddress">详细地址</param>
        /// <param name="PhoneNumber">发布需求者的手机号</param>
        /// <param name="personIds">多选农机手id</param>
        /// <param name="ExpectedStartPrice">期望的最低价格</param>
        /// <param name="ExpectedEndPrice">期望的最高价格</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult SaveOperatorRequirement(long id, long userId, int type, int cropId, int acreage, string description, string date,
            string address, string detailAddress, string PhoneNumber, string personIds, double ExpectedStartPrice = 0, double ExpectedEndPrice = 0)
        {
            using (ResponseResult<T_FARMER_PUBLISHED_DEMAND> result = new ResponseResult<T_FARMER_PUBLISHED_DEMAND>())
            {

                if (string.IsNullOrEmpty(date))
                    return ResponseErrorWithJson<T_FARMER_PUBLISHED_DEMAND>(result, ResponeString.DateNotNull);

                if (!Utility.RegexHelper.IsMatch(address, @"^\d*\|\d*\|\d*\|\d*\|\d*$"))
                    return ResponseErrorWithJson<T_FARMER_PUBLISHED_DEMAND>(result, ResponeString.AddressError);

                if (string.IsNullOrEmpty(PhoneNumber))
                    return ResponseErrorWithJson<T_FARMER_PUBLISHED_DEMAND>(result, ResponeString.PhonenumberNotNull);

                //获取大农户的需求类型
                var operatorDemandTypeList = _sysDictionary.GetAll(p => p.ParentCode == 100100).ToList();
                var operatorDemandTypeIdList = operatorDemandTypeList.Select(p => p.Code).ToList();

                if (!operatorDemandTypeIdList.Contains(type))
                    return ResponseErrorWithJson<T_FARMER_PUBLISHED_DEMAND>(result, "需求类型错误!");


                List<T_USER> operators = new List<T_USER>();
                if (!string.IsNullOrWhiteSpace(personIds) && personIds.Length > 0)
                {
                    var operatorUserIds = personIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(p => long.Parse(p))
                        .Distinct()
                        .ToList();

                    //判断这些用户是否是农机手
                    var operatorRoleId = (int)RoleType.MachineryOperator;
                    var operatorCount = _userRoleDemandTypeLevelRLService
                        .Count(p =>
                                p.RoleId == operatorRoleId &&
                                operatorUserIds.Contains(p.UserId) &&
                                p.DemandId == type
                            );

                    if (operatorCount != operatorUserIds.Count)
                        return ResponseErrorWithJson<T_FARMER_PUBLISHED_DEMAND>(result, "需求类型错误!");

                    var userList = _userRepository.GetAll(p => operatorUserIds.Contains(p.Id)).ToList();
                    operators.AddRange(userList);
                }

                if (operators.Count == 0)
                {
                    return ResponseErrorWithJson<T_FARMER_PUBLISHED_DEMAND>(result, ResponeString.NoPersonIdsForOperatorRequirement);
                }

                //验证参数是否有效
                string[] param = { cropId.ToString(), acreage.ToString(), type.ToString() };
                bool[] valid = { _commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == cropId && s.ParentCode == 100300))
                                       ,_commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == acreage && s.ParentCode == 100900))
                                       , _commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == type && (s.ParentCode == 100100 || s.ParentCode == 100800)))};
                for (int i = 0; i < valid.Length; i++)
                {
                    if (valid[i])
                    {
                        continue;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, param[i]);
                        return Json(result);
                    }
                }
                //验证userid是否有操作权限
                if (_commonrepository.CheckUserId(userId, (int)RoleType.Farmer))
                {
                    //执行添加操作
                    if (id == 0)
                    {
                        T_FARMER_PUBLISHED_DEMAND newRequirement = new T_FARMER_PUBLISHED_DEMAND();

                        newRequirement.DemandTypeId = type;
                        newRequirement.CropId = cropId;
                        newRequirement.VarietyId = 0;
                        newRequirement.AcresId = acreage;
                        newRequirement.Brief = description != null ? description : "";
                        newRequirement.ExpectedDate = date;
                        newRequirement.PublishStateId = 100501;
                        Dictionary<string, string> adc = StringHelper.GetAddress(address);

                        newRequirement.Province = adc["Province"];
                        newRequirement.City = adc["City"];
                        newRequirement.Region = adc["Region"];
                        newRequirement.Township = adc["Township"];
                        newRequirement.Village = adc["Village"];

                        newRequirement.DetailedAddress = detailAddress != null ? detailAddress : "";
                        newRequirement.PhoneNumber = PhoneNumber;
                        newRequirement.ExpectedStartPrice = Convert.ToDecimal(ExpectedStartPrice);
                        newRequirement.ExpectedEndPrice = Convert.ToDecimal(ExpectedEndPrice);
                        newRequirement.IsOpen = false;
                        newRequirement.CreateUserId = userId;
                        newRequirement.CreateTime = Utility.TimeHelper.GetChinaLocalTime();
                        newRequirement.IsDeleted = false;
                        _farmerRequirementService.Insert(newRequirement);
                        var farmerDemands = new List<T_USER_FARMERDEMANDS>();
                        foreach (var o in operators.Distinct())
                        {
                            farmerDemands.Add(new T_USER_FARMERDEMANDS
                            {
                                FarmerDemandId = newRequirement.Id,
                                UserId = o.Id
                            });
                        }

                        _farmerRequirementService.AssignOperators(farmerDemands);

                        result.Entity = newRequirement;
                        result.IsSuccess = true;
                        result.Message = ResponeString.SaveSuccessfullyMessage;
                        return Json(result);

                    }

                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = String.Format("{0} -" + ResponeString.NoJurisdiction, userId.ToString());
                }

                return Json(result);
            }
        }

        #region "需求详情"
        /// <summary>
        /// 获取需求详情(公共接口适用于大农户和产业商)
        /// </summary>
        /// <param name="id">需求id</param>
        /// <param name="roletype">角色编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult RequirementDetail(long id, int roletype)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            { //大农户
                if (roletype == Convert.ToInt32(RoleType.Farmer))
                {
                    FarmerPublishedDetailsModel req = _farmerRequirementService.GetFarmerDetail(id);
                    if (req == null)
                    {
                        result.Message = ResponeString.QueryWithNonMessage;
                    }
                    else
                    {
                        result.Entity = req;
                        result.Message = ResponeString.QuerySuccessfullyMessage;
                    }
                    result.IsSuccess = true;
                }
                //产业商
                else if (roletype == Convert.ToInt32(RoleType.Business))
                {
                    BusinessPublishedDetailsModel req = _farmerRequirementService.GetBusinessDetail(id);
                    if (req == null)
                    {
                        result.Message = ResponeString.QueryWithNonMessage;
                    }
                    else
                    {
                        result.Entity = req;
                        result.Message = ResponeString.QuerySuccessfullyMessage;
                    }
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, roletype.ToString());
                }

                return Json(result);
            }
        }
        #endregion

        #region "删除需求"
        /// <summary>
        /// 删除指定需求信息
        /// </summary>
        /// <param name="id">需求信息id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult RemoveRequirement(long id)
        {
            using (ResponseResult<T_FARMER_PUBLISHED_DEMAND> result = new ResponseResult<T_FARMER_PUBLISHED_DEMAND>())
            {
                T_FARMER_PUBLISHED_DEMAND req = _farmerRequirementService.GetByKey(id);

                if (req == null)
                {
                    result.Message = ResponeString.DeleteWithNonMessage;
                    result.IsSuccess = false;
                }
                else
                {
                    //进行中的允许删除
                    if (farmering.Contains(req.PublishStateId))
                    {
                        req.IsDeleted = true;
                        req.PublishStateId = (int)PublishState.CancelAlready; //100504;//已取消
                        _commonrepository.Modify<T_FARMER_PUBLISHED_DEMAND>(req, f => f.Id == id);
                        result.IsSuccess = true;
                        result.Entity = req;
                        result.Message = ResponeString.DeleteSuccessfullyMessage;
                        #region 调用E田接口,更新订单状态
                        //取消订单
                        if (req.DemandTypeId != (int)DuPont.Entity.Enum.FarmerDemandType.SellGrain && req.DemandTypeId != (int)DuPont.Entity.Enum.FarmerDemandType.SellSilage)
                        {
                            Task taskasync = new Task(() => UpdateCancleOrder(req));
                            taskasync.Start();
                        }
                        #endregion
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
        #endregion

        #region "发布给产业商的需求列表"
        /// <summary>
        /// 获取对产业商发布的需求
        /// </summary>
        /// <param name="userId">产业商id</param>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的条数</param>
        /// <param name="type">需求类型编号</param>
        /// <param name="region">区县（可选）</param>
        /// <param name="orderfield">排序标识（可选）</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult PublishedForBusiness(long userId, int pageIndex, int pageSize, int type, string region, string orderfield)
        {
            using (ResponseResult<List<PublishedModel>> result = new ResponseResult<List<PublishedModel>>())
            {
                pageIndex = pageIndex == 0 ? 1 : pageIndex;
                long TotalNums = 0;

                if (Utility.RegexHelper.IsMatch(region, @"^\d*\|\d*\|\d*\|\d*\|\d*$"))
                {
                    Dictionary<string, string> adc = StringHelper.GetAddress(region);


                    string coordinate = _commonrepository.GetCoordinate(userId);
                    string[] coordinatearry = coordinate.Split('|');
                    double businessLat = Convert.ToDouble(coordinatearry[0]);
                    double businessLng = Convert.ToDouble(coordinatearry[1]);
                    if (_commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => s.Code == type && s.ParentCode == 100800))
                    {
                        //获取发布列表
                        List<PublishedModel> list = _farmerRequirementService.GetRequirementList(businessLat, businessLng, type, adc["Province"].ToString(), adc["City"].ToString(), adc["Region"].ToString(), orderfield, pageIndex, pageSize, out  TotalNums);
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

        #region "发布给农机手的需求列表"
        /// <summary>
        /// 获取对农机手发布的需求
        /// </summary>
        /// <param name="userId">农机手id</param>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的条数</param>
        /// <param name="type">需求类型编号</param>
        /// <param name="region">区县编号（可选）</param>
        /// <param name="orderfield">排序标识（可选）</param>
        /// <param name="isAssignToMe">是否是指派给我的</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult PublishedForOperator(long userId, int pageIndex, int pageSize, int type, string region,
            string orderfield, int isAssignToMe = 0)
        {
            using (ResponseResult<List<PublishedModel>> result = new ResponseResult<List<PublishedModel>>())
            {
                pageIndex = pageIndex == 0 ? 1 : pageIndex;
                long TotalNums = 0;

                if (Utility.RegexHelper.IsMatch(region, @"^\d*\|\d*\|\d*\|\d*\|\d*$"))
                {
                    Dictionary<string, string> adc = StringHelper.GetAddress(region);
                    string coordinate = _commonrepository.GetCoordinate(userId);
                    string[] coordinatearry = coordinate.Split('|');
                    double operatorLat = Convert.ToDouble(coordinatearry[0]);
                    double operatorLng = Convert.ToDouble(coordinatearry[1]);
                    if (_commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => s.Code == type && s.ParentCode == 100100))
                    {
                        //获取需求列表
                        List<PublishedModel> list = _farmerRequirementService.GetRequirementList(operatorLat,
                            operatorLng, type, adc["Province"].ToString(), adc["City"].ToString(),
                            adc["Region"].ToString(), orderfield, pageIndex, pageSize, out  TotalNums, isAssignToMe > 0 ? userId : 0);

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

        #region "需求评价"
        /// <summary>
        /// 大农户进行订单评价
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="executeUserId">大农户id</param>
        /// <param name="userid">农机手id</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CommentRequirement(long id, long executeUserId, long userid, string commentString, int score)
        {
            using (ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> result = new ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>())
            {

                T_FARMER_PUBLISHED_DEMAND model = _commonrepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id && f.IsDeleted == false && f.CreateUserId == executeUserId);
                //验证当前需求编号是否存在于大农户需求表中且当前需求没有被删除并且是excuteuserId创建的
                if (model != null)
                {
                    T_FARMER_DEMAND_RESPONSE_RELATION responseRelation = _commonrepository.GetById<T_FARMER_DEMAND_RESPONSE_RELATION>(f => f.DemandId == id && f.UserId == userid);
                    //验证ueserid是否有效
                    if (responseRelation != null)
                    {
                        responseRelation.Score = score;
                        responseRelation.Comments = commentString != null ? commentString : "";
                        responseRelation.ReplyTime = Utility.TimeHelper.GetChinaLocalTime();
                        var bonusDPoint = int.Parse(this._sysSettingRepository.GetSetting(DataKey.BonusDPointByCommentSettingID).SETTING_VALUE);
                        responseRelation.BonusDPoint = bonusDPoint;

                        //评价成功后更改需求状态为已评价（100503）
                        T_FARMER_PUBLISHED_DEMAND fmodel = _commonrepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id);
                        if (fmodel == null)
                        {
                            result.IsSuccess = false;
                            result.Message = "数据意外丢失!";
                            return Json(result);
                        }

                        if (fmodel.DemandTypeId.ToString().StartsWith("1008"))
                        {
                            result.IsSuccess = false;
                            result.Message = "您不能评价产业商,操作被禁止!";
                            return Json(result);
                        }

                        //执行评价操作
                        _commonrepository.Modify<T_FARMER_DEMAND_RESPONSE_RELATION>(responseRelation, f => f.DemandId == id && f.UserId == userid);
                        fmodel.PublishStateId = 100503;//20170113记录还需要农机手的评价，才能变为已评价
                        _commonrepository.Modify<T_FARMER_PUBLISHED_DEMAND>(fmodel, f => f.Id == id);
                        if (score > 0)
                        {
                            var dpointTotal = score * bonusDPoint;
                            //给评价人自己添加系统默认先锋币
                            _commonrepository.AddDuPontPoint(model.CreateUserId, "评价添加先锋币", (int)SysCfg.SysUserId, bonusDPoint);

                            //给被评价人添加先锋币
                            _commonrepository.AddDuPontPoint(responseRelation.UserId, "被评价添加先锋币", (int)SysCfg.SysUserId, dpointTotal);

                            //计算被评价者的角色等级
                            var operatorRoleId = (int)RoleType.MachineryOperator;
                            var userRoleInfo = _userRoleService.GetAll(p => p.MemberType && p.UserID == userid && p.RoleID == operatorRoleId).FirstOrDefault();
                            if (userRoleInfo != null)
                            {
                                if (userRoleInfo.TotalReplyCount == null)
                                    userRoleInfo.TotalReplyCount = 0;

                                if (userRoleInfo.TotalStarCount == null)
                                    userRoleInfo.TotalStarCount = 0;

                                userRoleInfo.TotalReplyCount++;
                                userRoleInfo.TotalStarCount += score;
                                var averageStar = userRoleInfo.TotalStarCount / userRoleInfo.TotalReplyCount;
                                _userRoleService.Update(p => p.UserID == userid && p.RoleID == operatorRoleId, t => new T_USER_ROLE_RELATION
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
                        result.Message = String.Format("{0} - " + ResponeString.ParmetersInvalidMessage, userid.ToString());
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.NoJurisdiction;
                }

                return Json(result);
            }
        }
        #endregion

        #region "响应需求"
        /// <summary>
        /// 大农户接受产业商任务（向产业商需求响应表中添加记录）
        /// </summary>
        /// <param name="id">需求Id</param>
        /// <param name="userId">接收者id</param>
        /// <param name="weightrangetypeid">起购重量或者亩数编号</param>
        /// <param name="address">地址</param>
        /// <param name="phonenumber">手机号</param>
        /// <param name="brief">备注信息</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult AcceptTask(long id, long userId, int weightrangetypeid, string address, string phonenumber, string brief)
        {
            using (ResponseResult<T_BUSINESS_DEMAND_RESPONSE_RELATION> result = new ResponseResult<T_BUSINESS_DEMAND_RESPONSE_RELATION>())
            {

                //验证参数手机号是否有效
                if (!string.IsNullOrEmpty(phonenumber))
                {
                    //验证地址格式是否正确
                    if (!Utility.RegexHelper.IsMatch(address, @"^\d*\|\d*\|\d*\|\d*\|\d*$"))
                    {
                        result.IsSuccess = false;
                        result.Message = ResponeString.AddressError;
                        return Json(result);
                    }
                    //处理地址
                    Dictionary<string, string> adc = StringHelper.GetAddress(address);
                    address = _farmerRequirementService.GetAreaName(adc["Province"]) + _farmerRequirementService.GetAreaName(adc["City"]) + _farmerRequirementService.GetAreaName(adc["Region"]) + _farmerRequirementService.GetAreaName(adc["Township"]) + _farmerRequirementService.GetAreaName(adc["Village"]);
                    //获取当前需求
                    var firstmodel = _commonrepository.GetById<T_BUSINESS_PUBLISHED_DEMAND>(b => (b.Id == id && b.IsDeleted == false));
                    //验证weightrangetypeid是否有效
                    var typemodel = _commonrepository.GetById<T_SYS_DICTIONARY>(s => s.Code == weightrangetypeid);
                    if (firstmodel != null)
                    {
                        if (userId == firstmodel.CreateUserId)
                        {
                            result.IsSuccess = false;
                            result.Message = ResponeString.YourSelfRequirement;
                            return Json(result);
                        }
                        //验证当前用户id是否存在并且是否是大农户id
                        if (_commonrepository.CheckUserId(userId, (int)RoleType.Farmer))
                        {
                            if (typemodel == null)
                            {
                                result.IsSuccess = false;
                                result.Message = string.Format("{0} - " + ResponeString.ParmetersInvalidMessage, weightrangetypeid.ToString());
                                return Json(result);
                            }
                            //收粮
                            if (firstmodel.DemandTypeId == 100201)
                            {
                                if (typemodel.ParentCode != 100600)
                                {
                                    result.IsSuccess = false;
                                    result.Message = string.Format("{0} - " + ResponeString.ParmetersInvalidMessage, weightrangetypeid.ToString());
                                    return Json(result);
                                }
                            }
                            //收青储
                            if (firstmodel.DemandTypeId == 100202)
                            {
                                if (typemodel.ParentCode != 101200)
                                {
                                    result.IsSuccess = false;
                                    result.Message = string.Format("{0} - " + ResponeString.ParmetersInvalidMessage, weightrangetypeid.ToString());
                                    return Json(result);
                                }
                            }
                            //验证需求是否处于进行中状态
                            if (businessing.Contains(firstmodel.PublishStateId))
                            {

                                int wenumber = 0;
                                int firnumber = 0;
                                int areanumber = 0;
                                //获取产业商发布需求时的起购值
                                var weightmodel = _commonrepository.GetById<T_SYS_DICTIONARY>(s => (s.Code == firstmodel.FirstWeight));
                                //获取产业商发布不需求时的收购区间值
                                var areaweightmodel = _commonrepository.GetById<T_SYS_DICTIONARY>(s => (s.Code == firstmodel.AcquisitionWeightRangeTypeId));
                                if (areaweightmodel != null)
                                {
                                    if (areaweightmodel.DisplayName.Contains("吨"))
                                    {
                                        areanumber = areaweightmodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(areaweightmodel.DisplayName.Replace("吨以上", "")) + 1 : Convert.ToInt32(areaweightmodel.DisplayName.Replace("吨", "").Split('-')[1]);
                                    }
                                    else
                                    {
                                        areanumber = areaweightmodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(areaweightmodel.DisplayName.Replace("亩以上", "")) + 1 : Convert.ToInt32(areaweightmodel.DisplayName.Replace("亩", "").Split('-')[1]);
                                    }
                                }
                                if (typemodel.DisplayName.Contains("吨"))
                                {
                                    wenumber = typemodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(typemodel.DisplayName.Replace("吨以上", "")) + 1 : Convert.ToInt32(typemodel.DisplayName.Replace("吨", ""));
                                    firnumber = weightmodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(weightmodel.DisplayName.Replace("吨以上", "")) + 1 : Convert.ToInt32(weightmodel.DisplayName.Replace("吨", ""));
                                }
                                else
                                {
                                    wenumber = typemodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(typemodel.DisplayName.Replace("亩以上", "")) + 1 : Convert.ToInt32(typemodel.DisplayName.Replace("亩", ""));
                                    firnumber = weightmodel.DisplayName.Contains("以上") == true ? Convert.ToInt32(weightmodel.DisplayName.Replace("亩以上", "")) + 1 : Convert.ToInt32(weightmodel.DisplayName.Replace("亩", ""));
                                }
                                if (wenumber < firnumber)
                                {
                                    result.IsSuccess = false;
                                    result.Message = ResponeString.DissatisfyBusinessRequire;
                                    return Json(result);
                                }

                                //查询是否超过产业商的收购上限
                                //if (wenumber > areanumber)
                                //{
                                //    result.IsSuccess = false;
                                //    result.Message = ResponeString.ExceedBusinessRequirementCap;
                                //    return Json(result);
                                //}

                                //验证当前用户是否接受过此订单
                                if (!_commonrepository.CheckTypeid<T_BUSINESS_DEMAND_RESPONSE_RELATION>(b => b.DemandId == id && b.UserId == userId))
                                {
                                    T_BUSINESS_DEMAND_RESPONSE_RELATION model = new T_BUSINESS_DEMAND_RESPONSE_RELATION()
                                    {
                                        DemandId = id,
                                        UserId = userId,
                                        WeightRangeTypeId = weightrangetypeid,
                                        Address = address != null ? address : "",
                                        PhoneNumber = phonenumber,
                                        Brief = brief != null ? brief : "",
                                        CreateTime = Utility.TimeHelper.GetChinaLocalTime(),
                                        ReplyTime = Utility.TimeHelper.GetChinaLocalTime()
                                    };
                                    //添加响应记录
                                    _commonrepository.Add<T_BUSINESS_DEMAND_RESPONSE_RELATION>(model);
                                    //接受成功后更改需求状态为待评价（100502）
                                    firstmodel.PublishStateId = 100502;
                                    _commonrepository.Modify<T_BUSINESS_PUBLISHED_DEMAND>(firstmodel, b => b.Id == id);
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
                                result.Message = ResponeString.Requirementclose;
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
                        result.Message = ResponeString.NoRequirement;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.PhonenumberNotNull;
                }

                return Json(result);
            }

        }
        #endregion

        #region "我的应答列表"
        /// <summary>
        /// 大农户获取我的应答列表
        /// </summary>
        /// <param name="pageindex">页码数</param>
        /// <param name="pagesize">每页要显示的条数</param>
        /// <param name="isclosed">需求状态</param>
        /// <param name="userid">大农户id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult MyReply(int pageindex, int pagesize, int isclosed, long userid)
        {
            long TotalNums = 0;
            pageindex = pageindex == 0 ? 1 : pageindex;
            using (ResponseResult<List<ReplyModel>> result = new ResponseResult<List<ReplyModel>>())
            {

                //验证是否是大农户id
                if (_commonrepository.CheckUserId(userid, (int)RoleType.Farmer))
                {
                    List<ReplyModel> list = _farmerRequirementService.GetReplyList(pageindex, pagesize, isclosed, userid, out TotalNums);
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
        #endregion

        #region "应答详情"
        /// <summary>
        /// 应答详情
        /// </summary>
        /// <param name="requirementid">需求id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult AcceptRequirement(int requirementid)
        {
            using (ResponseResult<FarmerReplyDetailModel> result = new ResponseResult<FarmerReplyDetailModel>())
            {

                var model = _farmerRequirementService.GetReplyDetail(requirementid);
                if (model != null)
                {
                    result.Entity = model;
                    result.Entity.ReplyDetailedAddress = this._areaRepository.GetAreaNamesBy(result.Entity.ReplyDetailedAddress);
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
        #endregion

        #region "大农户发布给产业商和农机手的需求列表(未登录)"
        /// <summary>
        /// 获取大农户发布给产业商和农机手的需求列表
        /// </summary>        
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的条数</param>
        /// <param name="type">需求类型编号</param>        
        /// <param name="orderfield">排序标识（可选）</param> 
        /// <author>ww</author>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult PublishedForOperatorAndBusiness(int pageIndex, int pageSize, int type, string orderfield)
        {
            using (ResponseResult<List<PublishedModel>> result = new ResponseResult<List<PublishedModel>>())
            {
                pageIndex = pageIndex == 0 ? 1 : pageIndex;
                long TotalNums = 0;

                if (type != 0)
                {

                    if (_commonrepository.CheckTypeid<T_SYS_DICTIONARY>(s => s.Code == type && (s.ParentCode == 100100 || s.ParentCode == 100800)))
                    {
                        //获取给产业商和农机手的需求列表
                        List<PublishedModel> list = _farmerRequirementService.GetRequirementListForOperatorAndBusiness(pageIndex, pageSize, type, orderfield, out  TotalNums);
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

        #region "需求评价(加需求状态100507，100508后的接口)"
        /// <summary>
        /// 大农户进行订单评价
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="executeUserId">大农户id</param>
        /// <param name="userid">农机手id</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CommentRequirementForOperator(long id, long executeUserId, long userid, string commentString, int score)
        {
            using (ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> result = new ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>())
            {

                T_FARMER_PUBLISHED_DEMAND model = _commonrepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id && f.IsDeleted == false && f.CreateUserId == executeUserId);
                //验证当前需求编号是否存在于大农户需求表中且当前需求没有被删除并且是excuteuserId创建的
                if (model != null)
                {
                    T_FARMER_DEMAND_RESPONSE_RELATION responseRelation = _commonrepository.GetById<T_FARMER_DEMAND_RESPONSE_RELATION>(f => f.DemandId == id && f.UserId == userid);
                    //验证ueserid是否有效
                    if (responseRelation != null)
                    {
                        responseRelation.Score = score;
                        responseRelation.Comments = commentString != null ? commentString : "";
                        responseRelation.ReplyTime = Utility.TimeHelper.GetChinaLocalTime();
                        var bonusDPoint = int.Parse(this._sysSettingRepository.GetSetting(DataKey.BonusDPointByCommentSettingID).SETTING_VALUE);
                        responseRelation.BonusDPoint = bonusDPoint;

                        //评价成功后更改需求状态为已评价（100503）
                        T_FARMER_PUBLISHED_DEMAND fmodel = _commonrepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id);
                        if (fmodel == null)
                        {
                            result.IsSuccess = false;
                            result.Message = "数据意外丢失!";
                            return Json(result);
                        }

                        if (fmodel.DemandTypeId.ToString().StartsWith("1008"))
                        {
                            result.IsSuccess = false;
                            result.Message = "您不能评价产业商,操作被禁止!";
                            return Json(result);
                        }

                        //执行评价操作
                        _commonrepository.Modify<T_FARMER_DEMAND_RESPONSE_RELATION>(responseRelation, f => f.DemandId == id && f.UserId == userid);
                        //fmodel.PublishStateId = 100503;//20170113记录还需要农机手的评价，才能变为已评价
                        if (fmodel.PublishStateId == 100508)//农机手已经评价过，大农户再评论此需求单时，状态为已评价（100503）
                        {
                            fmodel.PublishStateId = 100503;
                        }
                        else
                        {
                            fmodel.PublishStateId = 100507;//大农户的评价
                        }
                        _commonrepository.Modify<T_FARMER_PUBLISHED_DEMAND>(fmodel, f => f.Id == id);
                        if (score > 0)
                        {
                            var dpointTotal = score * bonusDPoint;
                            //给评价人自己添加系统默认先锋币
                            _commonrepository.AddDuPontPoint(model.CreateUserId, "评价添加先锋币", (int)SysCfg.SysUserId, bonusDPoint);

                            //给被评价人添加先锋币
                            _commonrepository.AddDuPontPoint(responseRelation.UserId, "被评价添加先锋币", (int)SysCfg.SysUserId, dpointTotal);

                            //计算被评价者的角色等级
                            var operatorRoleId = (int)RoleType.MachineryOperator;
                            var userRoleInfo = _userRoleService.GetAll(p => p.MemberType && p.UserID == userid && p.RoleID == operatorRoleId).FirstOrDefault();
                            if (userRoleInfo != null)
                            {
                                if (userRoleInfo.TotalReplyCount == null)
                                    userRoleInfo.TotalReplyCount = 0;

                                if (userRoleInfo.TotalStarCount == null)
                                    userRoleInfo.TotalStarCount = 0;

                                userRoleInfo.TotalReplyCount++;
                                userRoleInfo.TotalStarCount += score;
                                var averageStar = userRoleInfo.TotalStarCount / userRoleInfo.TotalReplyCount;
                                _userRoleService.Update(p => p.UserID == userid && p.RoleID == operatorRoleId, t => new T_USER_ROLE_RELATION
                                {
                                    Star = averageStar,
                                    TotalStarCount = userRoleInfo.TotalStarCount,
                                    TotalReplyCount = userRoleInfo.TotalReplyCount
                                });
                            }
                        }

                        result.IsSuccess = true;
                        result.Entity = responseRelation;
                        //靠谱作业农机手接的单
                        if (responseRelation.SourceType == (int)DuPont.Entity.Enum.SourceType.JeRei)
                        {
                            //大农户评价靠谱作业农机手                
                            Task taskasync = new Task(() => CommentOrderForOperator(responseRelation,model.CreateUserId));
                            taskasync.Start();
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} - " + ResponeString.ParmetersInvalidMessage, userid.ToString());
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.NoJurisdiction;
                }

                return Json(result);
            }
        }
        #endregion

        #region 返回订单数据（传E田使用）
        /// <summary>
        /// 重新整理订单数据
        /// </summary>
        /// <param name="AcresId">亩数字典id</param>
        /// <author>ww</author>
        /// <returns></returns>
        public JsonResult ReturnOrderModel(long AcresId, string Address, long UserId, T_FARMER_PUBLISHED_DEMAND farmerdemand)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                DtoFarmerRequirementModel resultmodel = new DtoFarmerRequirementModel();
                try
                {
                    //查询亩数下限
                    T_SYS_DICTIONARY model = new DuPont.Repository.Sys_DictionaryRepository().GetByKey(AcresId);
                    if (model != null)
                    {
                        if (model.Order != 6)
                        {
                            string[] displays = model.DisplayName.Split('-');
                            if (displays.Length > 1)
                            {
                                resultmodel.Acreage = displays[0];
                            }
                        }
                        else
                        {
                            string[] displays = model.DisplayName.Split('亩');
                            if (displays.Length > 1)
                            {
                                resultmodel.Acreage = displays[0];
                            }
                        }
                    }
                    //查询地址经纬度
                    T_AREA areamodel = new DuPont.Repository.AreaRepository().GetByKey(Address);
                    if (areamodel != null)
                    {
                        resultmodel.Address = areamodel.Lng + "," + areamodel.Lat;
                    }
                    //查询用户名称
                    T_USER umodel = new DuPont.Repository.UserRepository().GetByWhere(x => x.Id == UserId);
                    if (umodel != null)
                    {
                        resultmodel.Name = umodel.UserName;
                    }
                    resultmodel.OrderId = farmerdemand.Id;
                    resultmodel.UserId = farmerdemand.CreateUserId;
                    resultmodel.CropId = farmerdemand.CropId;
                    resultmodel.DemandTypeId = farmerdemand.DemandTypeId;
                    resultmodel.Brief = farmerdemand.Brief;
                    resultmodel.PhoneNum = farmerdemand.PhoneNumber;
                    //如果是系统日期格式有tt.hh.mm.ss，t就会有PM，E田的mysql会报错
                    resultmodel.CreateTime = farmerdemand.CreateTime;
                    //干活日期
                    string tempdate = farmerdemand.ExpectedDate;
                    if (tempdate.Contains(',') || tempdate.Contains('-'))
                    {
                        string[] tempdates = tempdate.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tempdates.Length > 1)
                        {
                            resultmodel.StartDate = tempdates[0].ToString();
                            resultmodel.EndDate = tempdates[tempdates.Length - 1].ToString();
                        }
                        else
                        {
                            resultmodel.StartDate = tempdates[0].ToString();
                            resultmodel.EndDate = tempdates[0].ToString();
                        }

                    }
                    else if (tempdate.Contains('、'))
                    {
                        if (tempdate.EndsWith("、"))
                        {
                            tempdate = tempdate.TrimEnd('、');
                        }
                        string[] tempdates = tempdate.Split(new char[] { '、' }, StringSplitOptions.RemoveEmptyEntries);
                        if (tempdates.Length > 1)
                        {
                            resultmodel.StartDate = Convert.ToDateTime(tempdates[0].ToString()).ToString("yyyy-MM-dd");
                            resultmodel.EndDate = Convert.ToDateTime(tempdates[tempdates.Length - 1].ToString()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            resultmodel.StartDate = Convert.ToDateTime(tempdates[0].ToString()).ToString("yyyy-MM-dd");
                            resultmodel.EndDate = Convert.ToDateTime(tempdates[0].ToString()).ToString("yyyy-MM-dd");
                        }
                    }
                    else
                    {
                        resultmodel.StartDate = Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd");
                        resultmodel.EndDate = Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd");
                    }
                    #region 调用E田接口
                    var etcontent = new Dictionary<string, string>();
                    etcontent = ModelHelper.GetPropertyDictionary<DtoFarmerRequirementModel>(resultmodel);
                    //加密要传递的参数              
                    Dictionary<string, string> rurlcontent = EncryptDictionary(etcontent);
                    //e田接口地址
                    string Apiurl = ConfigHelper.GetAppSetting("EtApiUrl");
                    string Eturl = Apiurl + "/wei/work/dupont/farmer_order.jsp";
                    //证书的路径
                    var etcertification = GetCertificationFilePath();
                    //证书的密码
                    var etcertificationPwd = GetCertificationPwd();
                    var etresult = HttpAsynchronousTool.CustomHttpWebRequestPost(Eturl, rurlcontent, etcertification, etcertificationPwd);
                    var resultModel = JsonConvert.DeserializeObject<ETResponseResult<object>>(etresult);
                    if (resultModel.IsSuccess == true)
                    {  //写文件确认我方调用e田接口完成
                        string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "大农户的订单";
                        string parmeters = null;
                        foreach (var item in rurlcontent)
                        {
                            parmeters += item.Key + ":" + item.Value + "\r\n";
                        }

                        IOHelper.WriteLogToFile("FarmerRequirement/SaveRequirement" + logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
                    }
                    #endregion
                }

                catch (Exception ex)
                {
                    IOHelper.WriteLogToFile("FarmerRequirement/SaveRequirement" + "\r\n错误：" + ex.Message, RelativePath() + @"\DuPontRequestEtLog");
                    result.IsSuccess = false;
                }
                result.IsSuccess = true;
                result.Entity = resultmodel;
                return Json(result);
            }
        }
        #endregion

        #region 返回靠谱作业农机手数据（传E田使用）
        /// <summary>
        /// 评价靠谱作业的农机手
        /// </summary>
        /// <param name="Id">需求id</param>
        /// <param name="UserId">农机手id</param>
        /// <author>ww</author>
        /// <returns></returns>
        public JsonResult CommentOrderForOperator(T_FARMER_DEMAND_RESPONSE_RELATION rmodel,long FarmerId)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                DtoCommentFarmerDemandModel updatemodel = new DtoCommentFarmerDemandModel();
                try
                {
                    updatemodel.Id = rmodel.DemandId;
                    updatemodel.CommentString = rmodel.Comments;
                    updatemodel.Score = rmodel.Score;                   
                    updatemodel.FarmerUserId = FarmerId.ToString();                    
                    //查询用户名称
                    T_USER umodel = new DuPont.Repository.UserRepository().GetByWhere(x => x.Id == rmodel.UserId);
                    if (umodel != null)
                    {
                        //用weathercity记录靠谱作业的农机手id
                        updatemodel.OperatorUserId = umodel.WeatherCity;
                    }

                    #region 调用E田接口
                    var etcontent = new Dictionary<string, string>();
                    etcontent = ModelHelper.GetPropertyDictionary<DtoCommentFarmerDemandModel>(updatemodel);
                    //加密要传递的参数
                    Dictionary<string, string> rurlcontent = EncryptDictionary(etcontent);

                    //e田接口地址
                    string Apiurl = ConfigHelper.GetAppSetting("EtApiUrl");
                    string Eturl =" Apiurl "+ "/wei/work/dupont/evaluate_driver.jsp";
                    //证书的路径
                    var etcertification = GetCertificationFilePath();
                    //证书的密码
                    var etcertificationPwd = GetCertificationPwd();
                    var etresult = HttpAsynchronousTool.CustomHttpWebRequestPost(Eturl, rurlcontent, etcertification, etcertificationPwd);
                    var resultModel = JsonHelper.FromJsonTo<ETResponseResult<object>>(etresult);
                    if (resultModel.IsSuccess == true)
                    {
                        //写文件确认我方调用e田接口完成
                        string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "先锋帮大农户评价靠谱作业的农机手";
                        string parmeters = null;
                        foreach (var item in rurlcontent)
                        {
                            parmeters += item.Key + ":" + item.Value + "\r\n";
                        }
                        IOHelper.WriteLogToFile("FarmerRequirement/CommentOrderForOperator" + logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
                    }                    
                    #endregion
                }
                catch
                {
                    result.IsSuccess = false;
                }
                result.IsSuccess = true;
                result.Entity = updatemodel;
                return Json(result);
            }
        }
        #endregion

        #region 更新订单状态
        /// <summary>
        /// 先锋帮大农户取消需求订单
        /// </summary>
        /// <param name="t_FARMER_PUBLISHED_DEMAND"></param>
        /// <returns></returns>
        private string UpdateCancleOrder(T_FARMER_PUBLISHED_DEMAND t_FARMER_PUBLISHED_DEMAND)
        {
            DtoUpdateFarmerDemandModel updatemodel = new DtoUpdateFarmerDemandModel();
            updatemodel.Id = t_FARMER_PUBLISHED_DEMAND.Id;
            updatemodel.OrderState = t_FARMER_PUBLISHED_DEMAND.PublishStateId;
            updatemodel.FarmerName = t_FARMER_PUBLISHED_DEMAND.CreateUserId.ToString();
            #region 调用E田接口
            var etcontent = new Dictionary<string, string>();
            etcontent = ModelHelper.GetPropertyDictionary<DtoUpdateFarmerDemandModel>(updatemodel);
            //加密要传递的参数
            Dictionary<string, string> rurlcontent = EncryptDictionary(etcontent);
            //e田接口地址
            string Apiurl = ConfigHelper.GetAppSetting("EtApiUrl");
            string Eturl = Apiurl + "/wei/work/dupont/order_status.jsp";
            //证书的路径
            var etcertification = GetCertificationFilePath();
            //证书的密码
            var etcertificationPwd = GetCertificationPwd();
            var etresult = HttpAsynchronousTool.CustomHttpWebRequestPost(Eturl, rurlcontent, etcertification, etcertificationPwd);
            var resultModel = JsonConvert.DeserializeObject<ETResponseResult<object>>(etresult);
            if (resultModel.IsSuccess == true)
            {
                //写文件确认我方调用e田接口完成
                string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "先锋帮大农户取消需求订单";
                string parmeters = null;
                foreach (var item in rurlcontent)
                {
                    parmeters += item.Key + ":" + item.Value + "\r\n";
                }
                IOHelper.WriteLogToFile(logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
            }
            return resultModel.IsSuccess.ToString();
            #endregion
        }
        #endregion
    }
}