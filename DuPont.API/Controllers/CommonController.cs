using DuPont.API.Filters;
using DuPont.API.Models.Common;
using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Global.ActionResults;
using DuPont.Interface;
using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Dtos.Foreground.Common;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using RestSharp;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.IO;
using DuPont.API.Models;

namespace DuPont.API.Controllers
{
#if(!DEBUG)
    [AccessAuthorize]
#endif
    public class CommonController : BaseController
    {
        private ISys_Dictionary _dictionaryRepository;
        private IArea _arearepository;
        private IFileInfoRepository _fileInfoRepository;
        private ICommon _commonRepository;
        private IUser _userRepository;
        private ISms _smsRepository;
        private ISmsMessage _smsMessageRepository;
        private ICarouselRepository _carouselRepository;
        private readonly ISysSetting _sysSettingRepository;
        private readonly IFarmerRequirement _farmerRequirementService;
        private readonly IBusiness _businessRepository;
        private readonly IOperator _operatorRepository;
        public CommonController(IFileInfoRepository fileInfoRepository,
            ICarouselRepository carouselRepository,
            ISysSetting sysSettingRepository,
            IFarmerRequirement FarmerRepository,
            IBusiness BusinessRepository,
            IOperator OperatorRepository
            )
        {
            _dictionaryRepository = new DuPont.Repository.Sys_DictionaryRepository();
            _arearepository = new DuPont.Repository.AreaRepository();
            _fileInfoRepository = fileInfoRepository;
            _commonRepository = new DuPont.Repository.CommonRepository();
            _userRepository = new DuPont.Repository.UserRepository();
            _smsRepository = new DuPont.Repository.RongLianYunSmsRepository();
            _smsMessageRepository = new DuPont.Repository.SmsMessageRepository();
            _carouselRepository = carouselRepository;
            _sysSettingRepository = sysSettingRepository;
            _businessRepository = BusinessRepository;
            _farmerRequirementService = FarmerRepository;
            _operatorRepository = OperatorRepository;
        }
        //
        // GET: /Common/
        public ActionResult Index()
        {
            return View();
        }
        #region 根据父节点获取子节点字典数据
        /// <summary>
        /// 根据父节点获取子节点字典数据
        /// 可以传入多个Code（用逗号分隔）,可以同时获取多个Code的直接子节点数据
        /// </summary>
        /// <param name="Code">字典编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult GetDictionaryItems(string Code)
        {
            using (var result = new ResponseResult<List<DictionaryModel>>())
            {

                if (string.IsNullOrEmpty(Code))
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.ParmetersInvalidMessage;
                    return Json(result);
                }
                Code = Code.Trim().TrimEnd(',');
                var codes = Code.Split(',');
                List<T_SYS_DICTIONARY> res = new List<T_SYS_DICTIONARY>();
                var dictionaries = _dictionaryRepository.GetAll(d => d.ParentCode == 0)
                    .Where(d => codes.Contains(d.Code.ToString()))
                    .OrderBy(d => d.Order);

                foreach (var c in dictionaries)
                {
                    res.AddRange(_dictionaryRepository.GetAll(d => d.ParentCode == c.Code).OrderBy(d => d.Order).ToList());
                }

                var entities = new List<DictionaryModel>();
                foreach (var d in res)
                {
                    entities.Add(new DictionaryModel()
                        {
                            Code = d.Code,
                            DisplayName = d.DisplayName,
                            Order = d.Order
                        });
                }

                result.Entity = entities;
                result.IsSuccess = true;

                return Json(result);
            }
        }
        #endregion
        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <param name="ParentAId">字典编号</param>
        /// <returns>JsonResult.</returns>
        public JsonResult GetAreaChild(string ParentAId)
        {
            using (ResponseResult<List<AreaViewModel>> result = new ResponseResult<List<AreaViewModel>>())
            {

                if (!string.IsNullOrEmpty(ParentAId))
                {
                    result.Entity = _arearepository.GetAreaChilds(ParentAId);
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.ParmetersInvalidMessage;
                }

                return Json(result);
            }


        }
        /// <summary>
        /// jsonp获取数据
        /// </summary>
        /// <param name="ParentAId"></param>
        /// <returns></returns>
        public ActionResult Getjsonpd(string ParentAId,string callback)
        {
            var rankName = ParentAId;
            return new JsonpResult<object>(new { success = true, rankName = rankName }, callback);

        }
        /// <summary>
        /// 上传图片附件
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Path">图片路径</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult UploadPicture(Int64 UserId, string Path)
        {
            using (ResponseResult<Object> result = new ResponseResult<Object>())
            {

                result.IsSuccess = true;
                var newFile = new T_FileInfo()
                {
                    Path = Path,
                    UserId = UserId,
                    CreateTime = DateTime.Now
                };

                int num = _fileInfoRepository.Insert(newFile);

                if (num <= 0)
                {
                    result.IsSuccess = false;
                    result.Message = "图片信息记录插入失败";
                }

                result.Entity = newFile.Id;

                return Json(result);
            }

        }

        /// <summary>
        /// 根据当前日期获取上传文件夹名称（格式：年份月份）
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetFolderNameByDate()
        {
            string res = string.Format("{0}{1:D2}", DateTime.Now.Year, DateTime.Now.Month);

            return res;
        }

        /// <summary>
        /// 检查文件类型是否是图片
        /// </summary>
        /// <param name="extensionName"></param>
        /// <returns></returns>
        private bool CheckFileIsImage(string extensionName)
        {
            string[] extensions = { "jpg", "jpeg", "bmp", "png" };
            return extensions.Any(e => e == extensionName.ToLower());
        }

        /// <summary>
        /// 评价列表
        /// </summary>
        /// <param name="userid">被评价者的id</param>
        /// <param name="roletype">角色类别</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns>根据roletype的值：3产业商对大农户做出的评价，4返回大农户对农机手做出的评价，5大农户对产业商做出的评价</returns>
        [HttpPost]
        public JsonResult CommentDetail(long userid, int roletype, int pageindex, int pagesize)
        {
            pageindex = pageindex == 0 ? 1 : pageindex;
            long TotalNums = 0;
            using (ResponseResult<List<CommentDetailModel>> result = new ResponseResult<List<CommentDetailModel>>())
            {

                //产业商
                if (roletype == (int)RoleType.Business)
                {
                    //验证是否是产业商id
                    if (_commonRepository.CheckUserId(userid, (int)RoleType.Business))
                    {
                        List<CommentDetailModel> list = _commonRepository.GetBusinessCommentDetail(userid, pageindex, pagesize, out TotalNums);
                        if (list != null)
                        {
                            result.Entity = list;
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = ResponeString.NoJurisdiction;
                    }
                }
                //大农户 
                else if (roletype == (int)RoleType.Farmer)
                {
                    //验证是否是大农户id
                    if (_commonRepository.CheckUserId(userid, (int)RoleType.Farmer))
                    {
                        List<CommentDetailModel> list = _commonRepository.GetFarmerCommentDetail(userid, pageindex, pagesize, out TotalNums);
                        if (list != null)
                        {
                            result.Entity = list;
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = ResponeString.NoJurisdiction;
                    }
                }
                //农机手
                else if (roletype == (int)RoleType.MachineryOperator)
                {
                    //验证是否是农机手id
                    if (_commonRepository.CheckUserId(userid, (int)RoleType.MachineryOperator))
                    {
                        List<CommentDetailModel> list = _commonRepository.GetOperatorCommentDetail(userid, pageindex, pagesize, out TotalNums);
                        if (list != null)
                        {
                            result.Entity = list;
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = ResponeString.NoJurisdiction;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.ParmetersInvalidMessage;
                }

                result.TotalNums = TotalNums;
                result.PageIndex = pageindex;
                result.PageSize = pagesize;
                return Json(result);
            }
        }

        /// <summary>
        /// 检查手机验证码的发送状态
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckValidateCodeState(string phoneNumber)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                //1 手机格式不正确!
                //2 验证码发送记录不存在
                //3 验证码未过期
                //4 验证码已过期
                //5 验证码发送记录保存失败
                //100  验证码发送记录保存成功


                //验证手机号格式
                if (string.IsNullOrEmpty(phoneNumber) || PageValidate.IsMobile(phoneNumber) == false)
                {
                    //返回验证消息
                    result.IsSuccess = false;
                    result.State.Id = 1;
                    result.State.Description = "手机格式不正确!";
                    result.Message = result.State.Description;
                    return Json(result);
                }

                //验证最近一次验证码发送记录是否存在
                var lastSmsRecord = _smsMessageRepository.GetByKey(phoneNumber);
                if (lastSmsRecord == null)
                {
                    //返回验证消息
                    result.IsSuccess = false;
                    result.State.Id = 2;
                    result.State.Description = "验证码发送记录不存在";
                    result.Message = result.State.Description;
                    return Json(result);
                }

                //验证上次发送的验证码是否过期
                var expiredMinutes = 0;
                if (int.TryParse(ConfigurationManager.AppSettings["smsValidMinutes"], out expiredMinutes) == false)
                    expiredMinutes = 2;

                var smsCodeExpired = (TimeHelper.GetChinaLocalTime() - lastSmsRecord.SendTime).TotalMinutes > expiredMinutes;
                if (smsCodeExpired == false)
                {
                    //返回验证消息
                    result.IsSuccess = false;
                    result.State.Id = 3;
                    result.State.Description = "验证码未过期";
                    result.Entity = lastSmsRecord.Captcha;
                    result.Message = result.State.Description;
                    return Json(result);
                }
                else
                {
                    //返回验证消息
                    result.IsSuccess = true;
                    result.State.Id = 4;
                    result.State.Description = "验证码已过期";
                    result.Message = result.State.Description;
                    return Json(result);
                }
            }
        }

        /// <summary>
        /// 保存手机验证码发送记录
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <param name="validateCode">验证码</param>
        /// <returns></returns>
        public JsonResult SaveValidateCodeSendRecord(string phoneNumber, string validateCode)
        {
            var saveRecordSuccess = false;
            using (ResponseResult<Object> result = new ResponseResult<object>())
            {

                //验证最近一次验证码发送记录是否存在
                var lastSmsRecord = _smsMessageRepository.GetByKey(phoneNumber);
                if (lastSmsRecord == null)
                {
                    //不存在,插入一条新记录
                    var smsSendRecordEntity = new T_SMS_MESSAGE
                     {
                         Captcha = validateCode,
                         PhoneNumber = phoneNumber,
                         SendTime = TimeHelper.GetChinaLocalTime()
                     };
                    saveRecordSuccess = this._smsMessageRepository.Insert(smsSendRecordEntity) > 0;
                }
                else
                {
                    //存在,更新该条数据
                    lastSmsRecord.Captcha = validateCode;
                    lastSmsRecord.SendTime = TimeHelper.GetChinaLocalTime();
                    saveRecordSuccess = this._smsMessageRepository.Update(lastSmsRecord) > 0;
                }

                if (saveRecordSuccess)
                {
                    //记录到数据库成功
                    result.IsSuccess = true;
                    result.State.Id = 100;
                    result.State.Description = "验证码发送记录保存成功";
                    return Json(result);
                }
                else
                {
                    //记录到数据库失败
                    result.IsSuccess = false;
                    result.State.Id = 5;
                    result.State.Description = "验证码发送记录保存失败!";
                    return Json(result);
                }
            }
        }

        /// <summary>
        /// 获取轮播图片
        /// </summary>
        /// <param name="RoleId">角色编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CarouselPictures(int RoleId)
        {
            using (var result = new ResponseResult<List<CarouselFile>>())
            {
                var files = _carouselRepository.GetCarouselFiles(RoleId);
                result.IsSuccess = true;
                result.Entity = files;
                return Json(result);
            }
        }

        /// <summary>
        /// 获取指定地区拥有指定角色的人数
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="roleType">角色编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult GetPersonNumber(long id, int roleType)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            {

                //产业商
                if (roleType == (int)RoleType.Business)
                {
                    var model = _commonRepository.GetById<T_BUSINESS_PUBLISHED_DEMAND>(b => b.Id == id);
                    if (model == null)
                    {
                        result.IsSuccess = false;
                        result.Message = string.Format("{0} - " + ResponeString.ParmetersInvalidMessage, id.ToString());
                        result.Entity = 0;
                        return Json(result);
                    }
                    result.IsSuccess = true;
                    result.Entity = _commonRepository.GetNumber((int)RoleType.Farmer, model.CreateUserId, model.Province, model.City, model.Region);
                }
                //大农户
                else if (roleType == (int)RoleType.Farmer)
                {
                    var model = _commonRepository.GetById<T_FARMER_PUBLISHED_DEMAND>(b => b.Id == id);
                    if (model == null)
                    {
                        result.IsSuccess = false;
                        result.Message = string.Format("{0} - " + ResponeString.ParmetersInvalidMessage, id.ToString());
                        result.Entity = 0;
                        return Json(result);
                    }
                    //发布给农机手的需求
                    if (_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == model.DemandTypeId && s.ParentCode == 100100)))
                    {
                        result.IsSuccess = true;
                        if (model.IsOpen == false)//如果指定农机手，待响应就是指定的农机手的数量
                        {

                            result.Entity = _farmerRequirementService.SelectOperators(model.Id);
                        }
                        else
                            result.Entity = _commonRepository.GetNumber((int)RoleType.MachineryOperator, model.CreateUserId, model.Province, model.City, model.Region);
                    }
                    //发布给产业商的需求
                    else if (_commonRepository.CheckTypeid<T_SYS_DICTIONARY>(s => (s.Code == model.DemandTypeId && s.ParentCode == 100800)))
                    {
                        result.IsSuccess = true;
                        result.Entity = _commonRepository.GetNumber((int)RoleType.Business, model.CreateUserId, model.Province, model.City, model.Region);
                    }
                    else
                    {
                        result.IsSuccess = false;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                }

                return Json(result);
            }
        }

        /// <summary>
        /// 获取大农户附近农机手列表
        /// </summary>
        [HttpPost]
        public JsonResult GetOperatorsForFarmerRequire(NearbyOperatorListInput input)
        {
            using (var result = new ResponseResult<List<OperatorProfile>>())
            {
                if (!ModelState.IsValid)
                    return ResponseErrorWithJson(result, GetModelStateErrorMessage(ModelState));

                var person = _userRepository.GetByKey(input.UserId);
                long totalNums = 0;
                if (person != null)
                {
                    result.Entity = _commonRepository.GetOperatorsForFarmerRequirementWithDemandType(person.Province, person.City, input.PageIndex ?? 1, input.PageSize ?? int.MaxValue, input.DemandTypeId ?? 0, out totalNums);
                }

                SetJosnResult<List<OperatorProfile>>(result, input.PageIndex ?? 1, input.PageSize ?? int.MaxValue, totalNums, "获取附近农机手列表成功!");
                return Json(result);
            }
        }

        protected string currentURL()
        {
            return string.Format("{0}://{1}", Request.Url.Scheme, Request.Url.Authority);

        }



        /// <summary>
        /// 检查App的版本
        /// </summary>
        /// <param name="platform">当前平台(android/ios)</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult AppVersion(string platform)
        {
            using (ResponseResult<VersionUpgradeModel> result = new ResponseResult<VersionUpgradeModel>())
            {

                var lastVersionModel = this._commonRepository.GetLastVersion(platform);
                if (lastVersionModel != null)
                {
                    result.IsSuccess = true;
                    result.Entity.DownloadUrl = lastVersionModel.DownloadURL ?? "";
                    result.Entity.Platform = platform ?? "";
                    result.Entity.Version = lastVersionModel.Version ?? "";
                    result.Entity.VersionCode = lastVersionModel.VersionCode;
                    result.Entity.ChangeLog = lastVersionModel.ChangeLog ?? "";
                    result.Entity.IsOpen = lastVersionModel.IsOpen ? 1 : 0;

                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "没有该平台发布的版本数据";
                }

                return Json(result);
            }
        }

        /// <summary>
        /// 检查项目部署
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult CheckDeployment()
        {
            object result = 0;

            var value = this._commonRepository.CheckDatabaseDeployment();
            if (value > 0)
            {
                result = "db--ok";
            }

            return View(result);
        }

        /// <summary>
        /// 检查第三方登录的图标显示状态（仅针对ios终端使用）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Check_CAS_DisplayState()
        {
            using (var result = new ResponseResult<object>())
            {
                var setting = _sysSettingRepository.GetSetting(DataKey.CAS_DisplayState_ID);
                if (setting.IsNull())
                {
                    result.Entity = "0";
                }
                else
                {
                    result.Entity = setting.SETTING_VALUE;
                }
                SetJosnResult<object>(result, 1, 1, 0, "获取第三方登录显示状态成功。(0:隐藏;1:显示)");
                return new JsonResultEx(result);
            }
        }
        #region "获取评价列表(整合后的)"
        /// <summary>
        /// 评价列表
        /// </summary>
        /// <param name="userid">被评价者的id</param>
        /// <author>ww</author>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns>根据roletype的值：3产业商对大农户做出的评价，4返回大农户对农机手做出的评价，5大农户对产业商做出的评价</returns>
        [HttpPost]
        public JsonResult PublicCommentDetail(long userid, int pageindex, int pagesize)
        {
            pageindex = pageindex == 0 ? 1 : pageindex;
            long TotalNums = 0, resultTotalNums = 0;
            using (ResponseResult<List<CommentDetailModel>> result = new ResponseResult<List<CommentDetailModel>>())
            {
                List<T_USER_ROLE_RELATION> rolelist = _userRepository.GetRoleRelationInfo(userid);
                if (rolelist != null && rolelist.Count > 0)
                {
                    List<T_USER_ROLE_RELATION> roleInfo = rolelist.OrderBy(x => x.RoleID).ToList();
                    //评价列表
                    List<CommentDetailModel> list = new List<CommentDetailModel>();
                    //角色数量
                    var roleInfoCount = roleInfo.Count;
                    for (int i = 0; i < roleInfoCount; i++)
                    {
                        var currentRoleInfo = roleInfo[i];
                        var roletype = (Entity.Enum.RoleType)currentRoleInfo.RoleID;
                        //产业商
                        if (roletype == RoleType.Business)
                        {
                            list = _commonRepository.GetBusinessCommentDetail(userid, pageindex, pagesize, out TotalNums);
                            if (list != null)
                            {
                                result.Entity = list;
                            }
                            result.IsSuccess = true;
                            resultTotalNums += TotalNums;
                        }
                        //大农户 
                        else if (roletype == RoleType.Farmer)
                        {
                            //验证是否是大农户id                            
                            list = _commonRepository.GetFarmerCommentList(userid, pageindex, pagesize, out TotalNums);//大农户的接口要增加农机手对他的评价
                            resultTotalNums += TotalNums;
                        }
                        //农机手
                        if (roletype == RoleType.MachineryOperator)
                        {
                            //验证是否是农机手id
                            List<CommentDetailModel> listoperator = _commonRepository.GetOperatorCommentDetail(userid, pageindex, pagesize, out TotalNums);
                            if (listoperator != null)
                            {
                                list.AddRange(listoperator);//将农机手的评价加到大农户集合后面
                            }
                            resultTotalNums += TotalNums;
                        }
                    }//end for
                    if (list != null)
                    {
                        result.Entity = list;
                    }
                    result.IsSuccess = true;
                    result.Message = ResponeString.QuerySuccessfullyMessage;
                }//end if
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.NoJurisdiction;
                }//end else                
                result.TotalNums = resultTotalNums;
                result.PageIndex = pageindex;
                result.PageSize = pagesize;
                return Json(result);
            }
        }
        #endregion

        #region "我的应答列表"
        /// <summary>
        /// 获取我的应答列表
        /// </summary>
        /// <param name="pageindex">页码数</param>
        /// <param name="pagesize">每页要显示的条数</param>
        /// <param name="isclosed">需求状态</param>
        /// <param name="userid">用户id</param>
        /// <author>ww</author>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult MyReply(int pageindex, int pagesize, int isclosed, long userid)
        {
            long TotalNums = 0, resultTotalNums = 0;
            pageindex = pageindex == 0 ? 1 : pageindex;
            using (ResponseResult<List<CommonReplyModel>> result = new ResponseResult<List<CommonReplyModel>>())
            {
                List<T_USER_ROLE_RELATION> rolelist = _userRepository.GetRoleRelationInfo(userid);
                if (rolelist != null && rolelist.Count > 0)
                {
                    List<T_USER_ROLE_RELATION> roleInfo = rolelist.OrderBy(x => x.RoleID).ToList();
                    //我的应答列表 增加字段后的
                    List<CommonReplyModel> list = new List<CommonReplyModel>();
                    //角色数量
                    var roleInfoCount = roleInfo.Count;
                    for (int i = 0; i < roleInfoCount; i++)
                    {
                        var currentRoleInfo = roleInfo[i];
                        var roletype = (Entity.Enum.RoleType)currentRoleInfo.RoleID;
                        //产业商
                        if (roletype == RoleType.Business)
                        {
                            list = _businessRepository.GetBusinessReplyList(currentRoleInfo.RoleID, pageindex, pagesize, isclosed, userid, out TotalNums);
                            if (list != null)
                            {
                                result.Entity = list;
                            }
                            result.IsSuccess = true;
                            resultTotalNums += TotalNums;
                        }
                        //验证是否是大农户id
                        else if (roletype == RoleType.Farmer)
                        {
                            list = _farmerRequirementService.GetFarmerReplyList(currentRoleInfo.RoleID, pageindex, pagesize, isclosed, userid, out TotalNums);
                            resultTotalNums += TotalNums;
                        }
                        if (roletype == RoleType.MachineryOperator)
                        {
                            List<CommonReplyModel> operatorlist = _operatorRepository.GetOperatorReplayList(currentRoleInfo.RoleID, pageindex, pagesize, isclosed, userid, out TotalNums);

                            if (operatorlist != null)
                            {
                                if (operatorlist.Count > 0 && list != null)
                                {
                                    list.AddRange(operatorlist);
                                }
                                else
                                {
                                    list = operatorlist;
                                }
                                resultTotalNums += TotalNums;
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = string.Format("{0} - " + ResponeString.NoJurisdiction, userid.ToString());
                        }

                    }//endfor
                    if (list != null)
                    {
                        result.Entity = list;
                    }
                    result.IsSuccess = true;
                    result.Message = ResponeString.QuerySuccessfullyMessage;
                }
                result.TotalNums = TotalNums;
                result.PageIndex = pageindex;
                result.PageSize = pagesize;
                return Json(result);
            }
        }
        #endregion
        
        #region 查询天气接口
        /// <summary>
        /// 查询天气接口
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        private static dynamic SelectWeatherinfo(string cityName)
        {
            string url = ConfigHelper.GetAppSetting(DataKey.WeatherUrlNew);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var request = new WeatherRequest();
            dic.Add("key", request.key1);//key
            dic.Add("location", cityName);//地址
            dic.Add("language", request.language);
            dic.Add("unit", request.unit);
            string apiresult = HttpRequestHelper.sendGet(url + "/weather/now.json", dic);
            var js = new JavaScriptSerializer();
            var resultJson = js.Deserialize<dynamic>(apiresult);
            return resultJson;
        }
        #endregion

        #region 查询天气预报
        /// <summary>
        /// 首页天气预报接口
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetWeatherRealTime(string userId)
        {
            var result = new ResponseResult<GetWeatherRealTimeResponse>();
            
            if (string.IsNullOrEmpty(userId))
            {
                result.Message = "用户id为空";
                return Json(result);
            }
            try
            {
                string Cityname = null;
                //查询改userid
                var userModel = _userRepository.GetByKey(long.Parse(userId));
                if (userModel == null)
                {
                    result.Message = "该用户不存在";
                    return Json(result);
                }
                //查看此用户有木有天气城市的值
                if (!string.IsNullOrEmpty(userModel.WeatherCity))
                {
                    //查询到常用城市信息
                    Cityname = userModel.WeatherCity;
                   
                }
                //查询城市信息
                else if (!string.IsNullOrEmpty(userModel.City))
                {
                    var name = _arearepository.GetByKey(userModel.City).DisplayName;                    
                    //查询城市名称
                    Cityname= name;                    
                }
                //查询省
                else if (!string.IsNullOrEmpty(userModel.Province))
                {
                    var cityModel = _arearepository.GetAreaChilds(userModel.Province).OrderBy(p => p.AID).FirstOrDefault();
                    Cityname = cityModel.DisplayName;                   
                }
                //               
                if (string.IsNullOrEmpty(Cityname))
                {
                    Cityname = "北京";
                }
                else
                {
                    string SpecialCity = ConfigHelper.GetAppSetting("SpecialCity").ToString();//城市没有天气数据的特殊城市
                    if (SpecialCity.Contains(Cityname))//如果是特殊城市
                    {
                        string MappingRegin = ConfigHelper.GetAppSetting("MappingRegin").ToString();//没有天气数据的特殊城市，对应的区县
                        string[] specialcitys = SpecialCity.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);//特殊城市集合
                        int index = Array.IndexOf(specialcitys, Cityname);//找到特殊城市索引
                        string[] mappingregins = MappingRegin.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);//特殊城市对应区县的集合
                        Cityname = mappingregins[index];
                    }
                    else
                    {                      
                        var endIndex = Cityname.IndexOf('市') > 0 ? Cityname.IndexOf('市') : Cityname.Length;
                        Cityname = Cityname.Substring(0, endIndex);
                    }
                }
                var resultJson = SelectWeatherinfo(Cityname);
                var resultNow = resultJson["results"][0];
                var resulrModel = resultNow["now"];
                GetWeatherRealTimeResponse entity=new GetWeatherRealTimeResponse();
                entity.Name=Cityname;
                entity.Now_Code=resulrModel["code"];
                entity.Now_Text=resulrModel["text"];
                entity.Now_Temperature=resulrModel["temperature"];
                result.Entity = entity;               
                result.IsSuccess = true;
                result.Message = "成功";
                return Json(result);

            }
            catch (Exception)
            {
                var cityName = "北京";
                var resultJson = SelectWeatherinfo(cityName);
                var resultNow = resultJson["results"][0];
                var resulrModel = resultNow["now"];
                GetWeatherRealTimeResponse entity = new GetWeatherRealTimeResponse();
                entity.Name = cityName;
                entity.Now_Code = resulrModel["code"];
                entity.Now_Text = resulrModel["text"];
                entity.Now_Temperature = resulrModel["temperature"];
                result.Entity = entity;
                result.IsSuccess = true;
                result.Message = "成功";                
            }
            return Json(result);

        }      
        #endregion

        #region 修改城市接口
        /// <summary>
        ///  修改城市接口
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="name">城市名称</param>
        /// <returns></returns>
        public JsonResult ModifyUserWeatherCity(string userId, string name)
        {
            var result = new ResponseResult<object>();
            try
            {
                var userModel = _userRepository.GetByKey(long.Parse(userId));
                if (userModel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "该用户不存在";
                    result.Entity = null;
                    return Json(result);
                }
                userModel.WeatherCity = name;
                _userRepository.Update(userModel);
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "系统繁忙";
                result.Entity = null;
                return Json(result);
            }
            result.IsSuccess = true;
            result.Message = "成功";
            result.Entity = null;
            return Json(result);

        }
        #endregion

        #region 获取天气预报接口
        /// <summary>
        ///  获取天气预报接口
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="name">城市名称</param>
        /// <returns></returns>
        public JsonResult GetSearchWeather(string userId, string name)
        {
            var result =  new ResponseResult<DtoGetSearchWeatherResponse>();
            try
            {
                var userModel = _userRepository.GetByKey(long.Parse(userId));
                if (userModel == null)
                {
                    result.IsSuccess = false;
                    result.Message = "该用户不存在";
                    return Json(result);
                }
                else
                {
                    #region  天气类型匹配
                    //白天晴
                    int[] fine = { 0, 1, 38 };
                    //晚上 晴
                    int[] Clear = { 2, 3 };
                    //多云
                    int[] Cloudy = { 4, 5, 6, 7, 8 };
                    //阴天
                    int[] Overcast = { 9, 37 };
                    //雨天
                    int[] Rain = { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
                    //雪
                    int[] Snow = { 20, 21, 22, 23, 24, 25 };
                    //沙尘
                    int[] Dust = { 26, 27, 28, 29 };
                    //雾霾
                    int[] Foggy = { 30, 31 };
                    //风
                    int[] Windy = { 32, 33, 34, 35, 36 };
                    #endregion
                    #region
                    if (string.IsNullOrEmpty(userId))
                    {
                        result.IsSuccess = false;
                        result.Message = "用户id为空";
                        return Json(result);
                    }
                    if (string.IsNullOrEmpty(name))
                    {
                        result.IsSuccess = false;
                        result.Message = "城市名称为空";
                        return Json(result);
                    }
                    else
                    {
                        string url = ConfigHelper.GetAppSetting(DataKey.WeatherUrlNew);
                        Dictionary<string, string> dicNow = new Dictionary<string, string>();
                        var request = new WeatherRequest();
                        var weatherModel = new DtoGetSearchWeatherResponse();
                        dicNow.Add("key", request.key1);//key
                        dicNow.Add("location", name);//地址
                        dicNow.Add("language", request.language);
                        dicNow.Add("unit", request.unit);
                        string apiresultNow = HttpRequestHelper.sendGet(url + "/weather/now.json", dicNow);//查询实时天气
                        var js = new JavaScriptSerializer();
                        var resultJson = js.Deserialize<dynamic>(apiresultNow);
                        var resultNow = resultJson["results"][0];
                        var resulrModel = resultNow["now"];
                        weatherModel.Name = name;
                        weatherModel.Now_Text = resulrModel["text"];
                        weatherModel.Now_Code = int.Parse(resulrModel["code"]);
                        weatherModel.Now_Temperature = int.Parse(resulrModel["temperature"]);
                    #endregion
                        #region 加背景图片
                        if (fine.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 100;//晴
                        }
                        else if (Clear.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 101;//晚上，晴
                        }
                        else if (Cloudy.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 102;//多云
                        }
                        else if (Overcast.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 103;//阴天
                        }
                        else if (Rain.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 104;//雨天
                        }
                        else if (Snow.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 105;//雪天
                        }
                        else if (Dust.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 106;//沙尘
                        }
                        else if (Foggy.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 107;//雾霾
                        }
                        else if (Windy.Contains(weatherModel.Now_Code))
                        {
                            weatherModel.BackgroundType = 108;//风
                        }
                        #endregion
                        Dictionary<string, string> dic = new Dictionary<string, string>();

                        dic.Add("key", request.key);//key
                        dic.Add("location", name);//地址
                        dic.Add("language", request.language);
                        dic.Add("unit", request.unit);
                        dic.Add("start", "0");
                        dic.Add("days", "5");
                        string apiresult = HttpRequestHelper.sendGet(url + "/weather/daily.json", dic);
                        var resultModel = JsonConvert.DeserializeObject<DtoGetSearchWeather>(apiresult);
                        var apiModel = resultModel.results[0];
                        foreach (var item in apiModel.daily)
                        {
                            weatherModel.Daily.Add(new WeatherResponse()
                            {
                                Data = Utility.TimeHelper.GetMilliSeconds(Convert.ToDateTime(item.date)),//日期
                                Day_Text = item.text_day,//白天文字
                                Day_Code = item.code_day,//白天code
                                Night_Text = item.text_night,//晚间文字
                                Night_Code = item.code_night,//晚间code
                                Height = item.high,//最高
                                Low = item.low,//最低
                            });
                        }
                        result.Entity = weatherModel;
                        result.IsSuccess = true;
                        result.Message = "成功";

                        return Json(result);

                    }
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Message = "没有此地区天气信息";
                return Json(result);
            }          

        }
        #endregion

        #region 获得玉米价格
        /// <summary>
        /// 保存玉米价格json文件
        /// </summary>
        /// <param name="everydayurl"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetCornPrices(string everydayurl)
        {
            string result = "{";
            if (!string.IsNullOrWhiteSpace(everydayurl))
            {
                string priceUrl = "www.yumi.com.cn" + everydayurl;
                // Console.WriteLine("原粮价格_玉米价格地址："+ priceUrl);"http://" + "www.yumi.com.cn/html/2017/01/20170119093205203868.html"
                result += "\"IsSuccess\":true ,";
                result += "\"Message\":\"查询成功！\",";
                result += "\"State\":{\"Id\": 200,\"Description\": \"请求成功\"},";
                result += "\"Entity\":{";
                string td1 = "";
                string patternDateTime = everydayurl.Substring(everydayurl.LastIndexOf('/') + 1, 8); //第一个表格的标题   
                var datePath = DateTime.Now.ToString("yyyyMMdd");
                //if (datePath != patternDateTime)
                //{
                //    result = "{\"IsSuccess\":false,";
                //    result += "\"Message\":\"抓取的数据的时间为：" + patternDateTime + ",当前时间为" + datePath + "！\",";
                //    result += "\"Entity\":{";
                //    result += "}}";
                //    return result;
                //}               
                string tempHtml = GetTableTitleByHtml("http://" + priceUrl);
                var r1 = new Regex("(\\s|&nbsp;)");//已<开始的字符，出现0次或者多次，
                var tempHtmlTI = r1.Replace(tempHtml, "");
                string patterntitle = @"(日东北[\s\S]*?</b>)"; //第一个表格的标题   
                string patterntitle1 = @"(日华北黄淮[\s\S]*?</b>)"; //第2个表格的标题  
                string patterntitle2 = @"(日北方港口[\s\S]*?</b>)"; //第3个表格的标题  
                string patterntitle3 = @"(日南方港口[\s\S]*?</b>)"; //第4个表格的标题  
                string patterntitle4 = @"(日南方销区[\s\S]*?</b>)"; //第5个表格的标题  
                string patternRemarks = @"(注：(.|^\n)*?</p>)";
                Regex regtitle = new Regex(patterntitle, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                Regex regtitle1 = new Regex(patterntitle1, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                Regex regtitle2 = new Regex(patterntitle2, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                Regex regtitle3 = new Regex(patterntitle3, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                Regex regtitle4 = new Regex(patterntitle4, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                Regex regRemarks = new Regex(patternRemarks, RegexOptions.IgnoreCase);
                string dateTime = DateTime.Now.ToString("yyyy年MM月dd");           
                string tabletitle = dateTime + regtitle.Match(tempHtmlTI).Value;//1
                string tabletitle1 = dateTime + regtitle1.Match(tempHtmlTI).Value;//2                
                string tabletitle2 = dateTime + regtitle2.Match(tempHtmlTI).Value;//3
                string tabletitle3 = dateTime + regtitle3.Match(tempHtmlTI).Value;//4
                string tabletitle4 = dateTime + regtitle4.Match(tempHtmlTI).Value;//5
                var tableRemarks = regRemarks.Matches(tempHtmlTI);
                // string remarkStr = @"(</?\/g)";//</p>
                var r = new Regex("<(\\S*?)[^>]*>");//已<开始的字符，出现0次或者多次，

                //标题 
                tabletitle = r.Replace(tabletitle, "");//1
                //标题1 
                tabletitle1 = r.Replace(tabletitle1, "");//1
                //标题2 
                tabletitle2 = r.Replace(tabletitle2, "");//1
                //标题3 
                tabletitle3 = r.Replace(tabletitle3, "");//1
                //标题4 
                tabletitle4 = r.Replace(tabletitle4, "");//1              

                //备注
                var remark = r.Replace(tableRemarks[0].Value, "");//1

                //备注1
                var remark1 = r.Replace(tableRemarks[1].Value, "");//1

                //备注2
                var remark2 = r.Replace(tableRemarks[2].Value, "");//1

                //备注3
                var remark3 = r.Replace(tableRemarks[3].Value, "");//1

                //备注4
                var remark4 = r.Replace(tableRemarks[4].Value, "");//1
                #region table1 第一个表格
                string tempstr = GetTableByHtml(tempHtml, 1, 21);
                string[] tr = tempstr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo\":[{\"title\":\"" + tabletitle + "\",\"remark\":\"" + remark + "\",\"list\":[ ";
                var trlength = tr.Length;
                for (int i = 0; i < trlength; i++)
                {

                    string[] td = tr[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0 || i == 8 || i == 17 || i == 19)
                    {
                        td1 = tr[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        result += "{\"name\":\"" + td1.ToString() + "\",\"cityList\": [";   //黑龙江
                        result += "{\"city\":\"" + td[1].ToString() + "\",";
                        result += "\"PurchasingPrice\":\"" + td[2].ToString() + "\",";
                        result += "\"PContrastWithYesterday\":\"" + td[3].ToString() + "\",";
                        result += "\"ExPostPrice\":\"" + td[4].ToString() + "\",";
                        result += "\"EContrastWithYesterday\":\"" + td[5].ToString() + "\"";
                        result += "},";

                    }
                    else 
                    {
                        result += "{ \"city\":\"" + td[0].ToString() + "\",";
                        result += " \"PurchasingPrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PContrastWithYesterday\":\"" + td[2].ToString() + "\",";
                        result += " \"ExPostPrice\":\"" + td[3].ToString() + "\",";
                        result += " \"EContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                        result += "},";
                    }
                    if (i == 7 || i == 16 || i == 18 || i == 19) //是否闭合
                    {
                        result = result.TrimEnd(',') + "]},";//闭合之前删除最后的逗号
                    }

                }

                result = result.TrimEnd(new char[] { ',' }) + "]}],";
                #endregion
                #region table2第二个表格
                string tempstr1 = GetTableByHtml(tempHtml, 22, 45);
                string[] tr1 = tempstr1.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo1\":[{\"title\":\"" + tabletitle1 + "\",\"remark\":\"" + remark1 + "\",\"list\":[ ";
                for (int i = 0; i < tr1.Length; i++)
                {
                    string[] td = tr1[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0 || i == 5 || i == 7 || i == 11 || i == 18 || i == 22)
                    {
                        td1 = tr1[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        result += "{\"name\":\"" + td1.ToString() + "\",\"cityList\": [";
                        result += "{ \"city\":\"" + td[1].ToString() + "\",";
                        result += " \"PurchasingPrice\":\"" + td[2].ToString() + "\",";
                        result += " \"PContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                    }
                    else
                    {
                        result += "{ \"city\":\"" + td[0].ToString() + "\",";
                        result += " \"PurchasingPrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PContrastWithYesterday\":\"" + td[2].ToString() + "\",";
                        result = result.TrimEnd(new char[] { ',' }) + "},";
                    }
                    if (i == 4 || i == 6 || i == 10 || i == 17 || i == 21 || i == tr1.Length - 1) //是否闭合
                    {
                        result = result.TrimEnd(',') + "]},";//闭合之前删除最后的逗号
                    }
                }
                result = result.TrimEnd(new char[] { ',' }) + "]}],";
                #endregion
                #region table3第三个表格
                string tempstr2 = GetTableByHtml(tempHtml, 46, 50);
                string[] tr2 = tempstr2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo2\":[{\"title\":\"" + tabletitle2 + "\",\"remark\":\"" + remark2 + "\",\"list\":[";
                for (int i = 0; i < tr2.Length; i++)
                {
                    string[] td = tr2[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0)
                    {
                        td1 = tr2[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        result += "{\"name\":\"" + td1.ToString() + "\",\"cityList\": ["; //大连
                        result += "{ \"city\":\"" + td[1].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[2].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[3].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                        result += "},";
                    }

                    else if (i == 1)
                    {
                        result += "{ \"city\":\"" + td[0].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                        result = result.TrimEnd(',') + "]},";//闭合之前删除最后的逗号 大连闭合
                    }
                    else if (i == 2 || i == 3)
                    {
                        result += "{\"name\":\"" + td[0].ToString() + "\",\"cityList\": [";
                        result += "{ \"city\":\"\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                        result = result.TrimEnd(',') + "]},";//闭合之前删除最后的逗号  
                    }
                }
                result = result.TrimEnd(new char[] { ',' }) + "]}],";
                #endregion
                #region table4第四个表格
                string tempstr3 = GetTableByHtml(tempHtml, 51, 59);
                string[] tr3 = tempstr3.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo3\":[{\"title\":\"" + tabletitle3 + "\",\"remark\":\"" + remark3 + "\",\"list\":[";
                for (int i = 0; i < tr3.Length; i++)
                {
                    string[] td = tr3[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0 || i == 3 || i == 5 || i == 6)
                    {
                        td1 = tr3[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        result += "{\"name\":\"" + td1.ToString() + "\",\"cityList\": [";
                        result += "{ \"city\":\"" + td[1].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[2].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[3].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                        result += "},";
                    }
                    else if (i == 7)
                    {
                        result += "{\"name\":\"" + td[0].ToString() + "\",\"cityList\": [";
                        result += "{ \"city\":\"\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";

                    }
                    else
                    {
                        result += "{ \"city\":\"" + td[0].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                    }
                    if (i == 2 || i == 4 || i == 5 || i == 6 || i == tr3.Length - 1)
                    {
                        result = result.TrimEnd(',') + "]},";//闭合之前删除最后的逗号  
                    }
                }
                result = result.TrimEnd(new char[] { ',' }) + "]}],";
                #endregion
                #region table5第五个表格
                string tempstr4 = GetTableByHtml(tempHtml, 60, 69);
                string[] tr4 = tempstr4.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo4\":[{\"title\":\"" + tabletitle4 + "\",\"remark\":\"" + remark4 + "\",\"list\":[ ";
                for (int i = 0; i < tr4.Length; i++)
                {
                    string[] td = tr4[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0 || i == 2)
                    {
                        td1 = tr4[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                        result += "{\"name\":\"" + td1.ToString() + "\",\"cityList\": [";
                    }
                    if (i > 3)
                    {

                        result += "{\"name\":\"" + td[0].ToString() + "\",\"cityList\": [";
                        result += "{ \"city\":\"" + td[1].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[2].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[3].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                        result += "},";
                        result = result.TrimEnd(',') + "]},";//闭合之前删除最后的逗号  
                    }
                    else
                    {
                        if (td.Length == 4)
                        {
                            result += "{ \"city\":\"" + td[0].ToString() + "\",";
                            result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                            result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                            result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                            result += "},";
                        }
                        else
                        {
                            result += "{ \"city\":\"" + td[1].ToString() + "\",";
                            result += " \"AveragePrice\":\"" + td[2].ToString() + "\",";
                            result += " \"PriceType\":\"" + td[3].ToString() + "\",";
                            result += " \"AContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                            result += "},";
                        }
                    }
                    if (i == 1 || i == 3)
                    {
                        result = result.TrimEnd(',') + "]},";//闭合之前删除最后的逗号  
                    }
                }
                #endregion
                result = result.TrimEnd(',').Replace("<span", " ") + "]}]}}";

                string serverAppPath = Server.MapPath(@"~/FileJson");

                if (!Directory.Exists(serverAppPath))
                {
                    Directory.CreateDirectory(serverAppPath);

                }
                string serverAppPathFile = serverAppPath + "/" + datePath + ".json";
                if (!System.IO.File.Exists(serverAppPathFile))
                {
                    try
                    {
                        FileStream fs = new FileStream(serverAppPathFile, FileMode.OpenOrCreate);
                        byte[] data = System.Text.Encoding.UTF8.GetBytes(result);
                        //开始写入
                        fs.Write(data, 0, data.Length);
                        //清空缓冲区、关闭流
                        fs.Flush();
                        fs.Close();
                    }
                    catch (Exception ex)
                    {
                        IOHelper.WriteLogToFile("/Common/GetCornPrices:" + ex.Message, HttpContext.Server.MapPath("~/App_Data/Log"));
                        result += "\"IsSuccess\":true,";
                        result += "\"Message\":\"应用程序异常！\",";
                        result += "\"Entity\":{}";
                        result += "}";
                        return result;
                    }
                    //    System.IO.File.Create(serverAppPathFile);
                    //    ////获取文件信息
                    //    //FileInfo fileInfo = new FileInfo(serverAppPathFile);
                    //    ////获得该文件的访问权限
                    //    //System.Security.AccessControl.FileSecurity fileSecurity = fileInfo.GetAccessControl();
                    //    ////添加ereryone用户组的访问权限规则 完全控制权限
                    //    //fileSecurity.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, AccessControlType.Allow));
                    //    ////添加Users用户组的访问权限规则 完全控制权限
                    //    //fileSecurity.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, AccessControlType.Allow));
                    //    ////设置访问权限
                    //    //fileInfo.SetAccessControl(fileSecurity);
                }
                return result;
            }
            else
            {
                result += "\"IsSuccess\":true,";
                result += "\"Message\":\"输入参数为空！\",";
                result += "\"Entity\":{}";
                result += "}";
                return result;
            }

        }

        /// <summary>
        /// 获取数据抓取地址
        /// </summary>
        /// <returns>返回地址链接</returns>
        [HttpPost]
        public string GetModPriceUrl()
        {
            string result = "{";
            try
            {
                string dominAddress = "http://www.yumi.com.cn/yumijiage/index.html";
                string loadstr = HttpRequestHelper.sendGet(dominAddress, null);
                string pattern = @"(<ul class=""priceSubChe"">[\s\S]*?</ul>)";
                Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                string cpattern = @"(<p>.*</p>)";
                Regex creg = new Regex(cpattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                result += "\"IsSuccess\":true ,";
                result += "\"Message\":\"查询成功！\",";
                result += "\"Entity\":{";
                result += "\"dayurllist\":[";
                foreach (Match item in reg.Matches(loadstr))
                {
                    foreach (Match citem in creg.Matches(item.Value))
                    {
                        string hrefurl = new Regex("/html.*.html").Match(citem.Value).Value;
                        string title = new Regex("\">.*</a>").Match(citem.Value).Value;
                        title = title.Replace("\">", "").Substring(0, title.Replace("\">", "").IndexOf("</"));
                        result += "{\"title\":\"" + title + "\",";
                        result += " \"href\":\"" + hrefurl + "\"},";
                    }
                }
                return result.TrimEnd(',') + "]}}";
            }
            catch (Exception ex)
            {
                result += "\"IsSuccess\":true,";
                result += "\"Message\":\"应用程序异常！\",";
                result += "\"Entity\":{}";
                result += "}";
                return result;
            }
        }

        /// <summary>
        /// 数据保存在table中
        /// </summary>
        /// <returns></returns>
        private static string GetTableByHtml(string htmlstr, int start, int end)
        {
            string htmlTablestr = htmlstr.Substring(htmlstr.IndexOf("<table"), htmlstr.LastIndexOf("table") - htmlstr.IndexOf("<table"));
            //行数据 
            string pattern = @"(<tr>[\s\S]*?</tr>)";//"(<p.class=\".*</p>)";                       
            Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //单元格数据
            string cellpattern = @"(<.*</span>)";            
            Regex cellreg = new Regex(cellpattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            string title = "";
            for (int i = start; i < end; i++)
            {
                string row = reg.Matches(htmlTablestr)[i].Value;
                int rowc = cellreg.Matches(row).Count;
                if (rowc > 4)
                {
                    for (int j = 0; j < rowc; j++)
                    {
                        string rowvalue = new Regex(">(.*)</span>").Match(cellreg.Matches(row)[j].Value).Value;
                        var r = new Regex("<(\\S*?)[^>]*>");//已<开始的字符，出现0次或者多次，
                        var kg = "&nbsp;";
                        rowvalue = r.Replace(rowvalue, "");
                        title += rowvalue.Replace(">", "").Replace(kg, "") + ",";
                    }
                }
                else //没有span标签时，只有4行
                {
                    string cellpattern2 = @"(<p[\s\S]*?</p>)";
                    Regex cellreg2 = new Regex(cellpattern2, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    int rowc2 = cellreg2.Matches(row).Count;
                    for (int j = 0; j < rowc2; j++)
                    {
                        string rowvalue = new Regex(">(.*)</span>").Match(cellreg2.Matches(row)[j].Value).Value;
                        if (rowvalue == "")//没有span标签时，匹配不到数据，再用p标签来匹配
                        {
                            //(.|\n)*?//
                            string rowvalue2 = new Regex(">[\\s\\S]*?</p>").Match(cellreg2.Matches(row)[j].Value).Value;                            
                            var r = new Regex("<(\\S*?)[^>]*>");//已<开始的字符，出现0次或者多次，
                            var kg = "&nbsp;";
                            rowvalue2 = r.Replace(rowvalue2, "");
                            title += rowvalue2.Replace(">", "").Replace(kg, "") + ",";
                        }
                        else
                        {
                            var r = new Regex("<(\\S*?)[^>]*>");//已<开始的字符，出现0次或者多次，
                            var kg = "&nbsp;";
                            rowvalue = r.Replace(rowvalue, "");
                            title += rowvalue.Replace(">", "").Replace(kg, "") + ",";
                        }
                    }
                }
                title += ";";
            }
            return title;
        }
        private static string GetTableTitleByHtml(string priceurl)
        {
            string htmlstr = HttpRequestHelper.sendGet(priceurl, null);

            return htmlstr;
        }
        /// <summary>
        /// 获取玉米价格日期列表
        /// </summary>
        /// <author>ww</author>
        /// <returns>目录中的文件名集合</returns>
        [HttpPost]
        public string GetModList(int pageIndex, int pageSize = 10)
        {  
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            string result = "{";
            try
            {
                string dominAddress = "http://www.yumi.com.cn/yumijiage/index.html";
                string loadstr = HttpRequestHelper.sendGet(dominAddress, null);
                string pattern = @"(<ul class=""priceSubChe"">[\s\S]*?</ul>)";
                Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                string cpattern = @"(<p>.*</p>)";
                Regex creg = new Regex(cpattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                int totalnums = pageIndex * pageSize;
                result += "\"IsSuccess\":true ,";
                result += "\"Message\":\"查询成功！\",";
                result += "\"PageIndex\":" + pageIndex + ",";
                result += "\"PageSize\":" + pageSize + ",";
                result += "\"State\":{\"Id\": 200,\"Description\": \"请求成功\"},";
                result += "\"Entity\":{";
                result += "\"InfoList\":[";
                if (reg.Matches(loadstr).Count > 0)
                {
                    Match citem = creg.Match(reg.Matches(loadstr)[0].Value);
                    //返回数据条数
                    int eachnum = 0;
                    string title = new Regex("\">.*</a>").Match(citem.Value).Value;
                    title = title.Replace("\">", "").Substring(0, title.Replace("\">", "").IndexOf("</"));
                    title = title.Substring(title.IndexOf("日") + 1);
                    //日期路径
                    string cornurl = Server.MapPath(@"~/FileJson/");
                    DirectoryInfo theFolder = new DirectoryInfo(cornurl);
                    //获得文件列表并按时间倒序
                    var fileInfo = theFolder.GetFiles().OrderByDescending(x => x.CreationTime);
                    foreach (FileInfo NextFile in fileInfo)  //遍历文件
                    {
                        if (eachnum < totalnums && eachnum >= (pageIndex - 1) * pageSize)
                        {
                            string filename = NextFile.Name;
                            filename = filename.Substring(0, filename.IndexOf('.'));
                            string year = filename.Substring(0, 4) + "年";
                            string month = filename.Substring(4, 2) + "月";
                            string day = filename.Substring(6, 2) + "日";
                            string CornTitle = year + month + day;
                            result += "{\"title\":\"" + CornTitle + title + "\",";
                            result += " \"href\":\"" + filename + "\"},";
                        }
                        eachnum++;
                    }

                }
                return result.TrimEnd(',') + "]}}";
            }
            catch (Exception ex)
            {
                result = "{";
                result += "\"IsSuccess\":true,";
                result += "\"Message\":\"应用程序异常！\",";
                result += "\"Entity\":{}";
                result += "}";
                return result;
            }
        }
    
        /// <summary>
        /// 获取玉米价格表
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetModDataList(string dateTime)
        {
            var result = "";
            try
            {
                string pths = Server.MapPath(@"~/FileJson/" + dateTime + ".json");
                if (!System.IO.File.Exists(pths))
                {
                    result += "\"IsSuccess\":false,";
                    result += "\"Message\":\"文件不存在！\",";
                    result += "\"Entity\":{}";
                    result += "}";
                    return result;
                }
                StreamReader sr = new StreamReader(pths, Encoding.UTF8);
                result = sr.ReadToEnd();
                //清空缓冲区、关闭流
                sr.Dispose();
                sr.Close();
            }
            catch (Exception ex)
            {
                result += "\"IsSuccess\":true,";
                result += "\"Message\":\"应用程序异常！\",";
                result += "\"Entity\":{}";
                result += "}";
                return result;
            }
            return result;
        }       

        #endregion
    }
}