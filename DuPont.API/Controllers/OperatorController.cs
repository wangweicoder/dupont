using DuPont.API.Filters;
using DuPont.Entity.Enum;
using DuPont.Interface;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DuPont.API.Controllers
{
#if(!DEBUG)
    [AccessAuthorize]
#endif
    public class OperatorController : BaseController
    {
        private readonly IOperator repository;
        private readonly ICommon _commonRepository;
        private readonly INotification _notificationService;
        private readonly ISysSetting _sysSettingRepository;
        private readonly IUser_Role _userRoleService;
        private readonly IUser _userRepository;
        private readonly IOperatorInfoVerifciationRepository _operatorRepository;
        private readonly IRoleVerification _roleVerificationRepository;
        public OperatorController(IOperator repository,
            ICommon commonRepository,
            INotification notificationService,
            ISysSetting sysSettingRepository,
            IUser_Role userRoleService,
            IUser userRepository,
            IOperatorInfoVerifciationRepository operatorRepository,
            IRoleVerification roleVerificationRepository)
        {
            this.repository = repository;
            _commonRepository = commonRepository;
            _notificationService = notificationService;
            _sysSettingRepository = sysSettingRepository;
            _userRepository = userRepository;
            _userRoleService = userRoleService;
            _operatorRepository = operatorRepository;
            _roleVerificationRepository = roleVerificationRepository;
        }

        #region "响应大农户的需求"
        /// <summary>
        /// 农机手应大农户的需求（向大农户需求响应表添加记录）
        /// </summary>
        /// <param name="id">需求id</param>
        /// <param name="userId">农机手id</param>
        /// <returns>JsonResult.</returns>
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
                    //验证是否是发布给农机手的需求
                    if (_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => s.Code == firstmodel.DemandTypeId && s.ParentCode == 100100))
                    {
                        //验证userid是否是农机手id
                        if (_commonRepository.CheckUserId(userId, (int)RoleType.MachineryOperator))
                        {
                            //验证需求为待响应
                            if (firstmodel.PublishStateId == 100501)
                            {
                                //验证当前用户是否接受过此订单
                                if (!_commonRepository.CheckTypeid<T_FARMER_DEMAND_RESPONSE_RELATION>(f => f.DemandId == id && f.UserId == userId))
                                {
                                    T_FARMER_DEMAND_RESPONSE_RELATION model = new T_FARMER_DEMAND_RESPONSE_RELATION()
                                    {
                                        DemandId = id,
                                        UserId = userId,
                                        CreateTime = Utility.TimeHelper.GetChinaLocalTime(),
                                        ReplyTime = Utility.TimeHelper.GetChinaLocalTime(),
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

                                    //给大农户发送一个通知
                                    _notificationService.Insert(new T_NOTIFICATION
                                    {
                                        MsgContent = "您的需求有人响应啦,快去看看吧!",
                                        IsPublic = false,
                                        TargetUserId = firstmodel.CreateUserId,
                                        NotificationType = 3,
                                        NotificationSource = "",
                                        NotificationSourceId = firstmodel.Id
                                    });
                                    #region 调用E田接口,更新订单状态
                                    //接受订单                
                                    Task taskasync = new Task(() => AcceptOrder(firstmodel, model.UserId, model, 0));
                                    taskasync.Start();
                                    #endregion
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
                        result.Message = ResponeString.NotFAccept;
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
        #endregion

        #region "我的应答列表"
        /// <summary>
        /// 农机手获取我的应答列表
        /// </summary>
        /// <param name="pageindex">页码数</param>
        /// <param name="pagesize">每页要显示的条数</param>
        /// <param name="isclosed">The isclosed.</param>
        /// <param name="userid">用户编号</param>
        /// <returns>JsonResult.</returns>
        public JsonResult MyReply(int pageindex, int pagesize, int isclosed, long userid)
        {
            long TotalNums = 0;
            pageindex = pageindex == 0 ? 1 : pageindex;
            using (ResponseResult<List<ReplyModel>> result = new ResponseResult<List<ReplyModel>>())
            {

                //验证是否是农机手id
                if (_commonRepository.CheckUserId(userid, (int)RoleType.MachineryOperator))
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
        #endregion

        #region "应答详情"
        /// <summary>
        /// 应答详情
        /// </summary>
        /// <param name="requirementid">需求id</param>
        /// <returns>JsonResult.</returns>
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
        #endregion

        #region "农机手评价大农户需求单"
        /// <summary>
        /// 农机手评价大农户订单
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="FarmerUserId">大农户id</param>
        /// <param name="OperatorUserid">农机手id</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CommentRequirement(long id, long OperatorUserid, long FarmerUserId, string CommentString, int Score, int SourceType = 0)
        {
            using (ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> result = new ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>())
            {

                T_FARMER_PUBLISHED_DEMAND model = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id && f.IsDeleted == false && f.CreateUserId == FarmerUserId);
                //验证当前需求编号是否存在于大农户需求表中且当前需求没有被删除并且是excuteuserId创建的
                if (model != null)
                {
                    T_FARMER_DEMAND_RESPONSE_RELATION responseRelation = _commonRepository.GetById<T_FARMER_DEMAND_RESPONSE_RELATION>(f => f.DemandId == id && f.UserId == OperatorUserid);
                    //验证ueserid是否有效
                    if (responseRelation != null)
                    {
                        responseRelation.ScoreFarmer = Score;
                        responseRelation.CommentsFarmer = CommentString != null ? CommentString : "";
                        responseRelation.ReplyTimeFarmer = Utility.TimeHelper.GetChinaLocalTime();
                        var bonusDPoint = int.Parse(_sysSettingRepository.GetSetting(DataKey.BonusDPointByCommentSettingID).SETTING_VALUE);
                        responseRelation.BonusDPoint = bonusDPoint;

                        //评价成功后更改需求状态为已评价（100503）
                        T_FARMER_PUBLISHED_DEMAND fmodel = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id);
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
                        _commonRepository.Modify<T_FARMER_DEMAND_RESPONSE_RELATION>(responseRelation, f => f.DemandId == id && f.UserId == OperatorUserid);
                        //需求单状态判断
                        if (fmodel.PublishStateId == 100507)//大农户已经评价过，农机手再评论此需求单时，状态为已评价（100503）
                        {
                            fmodel.PublishStateId = 100503;
                        }
                        else
                        {
                            fmodel.PublishStateId = 100508;//农机手的评价
                        }
                        _commonRepository.Modify<T_FARMER_PUBLISHED_DEMAND>(fmodel, f => f.Id == id);
                        if (Score > 0)
                        {
                            var dpointTotal = Score * bonusDPoint;
                            //给评价人农机手自己添加系统默认先锋币
                            _commonRepository.AddDuPontPoint(responseRelation.UserId, "评价添加先锋币", (int)SysCfg.SysUserId, bonusDPoint);

                            //给被评价人大农户添加先锋币
                            _commonRepository.AddDuPontPoint(model.CreateUserId, "被评价添加先锋币", (int)SysCfg.SysUserId, dpointTotal);

                            //计算被评价者的角色等级
                            var operatorRoleId = (int)RoleType.Farmer;
                            var userRoleInfo = _userRoleService.GetAll(p => p.MemberType && p.UserID == FarmerUserId && p.RoleID == operatorRoleId).FirstOrDefault();
                            if (userRoleInfo != null)
                            {
                                if (userRoleInfo.TotalReplyCount == null)
                                    userRoleInfo.TotalReplyCount = 0;

                                if (userRoleInfo.TotalStarCount == null)
                                    userRoleInfo.TotalStarCount = 0;

                                userRoleInfo.TotalReplyCount++;
                                userRoleInfo.TotalStarCount += Score;
                                var averageStar = userRoleInfo.TotalStarCount / userRoleInfo.TotalReplyCount;
                                _userRoleService.Update(p => p.UserID == FarmerUserId && p.RoleID == operatorRoleId, t => new T_USER_ROLE_RELATION
                                {
                                    Star = averageStar,
                                    TotalStarCount = userRoleInfo.TotalStarCount,
                                    TotalReplyCount = userRoleInfo.TotalReplyCount
                                });
                            }
                        }

                        result.IsSuccess = true;
                        result.Entity = responseRelation;
                        #region 调用E田接口,更新订单状态
                        //评价大农户订单                
                        Task taskasync = new Task(() => AcceptOrder(model, responseRelation.UserId, responseRelation, 1));
                        taskasync.Start();
                        #endregion
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} - " + ResponeString.ParmetersInvalidMessage, OperatorUserid.ToString());
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
        /// <param name="model">需求表</param>
        /// <param name="UserId">农机手id</param>
        /// <param name="t_FARMER_DEMAND_RESPONSE_RELATION">应答表</param>
        /// <param name="type">0:接单，1:评价订单</param>
        /// <author>ww</author>
        /// <returns></returns>
        public JsonResult AcceptOrder(T_FARMER_PUBLISHED_DEMAND model, long UserId, T_FARMER_DEMAND_RESPONSE_RELATION t_FARMER_DEMAND_RESPONSE_RELATION, int type)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                DtoUpdateFarmerDemandModel updatemodel = new DtoUpdateFarmerDemandModel();
                try
                {
                    updatemodel.FarmerName = model.CreateUserId.ToString();
                    //查询用户名称
                    T_USER umodel = new DuPont.Repository.UserRepository().GetByWhere(x => x.Id == UserId);
                    if (umodel != null)
                    {
                        updatemodel.OperatoName = umodel.UserName;
                    }
                    if (type == 0)//接单
                    {
                        updatemodel.Id = model.Id;
                        updatemodel.OrderState = 100502;
                        
                    }
                    else if(type==1)//评价需求
                    {
                        updatemodel.Id = t_FARMER_DEMAND_RESPONSE_RELATION.DemandId;
                        updatemodel.OrderState = 100508;
                        updatemodel.CommentString = t_FARMER_DEMAND_RESPONSE_RELATION.CommentsFarmer;
                        updatemodel.Score = t_FARMER_DEMAND_RESPONSE_RELATION.ScoreFarmer;
                    }
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
                    var resultModel = JsonHelper.FromJsonTo<ETResponseResult<object>>(etresult);
                    if (resultModel.IsSuccess == true)
                    {
                        //写文件确认我方调用e田接口完成
                        string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "先锋帮的农机手接受或评价订单";
                        string parmeters = null;
                        foreach (var item in rurlcontent)
                        {
                            parmeters += item.Key + ":" + item.Value + "\r\n";
                        }
                        IOHelper.WriteLogToFile("Operator/AcceptOrder" + logErrstring + parmeters, RelativePath() + @"\DuPontRequestEtLog");
                    }

                    #endregion
                    result.IsSuccess = true;
                    result.Entity = updatemodel;
                }
                catch
                {
                    result.IsSuccess = false;
                }
                return Json(result);
            }
        }
        #endregion

        #region "靠谱作业农机手接大农户的需求"
        /// <summary>
        /// 农机手应大农户的需求（向大农户需求响应表添加记录）
        /// </summary>
        /// <param name="id">需求id</param>
        /// <param name="userId">农机手id</param>
        /// <returns>JsonResult.</returns>
#if(!DEBUG)
        [EtAccessAuthorize] 
#endif
        public JsonResult ReplyFarmerRequirement(long id, string userId, string Name, string NickName, string Address, string PhoneNum, string OtherMachinery, int Credit, int SourceType = 1)
        {
            using (ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> result = new ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>())
            {
                //e田农机手，在先锋帮的用户id
                long UserId = 0;
                T_FARMER_PUBLISHED_DEMAND firstmodel = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id);
                //验证需求是否存在于大农户需求表中 
                if (firstmodel != null)
                {
                    #region 保存农机手数据

                    List<T_USER> list = _userRepository.GetAll(c => c.WeatherCity == userId).ToList();
                    if (list.Count() > 0)
                    {
                        UserId = list[0].Id;
                    }
                    else
                    {
                        var user = new T_USER
                        {
                            PhoneNumber = PhoneNum,
                            Password = Encrypt.MD5Encrypt("111"),
                            CreateTime = DateTime.Now,
                            IsDeleted = false,
                            LoginUserName = PhoneNum,
                            LastLoginTime = DateTime.Now,
                            LastUpdatePwdTime = DateTime.Now,
                            SourceType = SourceType,
                            WeatherCity = userId//因为e田的农机手不登录，所以用本字段
                        };
                        int rows = _userRepository.Insert(user);
                        //构建农机手角色申请信息实体
                        var entity = new T_MACHINERY_OPERATOR_VERIFICATION_INFO()
                        {
                            UserId = user.Id,
                            CreateTime = DateTime.Now,
                            AuditState = 0,
                            Machinery = "[]",//必须有内容，否则后台getUpddata方法会报错
                            OtherMachineDescription = OtherMachinery,
                            RealName = Name,
                            PhoneNumber = PhoneNum
                        };
                        _operatorRepository.Insert(entity);

                        var userRoleDemandTypeLevelMapping = new Dictionary<int, int> { { 1, 1 } };

                        var verificationSuccess = this._roleVerificationRepository.ApproveOperatorVerification(entity.Id, user.Id, userRoleDemandTypeLevelMapping);
                        UserId = user.Id;//农机手id
                    }


                    #endregion
                    //验证是否是发布给农机手的需求
                    if (_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => s.Code == firstmodel.DemandTypeId && s.ParentCode == 100100))
                    {
                        //验证userid是否是农机手id
                        if (_commonRepository.CheckUserId(UserId, (int)RoleType.MachineryOperator))
                        {
                            //验证需求为待响应
                            if (firstmodel.PublishStateId == 100501)
                            {
                                //验证当前用户是否接受过此订单
                                if (!_commonRepository.CheckTypeid<T_FARMER_DEMAND_RESPONSE_RELATION>(f => f.DemandId == id && f.UserId == UserId))
                                {
                                    T_FARMER_DEMAND_RESPONSE_RELATION model = new T_FARMER_DEMAND_RESPONSE_RELATION()
                                    {
                                        DemandId = id,
                                        UserId = UserId,
                                        CreateTime = Utility.TimeHelper.GetChinaLocalTime(),
                                        ReplyTime = Utility.TimeHelper.GetChinaLocalTime(),
                                        //增加新增字段ww
                                        ReplyTimeFarmer = Utility.TimeHelper.GetChinaLocalTime(),
                                        //1 E田的农机手
                                        SourceType = 1
                                    };
                                    //添加响应记录
                                    _commonRepository.Add<T_FARMER_DEMAND_RESPONSE_RELATION>(model);
                                    //响应成功后更改需求状态为待评价（100502）
                                    firstmodel.PublishStateId = 100502;
                                    _commonRepository.Modify<T_FARMER_PUBLISHED_DEMAND>(firstmodel, f => f.Id == id);
                                    result.IsSuccess = true;
                                    result.Entity = model;

                                    //给大农户发送一个通知
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
                        result.Message = ResponeString.NotFAccept;
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
        #endregion

        #region "靠谱作业农机手评价大农户需求单"
        /// <summary>
        /// 农机手评价大农户订单
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="FarmerUserId">大农户id</param>
        /// <param name="OperatorUserid">农机手id</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
#if(!DEBUG)
        [EtAccessAuthorize]     
#endif
        public JsonResult EtCommentRequirement(long id, string OperatorUserid, long FarmerUserId, string CommentString, int Score, int SourceType = 1)
        {
            using (ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> result = new ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>())
            {

                T_FARMER_PUBLISHED_DEMAND model = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id && f.IsDeleted == false && f.CreateUserId == FarmerUserId);
                //验证当前需求编号是否存在于大农户需求表中且当前需求没有被删除并且是excuteuserId创建的
                if (model != null)
                {
                    //通过WeatherCity保存的e田农机手id，查询先锋帮农机手id
                    long operatorid = _userRepository.GetByWhere(x => x.WeatherCity.Contains(OperatorUserid)).Id;
                    T_FARMER_DEMAND_RESPONSE_RELATION responseRelation = _commonRepository.GetById<T_FARMER_DEMAND_RESPONSE_RELATION>(f => f.DemandId == id && f.UserId == operatorid);
                    //验证ueserid是否有效
                    if (responseRelation != null)
                    {
                        responseRelation.ScoreFarmer = Score;
                        responseRelation.CommentsFarmer = CommentString != null ? CommentString : "";
                        responseRelation.ReplyTimeFarmer = Utility.TimeHelper.GetChinaLocalTime();
                        var bonusDPoint = int.Parse(_sysSettingRepository.GetSetting(DataKey.BonusDPointByCommentSettingID).SETTING_VALUE);
                        responseRelation.BonusDPoint = bonusDPoint;

                        //评价成功后更改需求状态为已评价（100503）
                        T_FARMER_PUBLISHED_DEMAND fmodel = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id);
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
                        _commonRepository.Modify<T_FARMER_DEMAND_RESPONSE_RELATION>(responseRelation, f => f.DemandId == id && f.UserId == operatorid);
                        //需求单状态判断
                        if (fmodel.PublishStateId == 100507)//大农户已经评价过，农机手再评论此需求单时，状态为已评价（100503）
                        {
                            fmodel.PublishStateId = 100503;
                        }
                        else
                        {
                            fmodel.PublishStateId = 100508;//农机手的评价
                        }
                        _commonRepository.Modify<T_FARMER_PUBLISHED_DEMAND>(fmodel, f => f.Id == id);
                        if (Score > 0)
                        {
                            var dpointTotal = Score * bonusDPoint;
                            //给评价人农机手自己添加系统默认先锋币
                            _commonRepository.AddDuPontPoint(responseRelation.UserId, "评价添加先锋币", (int)SysCfg.SysUserId, bonusDPoint);

                            //给被评价人大农户添加先锋币
                            _commonRepository.AddDuPontPoint(model.CreateUserId, "被评价添加先锋币", (int)SysCfg.SysUserId, dpointTotal);

                            //计算被评价者的角色等级
                            var operatorRoleId = (int)RoleType.Farmer;
                            var userRoleInfo = _userRoleService.GetAll(p => p.MemberType && p.UserID == FarmerUserId && p.RoleID == operatorRoleId).FirstOrDefault();
                            if (userRoleInfo != null)
                            {
                                if (userRoleInfo.TotalReplyCount == null)
                                    userRoleInfo.TotalReplyCount = 0;

                                if (userRoleInfo.TotalStarCount == null)
                                    userRoleInfo.TotalStarCount = 0;

                                userRoleInfo.TotalReplyCount++;
                                userRoleInfo.TotalStarCount += Score;
                                var averageStar = userRoleInfo.TotalStarCount / userRoleInfo.TotalReplyCount;
                                _userRoleService.Update(p => p.UserID == FarmerUserId && p.RoleID == operatorRoleId, t => new T_USER_ROLE_RELATION
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
                        result.Message = String.Format("{0} - " + ResponeString.ParmetersInvalidMessage, OperatorUserid.ToString());
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "接口安全验证通过，但是" + ResponeString.NoJurisdiction;
                }

                return Json(result);
            }
        }
        #endregion

        #region "靠谱作业农机手取消大农户需求单"
        /// <summary>
        /// 农机手取消大农户订单
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="FarmerUserId">大农户id</param>
        /// <param name="OperatorUserId">农机手id</param>
        /// <param name="OrderState">订单状态539 机手已取消</param>      
        /// <returns>JsonResult.</returns>
        [HttpPost]
#if(!DEBUG)
        [EtAccessAuthorize]     
#endif
        public JsonResult CancelFarmerRequirement(long id, string OperatorUserId, string FarmerUserId, int OrderState)
        {
            using (ResponseResult<T_FARMER_PUBLISHED_DEMAND> result = new ResponseResult<T_FARMER_PUBLISHED_DEMAND>())
            {

                T_FARMER_PUBLISHED_DEMAND model = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(f => f.Id == id && f.IsDeleted == false && f.CreateUserId.ToString() == FarmerUserId);
                //验证当前需求编号是否存在于大农户需求表中且当前需求没有被删除并且是excuteuserId创建的
                if (model != null)
                {
                    if (OrderState == 539)
                        model.PublishStateId = (int)PublishState.WaitForResponse; //100501;//待响应
                    try
                    {
                        var taskresult = Task.Run(() => this._operatorRepository.UpdateDEMAND(model, id, OperatorUserId)).Result;
                        result.IsSuccess = taskresult > 0 ? true : false;
                    }
                    catch (Exception ex)
                    {
                        string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "错误";
                        IOHelper.WriteLogToFile(logErrstring + ex.Message, HttpContext.Server.MapPath("~/App_Data/Log") + @"\DuPontRequestEtLog");
                    }
                    //result.IsSuccess= _operatorRepository.UpdateDemand(model, id, OperatorUserId);
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "接口安全验证通过，但是" + ResponeString.NoJurisdiction;
                }

                return Json(result);
            }
        }
        #endregion
    }
}