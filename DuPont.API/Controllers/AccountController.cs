using DuPont.API.Filters;
using DuPont.API.Models;
using DuPont.API.Models.Account;
using DuPont.Global.ActionResults;
using DuPont.Interface;
using DuPont.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Extensions;
using System.Text.RegularExpressions;
using DuPont.Utility.LogModule.Model;
using DuPont.Models.Models;
using DuPont.Entity.Enum;
using DuPont.Models.Dtos.Foreground.Account;
using DuPont.Global.Filters.ActionFilters;
using AutoMapper;
namespace DuPont.API.Controllers
{
#if(!DEBUG)
    [AccessAuthorize]
#endif
    [Validate]
    public class AccountController : BaseController
    {
        private readonly IUser _userService;
        private readonly IQQUser _qqUserRepository;
        private readonly IWeChatUser _weChatUserRepository;
        private readonly IFarmerVerficationInfoRepository _farmerVerificationRepository;
        private readonly IPublished_Demand _fpdrepository;
        private readonly IOperatorInfoVerifciationRepository _operatorRepository;
        private readonly IBusinessVerificationRepository _businessVerificationRepository;
        private readonly IArea _areaRepository;
        private readonly ICommon _commonRepository;
        private readonly IUser_Password_History _userPasswordHistory;
        private readonly ISysSetting _sysSetting;
        private readonly IAdminUser _adminUserRepository;
        private readonly IRoleVerification _roleVerificationRepository;
        private readonly ISys_Dictionary _sysDictionaryRespository;
        private readonly INotification _notificationService;
        private readonly IUser_Role _userRoleService;
        private readonly IMachineDemandTypeRL _machineDemandTypeService;
        public AccountController(IUser repository, IPublished_Demand fpdrepository, IFarmerVerficationInfoRepository farmerVerificationRepository,
            IOperatorInfoVerifciationRepository operatorRepository,
            IBusinessVerificationRepository businessRepository, IArea areaRepository,
            ICommon commonRepository, IUser_Password_History userPasswordHistory,
            ISysSetting sysSetting, IAdminUser adminUserRepository,
            IRoleVerification roleVerificationRepository, ISys_Dictionary sysDictionaryRespository,
            INotification notificationService, IQQUser qqUserRepository, IWeChatUser weChatUserRepository,
            IUser_Role userRoleService,
            IMachineDemandTypeRL machineDemandTypeService
            )
        {
            _userService = repository;
            _fpdrepository = fpdrepository;
            _farmerVerificationRepository = farmerVerificationRepository;
            _operatorRepository = operatorRepository;
            _businessVerificationRepository = businessRepository;
            _areaRepository = areaRepository;
            _commonRepository = commonRepository;
            _userPasswordHistory = userPasswordHistory;
            _sysSetting = sysSetting;
            _adminUserRepository = adminUserRepository;
            _roleVerificationRepository = roleVerificationRepository;
            _sysDictionaryRespository = sysDictionaryRespository;
            _notificationService = notificationService;
            _qqUserRepository = qqUserRepository;
            _weChatUserRepository = weChatUserRepository;
            _userRoleService = userRoleService;
            _machineDemandTypeService = machineDemandTypeService;
        }

        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="Password">密码</param>
        /// <param name="ValidateCode">手机验证码</param>
        /// <returns>返回结果</returns>
        [HttpPost]
        public JsonResult Register(RegisterInput input)
        {

            ResponseResult<RegisterModel> result = new ResponseResult<RegisterModel>();

            if (string.IsNullOrEmpty(input.PhoneNumber) || string.IsNullOrEmpty(input.Password))
            {
                result.IsSuccess = false;
                result.Message = "手机号码和密码不能为空！";
                return Json(result);
            }

            //if (!PageValidate.IsSafePassword(Password))
            //{
            //    result.IsSuccess = false;
            //    result.Message = "密码必须包含字母、数字、特殊符号,且字母包含大小写长度在(7-18)";
            //    return Json(result);
            //}

            var list = _userService.GetAll(c => c.LoginUserName == input.PhoneNumber);
            if (list.Count() > 0)
            {
                result.IsSuccess = false;
                result.Message = "当前手机号已经注册过，请选择登录！";
                return Json(result);
            }

            var user = new T_USER
            {
                PhoneNumber = input.PhoneNumber,
                Password = Encrypt.MD5Encrypt(input.Password),
                CreateTime = DateTime.Now,
                IsDeleted = false,
                LoginUserName = input.PhoneNumber,
                LastLoginTime = DateTime.Now,
                LastUpdatePwdTime = DateTime.Now
            };

            int rows = _userService.Insert(user);

            if (rows > 0)
            {

                // var user = _userService.GetAll(u => u.PhoneNumber == input.PhoneNumber).FirstOrDefault();
                int hisRows = _userPasswordHistory.Insert(new T_USER_PASSWORD_HISTORY
                {
                    UserID = user.Id,
                    Password = user.Password,
                    CreateTime = DateTime.Now
                });

                if (hisRows > 0)
                {
                    result.Message = "恭喜您，注册成功！";
                    user.LoginToken = Guid.NewGuid().ToString("N").ToUpper();
                    _userService.Update(user);
                    result.Entity.Token = user.LoginToken;
                    result.Entity.UserId = user.Id;
                }

            }
            else
            {
                result.IsSuccess = false;
                result.Message = "对不起，注册失败！";
            }
            return Json(result);
        }

        #endregion

        #region 用户登录
        #region "普通用户登录"
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="Password">登录密码</param>
        /// <returns>返回结果</returns>
        [HttpPost]
        public JsonResult Login(string PhoneNumber, string Password)
        {
            ResponseResult<LoginModel> result = new ResponseResult<LoginModel>();
            if (string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Password))
            {
                result.IsSuccess = false;
                result.Message = "手机号码和密码不能为空！";
                return Json(result);
            }
            Password = Encrypt.MD5Encrypt(Password);

            var list = _userService.GetAll(u => u.LoginUserName == PhoneNumber && u.Password == Password);
            if (list.Count() > 0)
            {
                T_USER user = list.ElementAt(0);
                user.LoginToken = Guid.NewGuid().ToString("N").ToUpper();
                int row = _userService.Update(user);
                if (row <= 0)
                {
                    result.Message = "登录验证更新失败！";
                    result.IsSuccess = false;
                }
                else if (user.IsDeleted)
                {
                    result.Message = ResponseStatusCode.UserIsLock.GetDescription();
                    result.IsSuccess = false;
                }
                else
                {
                    result.Message = "欢迎回来！";
                    result.Entity.Token = user.LoginToken;
                    result.Entity.UserId = user.Id;
                    result.Entity.PhoneNumber = user.PhoneNumber;
                    result.Entity.RealName = user.UserName ?? string.Empty;
                    result.Entity.Roles = _userService.GetRoles(user.Id);
                    //result.Entity.DetailAddress = user.DetailedAddress ?? string.Empty;
                    string addressCodesWithPipe = GetAreaCodePath(user);
                    string addressWithPipe = _areaRepository.GetAreaNamesBy(addressCodesWithPipe);
                    result.Entity.Address = string.Format("{0}|{1}|{2}|{3}|{4}", user.Province, user.City, user.Region, user.Township, user.Village);
                    result.Entity.DetailAddress = addressWithPipe + " " + (user.DetailedAddress ?? string.Empty);

                    //更新用户最新一次登录时间
                    user.LastLoginTime = DateTime.Now;
                    _userService.Update(user);
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "手机号或密码输入错误！";
            }
            return Json(result);
        }
        #endregion

        #region "第三方用户登录"
        [HttpPost]
        public JsonResult SocialLogin(string SocialId, string NickName, UserTypes type)
        {
            var result = new ResponseResult<LoginModel>();
            if (string.IsNullOrEmpty(SocialId))
            {
                result.IsSuccess = false;
                result.Message = "没有获取到第三方账号！";
                return Json(result);
            }

            //生成登录令牌
            var loginToken = Guid.NewGuid().ToString("N").ToUpper();
            var isRegistried = true;//是否是老用户
            var isRegisteriedSuccess = false;//是否注册成功
            var isLoginSuccess = false;//是否登录成功
            T_USER userInfo = null;

            switch (type)
            {
                case UserTypes.QQUser:
                    userInfo = _qqUserRepository.GetUserBy(SocialId);
                    if (userInfo == null)
                    {
                        isRegistried = false;
                        var newUser = new QQUser
                         {
                             PhoneNumber = string.Empty,
                             Password = string.Empty,
                             UserName = NickName,
                             CreateTime = DateTime.Now,
                             IsDeleted = false,
                             LoginUserName = string.Empty,
                             LastLoginTime = DateTime.Now,
                             LastUpdatePwdTime = DateTime.Now,
                             OpenId = SocialId,
                             LoginToken = loginToken,
                         };
                        isRegisteriedSuccess = _qqUserRepository.Insert(newUser) > 0;
                        userInfo = newUser;
                    }
                    break;
                case UserTypes.WeChatUser:
                    userInfo = _weChatUserRepository.GetUserBy(SocialId);
                    if (userInfo == null)
                    {
                        isRegistried = false;
                        var newUser = new WeChatUser
                        {
                            PhoneNumber = string.Empty,
                            Password = string.Empty,
                            UserName = NickName,
                            CreateTime = DateTime.Now,
                            IsDeleted = false,
                            LoginUserName = string.Empty,
                            LastLoginTime = DateTime.Now,
                            LastUpdatePwdTime = DateTime.Now,
                            UnionId = SocialId,
                            LoginToken = loginToken
                        };
                        isRegisteriedSuccess = _weChatUserRepository.Insert(newUser) > 0;
                        userInfo = newUser;
                    }
                    break;
                case UserTypes.PhoneUser:
                default:
                    result.IsSuccess = false;
                    result.Message = "指定的type参数值错误！";
                    return Json(result);
            }

            //已注册用户
            if (isRegistried)
            {
                if (userInfo.IsDeleted)
                {
                    result.Message = ResponseStatusCode.UserIsLock.GetDescription();
                    result.IsSuccess = false;
                }
                else //如果这里不加else，还会走下面两行代码，isLonginSuccess还是ture。会导致返回登录信息了，但IsSuccess是为false。
                {
                    userInfo.LoginToken = loginToken;
                    isLoginSuccess = _userService.Update(p => p.Id == userInfo.Id, t => new T_USER { LoginToken = loginToken, LastLoginTime = DateTime.Now }) > 0;
                }
            }

            //响应登录结果
            if (isRegisteriedSuccess || isLoginSuccess)
            {
                result.Message = "欢迎登录！";
                result.Entity.Token = userInfo.LoginToken;
                result.Entity.UserId = userInfo.Id;
                result.Entity.PhoneNumber = userInfo.PhoneNumber;
                result.Entity.RealName = userInfo.UserName ?? string.Empty;
                result.Entity.Roles = _userService.GetRoles(userInfo.Id);
                string addressCodesWithPipe = GetAreaCodePath(userInfo);
                string addressWithPipe = _areaRepository.GetAreaNamesBy(addressCodesWithPipe);
                result.Entity.Address = string.Format("{0}|{1}|{2}|{3}|{4}", userInfo.Province, userInfo.City, userInfo.Region, userInfo.Township, userInfo.Village);
                result.Entity.DetailAddress = addressWithPipe + " " + (userInfo.DetailedAddress ?? string.Empty);
            }
            else
            {
                result.Message = "登录失败!";
                result.IsSuccess = false;
            }

            return Json(result);
        }
        #endregion
        #endregion

        #region  角色认证
        /// <summary>
        /// 大农户角色认证
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="RealName">真实姓名</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <param name="DoPuntOrderNumbers">杜帮订单号</param>
        /// <param name="Land">共有土地</param>
        /// <param name="PurchasedProductsQuantity">已购先锋亩数</param>
        /// <returns>System.Web.Mvc.JsonResult.</returns>
        [HttpPost]
        public JsonResult RoleFarmerRegister(Int64 UserId, string PhoneNumber, string RealName, string Address, string DetailAddress, string DoPuntOrderNumbers, int Land, int PurchasedProductsQuantity)
        {
            var result = new ResponseResult<Object>();

            var user = _userService.GetByKey(UserId);
            if (user != null)
            {
                var dicAddress = StringHelper.GetAddress(Address);
                //user.UserName = RealName;
                //
                //user.DetailedAddress = DetailAddress;
                //repository.Update(user);
                var roles = this._userService.GetRoles(UserId);
                if (roles != null)
                {
                    int[] roleArray = roles.Select(r => r.Id).ToArray();
                    if (Array.IndexOf(roleArray, (int)RoleType.Business) > -1)
                    {
                        result.Message = "已经拥有产业商,不能申请大农户!";
                        result.IsSuccess = false;
                        return new JsonResultEx(result);
                    }
                    if (Array.IndexOf(roleArray, (int)RoleType.Farmer) > -1)
                    {
                        //TempData["Error"] = "已经拥有大农户角色!";
                        result.Message = "已经拥有大农户角色!";
                        result.IsSuccess = false;
                        //var model = new RoleStateViewModel();
                        //model.verificationId = verificationId;
                        //model.roleId = RoleType.Farmer;
                        return new JsonResultEx(result);
                    }
                }
                long waitAuditCount = _farmerVerificationRepository.Count(v => v.AuditState == 0 && v.UserId == UserId);
                if (waitAuditCount > 0)
                {
                    this._farmerVerificationRepository.Delete(f => f.UserId == UserId);
                }
                var entity = new T_FARMER_VERIFICATION_INFO()
                {
                    UserId = UserId,
                    DupontOrderNumbers = DoPuntOrderNumbers,
                    Land = Land,
                    PurchasedProducts = PurchasedProductsQuantity.ToString(),
                    CreateTime = DateTime.Now,
                    AuditState = 0,
                    Province = dicAddress["Province"],
                    City = dicAddress["City"],
                    Region = dicAddress["Region"],
                    Township = dicAddress["Township"],
                    Village = dicAddress["Village"],
                    RealName = RealName,
                    PhoneNumber = PhoneNumber,
                    DetailAddress = DetailAddress
                };
                _farmerVerificationRepository.Insert(entity);

                var bo = this._roleVerificationRepository.ApproveFarmerVerification(entity.Id, UserId, Convert.ToByte(ConfigHelper.GetAppSetting("FarmerLevel")));
                if (bo == true)
                {
                    result.Message = "同意了【" + RoleType.Farmer.GetDescription() + "】角色的认证!";
                    result.IsSuccess = true;
                    return Json(result);
                }
            }

            result.IsSuccess = false;
            return Json(result);
        }

        /// <summary>
        /// 农机手角色认证
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="RealName">真实姓名</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <param name="Machinery">拥有的农机</param>
        /// <param name="OtherMachinery">其它农机</param>
        /// <param name="PicturesIds">图片编号列表</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult RoleOperatorRegister(Int64 UserId, string PhoneNumber, string RealName, string Address,
            string DetailAddress, string Machinery, string OtherMachinery, string PicturesIds)
        {
            var result = new ResponseResult<Object>();

            if (Machinery.IsNullOrEmpty()) return ResponseErrorWithJson(result, "农机信息不可为空!");
            if (UserId <= 0) return ResponseErrorWithJson(result, "用户编号不可为空!");

            var user = _userService.GetByKey(UserId);
            if (user.IsNull()) return ResponseErrorWithJson(result, "用户不存在!");

            var dicAddress = StringHelper.GetAddress(Address);
            var roles = this._userService.GetRoles(UserId);
            if (roles != null)
            {
                int[] roleArray = roles.Select(r => r.Id).ToArray();
                if (Array.IndexOf(roleArray, (int)RoleType.Business) > -1)
                    return ResponseErrorWithJson(result, "已经拥有产业商,不能申请农机手!");

                if (Array.IndexOf(roleArray, (int)RoleType.MachineryOperator) > -1)
                    return ResponseErrorWithJson(result, "已经拥有农机手角色!");
            }

            long waitAuditCount = _operatorRepository.Count(v => v.AuditState == 0 && v.UserId == UserId);

            //删除所有待审核的记录
            if (waitAuditCount > 0) this._operatorRepository.Delete(f => f.UserId == UserId);

            //构建农机手角色申请信息实体
            var entity = new T_MACHINERY_OPERATOR_VERIFICATION_INFO()
            {
                UserId = UserId,
                Machinery = Machinery,
                OtherMachineDescription=OtherMachinery,
                PicturesIds = PicturesIds,
                CreateTime = DateTime.Now,
                AuditState = 0,
                Province = dicAddress["Province"],
                City = dicAddress["City"],
                Region = dicAddress["Region"],
                Township = dicAddress["Township"],
                Village = dicAddress["Village"],
                RealName = RealName,
                PhoneNumber = PhoneNumber,
                DetailAddress = DetailAddress
            };
            _operatorRepository.Insert(entity);            
            //根据农机信息获取对应的服务能力
            var machineList = JsonHelper.FromJsonTo<List<DuPont.Models.Models.ProductInfo>>(Machinery);
            var machineNameList = machineList.Select(p => p.Name).ToList();
            var machineInfoList = _sysDictionaryRespository.GetAll(p => machineNameList.Contains(p.DisplayName));

            if (machineInfoList.Count() == 0)
                return ResponseErrorWithJson(result, "农机数据错误!");
            var machineCodeList = machineInfoList.Select(p => p.Code).Distinct().ToList();

            var demandTypeIdList = _machineDemandTypeService.GetAll(p => machineCodeList.Contains(p.MachineId)).Select(p => p.DemandTypeId).Distinct().ToList();
            var userRoleDemandTypeLevelMapping = demandTypeIdList.ToDictionary(p => p, p => Convert.ToInt32(ConfigHelper.GetAppSetting("OperatorLevel")));

            var verificationSuccess = this._roleVerificationRepository.ApproveOperatorVerification(entity.Id, UserId, userRoleDemandTypeLevelMapping);
            if (verificationSuccess)
                return ResponseSuccessWithJson(result, "同意了【" + RoleType.MachineryOperator.GetDescription() + "】角色的认证!");

            return ResponseErrorWithJson(result, RoleType.MachineryOperator.GetDescription() + "认证失败!");
        }

        /// <summary>
        /// 产业商角色申请
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="PhoneNumber">手机号码</param>
        /// <param name="RealName">真实姓名</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <param name="PurchaseType">收购类型</param>
        /// <param name="Description">备注</param>
        /// <param name="PicturesIds">图片编号列表</param>
        /// <returns>JsonResult.</returns>
        public JsonResult RoleBusinessRegister(Int64 UserId, string PhoneNumber, string RealName, string Address,
            string DetailAddress, string PurchaseType, string Description, string PicturesIds)
        {
            var result = new ResponseResult<Object>();

            var user = _userService.GetByKey(UserId);
            if (user != null)
            {
                //user.UserName = RealName;
                var dicAddress = StringHelper.GetAddress(Address);

                var roles = this._userService.GetRoles(UserId);
                if (roles != null)
                {
                    int[] roleArray = roles.Select(r => r.Id).ToArray();
                    if (Array.IndexOf(roleArray, (int)RoleType.Farmer) > -1)
                    {
                        result.Message = "已经拥有大农户,不能申请产业商!";
                        result.IsSuccess = false;
                        return new JsonResultEx(result);
                    }
                    if (Array.IndexOf(roleArray, (int)RoleType.MachineryOperator) > -1)
                    {
                        result.Message = "已经拥有农机手,不能申请产业商!";
                        result.IsSuccess = false;
                        return new JsonResultEx(result);
                    }
                    if (Array.IndexOf(roleArray, (int)RoleType.Business) > -1)
                    {
                        result.Message = "已经拥有产业商角色!";
                        result.IsSuccess = false;
                        return new JsonResultEx(result);
                    }
                }
                //user.DetailedAddress = DetailAddress;
                //repository.Update(user);
                long waitAuditCount = _businessVerificationRepository.Count(v => v.AuditState == 0 && v.UserId == UserId);
                if (waitAuditCount > 0)
                {
                    this._businessVerificationRepository.Delete(f => f.UserId == UserId);
                }
                var entity = new T_BUSINESS_VERIFICATION_INFO()
                {
                    UserId = UserId,
                    PurchaseType = PurchaseType,
                    PictureIds = PicturesIds,
                    Introduction = Description,
                    CreateTime = DateTime.Now,
                    AuditState = 0,
                    Province = dicAddress["Province"],
                    City = dicAddress["City"],
                    Region = dicAddress["Region"],
                    Township = dicAddress["Township"],
                    Village = dicAddress["Village"],
                    RealName = RealName,
                    PhoneNumber = PhoneNumber,
                    DetailAddress = DetailAddress
                };
                _businessVerificationRepository.Insert(entity);
                var dics = PurchaseType.Split(',').ToDictionary(m => Convert.ToInt32(m), m => Convert.ToInt32(ConfigHelper.GetAppSetting("BusinessLevel")));//sysDictionaryRespository.GetAll(m => m.ParentCode == 100200).ToDictionary(m => m.Code, m => 1);
                if (dics.Count == 0)
                {
                    result.Message = "请至少选择一个已评星的技能";

                    result.IsSuccess = false;
                    return new JsonResultEx(result);
                }
                //var businessVerificationInfo = this._businessVerificationRepository.GetByKey(entity.Id);
                //if (businessVerificationInfo == null)
                //{
                //    res.Message = "记录不存在!";
                //    var model = new RoleStateViewModel();
                //    model.verificationId = verificationId;
                //    model.roleId = RoleType.Business;
                //    res.IsSuccess = false;
                //    res.Entity = model;
                //    return new JsonResultEx(res);
                //}

                var res = this._roleVerificationRepository.ApproveBusinessVerification(entity.Id, UserId, dics);
                if (res == true)
                {
                    result.Message = "同意了【" + RoleType.Business.GetDescription() + "】角色的认证!";
                    result.IsSuccess = true;
                    return Json(result);
                }
            }

            result.IsSuccess = false;
            return Json(result);
        }

        /// <summary>
        /// 获取用户所有角色申请待审核的角色编号列表
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <returns>JsonResult.</returns>
        public JsonResult RoleVerificaionInfo(Int64 userId)
        {
            using (var result = new ResponseResult<VerificationStatusInfo>())
            {

                var userInfo = this._userService.GetByKey(userId);
                if (userInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.UserNotExistsMessage;
                    return Json(result);
                }
                else if (userInfo.IsDeleted)
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.AccountWasLockedMessage;
                    return Json(result);
                }

                //获取待审核的角色列表
                List<T_ROLE> waitAuditRoleList = this._userService.GetWaitAuditRoleList(userId);

                result.IsSuccess = true;
                result.Entity.RoleIdList = waitAuditRoleList == null ? new int[0] : waitAuditRoleList.Select(role => role.Id).ToArray();
                return Json(result);
            }
        }
        #endregion  角色认证

        #region 用户登出（退出）

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="Token">用户登录Token值</param>
        /// <returns>返回结果</returns>
        [HttpPost]
        public JsonResult Logout(string userID, string encrypt, string Token)
        {

            ResponseResult<Object> result = new ResponseResult<Object>();

            var model = _userService.GetByKey(long.Parse(userID));
            if (model == null)
            {
                result.IsSuccess = false;
                result.Message = "用户不存在！";
            }
            else
            {
                model.IosDeviceToken = null;
                model.LoginToken = Guid.NewGuid().ToString("N").ToUpper();
                int row = _userService.Update(model);
                if (row > 0)
                {
                    result.Message = "退出成功！";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "请求退出失败！";
                }
            }

            return Json(result);
        }

        #endregion

        #region 我的发布
        /// <summary>
        /// 获取发布列表
        /// </summary>
        /// <param name="UserId">发布者id</param>
        /// <param name="IsClosed">发布状态：0表示进行中，1表示已关闭</param>
        /// <param name="RoleTyp">角色类别（根据角色类别读取相应表的数据）</param>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">一页显示的条数</param>
        /// <returns>返回发布列表集合</returns>
        [HttpPost]
        public JsonResult PublishedRequirement(long userId, int isClosed, int roleType, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex == 0 ? 1 : pageIndex;
            long TotalNums = 0;
            using (ResponseResult<List<PublishedModel>> result = new ResponseResult<List<PublishedModel>>())
            {

                //产业商
                if (roleType == (int)RoleType.Business)
                {
                    if (_commonRepository.CheckUserId(userId, (int)RoleType.Business))
                    {
                        List<PublishedModel> list = _fpdrepository.GetBusinessPublishedRequirement(userId, isClosed, roleType, pageIndex, pageSize, out  TotalNums);
                        if (list != null)
                        {
                            result.Entity = list;
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} 与{1}" + ResponeString.Mismatching, userId.ToString(), roleType.ToString());
                    }
                }
                //大农户
                else if (roleType == (int)RoleType.Farmer)
                {
                    if (_commonRepository.CheckUserId(userId, (int)RoleType.Farmer))
                    {
                        List<PublishedModel> list = _fpdrepository.GetFarmerPublishedRequirement(userId, isClosed, roleType, pageIndex, pageSize, out  TotalNums);
                        if (list != null)
                        {
                            result.Entity = list;
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = String.Format("{0} 与{1}" + ResponeString.Mismatching, userId.ToString(), roleType.ToString());
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = String.Format("{0} -" + ResponeString.ParmetersInvalidMessage, roleType.ToString());
                }

                result.TotalNums = TotalNums;
                result.PageIndex = pageIndex;
                result.PageSize = pageSize;
                return Json(result);
            }

        }
        #endregion

        #region "校验验证码"
        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="smscode"></param>
        /// <param name="phonenumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidCode(string smscode, string phonenumber)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                //测试模式时将起作用
                if (PageValidate.IsMobile(phonenumber) && ConfigurationManager.AppSettings["smsCodeSenderInTestMode"] == "1")
                {
                    result.Entity = true;
                    result.IsSuccess = true;
                    return Json(result);
                }

                if (!string.IsNullOrEmpty(phonenumber))
                {
                    //从数据库获取出当前手机号的验证码
                    T_SMS_MESSAGE model = _commonRepository.GetById<T_SMS_MESSAGE>(s => s.PhoneNumber == phonenumber);
                    if (model != null)
                    {
                        //验证验证码是否过期
                        TimeSpan ts = DateTime.Now - model.SendTime;
                        if (Convert.ToInt32(ts.TotalMinutes) < Convert.ToInt32(ConfigurationManager.AppSettings["smsValidMinutes"]))
                        {
                            string oldsmscode = model.Captcha;
                            //与传来的验证码作比较
                            if (smscode == oldsmscode)
                            {
                                result.Entity = true;
                                result.IsSuccess = true;
                            }
                            else
                            {
                                result.Entity = false;
                                result.IsSuccess = false;
                                result.Message = ResponeString.CodeError;
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = ResponeString.CodePastDue;
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = ResponeString.NoPhoneNumber;
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

        #region "修改密码"
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newpas"></param>
        /// <param name="phonenumber"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdPas(UpdatePasswordInput input)
        {
            using (var result = new ResponseResult<UpdatePasswordOutput>())
            {
                if (input.phonenumber.IsNullOrEmpty())
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.PhonenumberNotNull;
                    return new JsonResultEx(result);
                }

                var userInfo = _userService.GetByWhere(u => u.PhoneNumber == input.phonenumber);
                if (userInfo == null)
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.NoPhoneNumber;
                    return new JsonResultEx(result);
                }

                //设定新密码
                userInfo.Password = Encrypt.MD5Encrypt(input.newpas);
                //更新密码最近修改时间
                userInfo.LastUpdatePwdTime = DateTime.Now;
                _userService.Update(userInfo);

                result.Entity = Mapper.Map<UpdatePasswordOutput>(userInfo);
                result.IsSuccess = true;
                result.Message = "密码修改成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region 获取用户的个人信息
        /// <summary>
        /// 获取用户的个人信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="RoleId">角色编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public new JsonResult Profile(long UserId, int RoleId)
        {
            using (var result = new ResponseResult<DuPont.API.Models.Account.UserProfileModel>())
            {
                var user = _userService.GetByKey(UserId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.Message = "用户不存在!";
                    return new JsonResultEx(result);
                }

                var role = (Entity.Enum.RoleType)RoleId;

                if (user != null)
                {
                    string addressCodesWithPipe = GetAreaCodePath(user);
                    string addressWithPipe = _areaRepository.GetAreaNamesBy(addressCodesWithPipe);

                    result.Entity = new DuPont.API.Models.Account.UserProfileModel()
                    {
                        RealName = user.UserName ?? "",
                        PhoneNumber = user.PhoneNumber,
                        Address = addressWithPipe.Replace("|", "") + " " + user.DetailedAddress ?? string.Empty,
                        Credit = _userService.GetUserCredit(UserId, role),
                        DuPontMoney = user.DPoint ?? 0,
                        OriginalAddress = addressWithPipe,
                        OriginalAddressCode = addressCodesWithPipe,
                        UserType = user.Type
                    };
                }

                var current_Role_Level = new RoleBehavior()
                {
                    Name = role.GetDescription(),
                    Level = 0
                };


                //获取用户的角色关联信息
                var roleInfo = _userService.GetRoleRelationInfo(UserId);

                if (roleInfo != null && roleInfo.Count > 0)
                {
                    var currentRoleInfo = roleInfo.Where(r => r.RoleID == RoleId).FirstOrDefault();

                    if (role == Entity.Enum.RoleType.Farmer)
                    {
                        int level = 0;
                        if (currentRoleInfo != null)
                        {
                            level = (int)currentRoleInfo.Star;
                            current_Role_Level.Level = level;
                            result.Entity.Behaviors.Add(current_Role_Level);
                        }
                    }
                    else
                    {
                        if (currentRoleInfo != null)
                        {
                            //农机手或产业商
                            var roleLevels = _userService.GetUserRoleDemandData(UserId, RoleId);
                            if (roleLevels != null && roleLevels.Count > 0)
                            {
                                var topLevel = roleLevels.Max(m => m.Star);
                                current_Role_Level.Level = topLevel;
                                result.Entity.Level = topLevel;
                                result.Entity.Behaviors.Add(current_Role_Level);
                            }
                        }
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "角色受限!";
                    return new JsonResultEx(result);
                }

                long totalCount = 0;
                //用户的额外信息
                switch (role)
                {
                    case RoleType.Farmer:
                        //获取最近一次大农户角色验证信息
                        var farmerVerificationInfo = _farmerVerificationRepository.GetAll<DateTime>(p => p.UserId == UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                        if (farmerVerificationInfo != null)
                        {
                            var purchasedProducts = 0;
                            if (int.TryParse(farmerVerificationInfo.PurchasedProducts, out purchasedProducts))
                            {
                                result.Entity.PurchasedProductsQuantity = purchasedProducts;
                            }
                            result.Entity.Land = farmerVerificationInfo.Land ?? 0;
                        }
                        break;
                    case RoleType.MachineryOperator:
                        //获取最近一次农机手角色验证信息
                        var operatorVerificationInfo = _operatorRepository.GetAll<DateTime>(p => p.UserId == UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                        if (operatorVerificationInfo != null)
                        {
                            if (operatorVerificationInfo.Machinery.IsNullOrEmpty() == false && operatorVerificationInfo.Machinery.StartsWith("["))
                            {
                                result.Entity.Machinery = JsonHelper.FromJsonTo<List<DuPont.Models.Models.ProductInfo>>(operatorVerificationInfo.Machinery);
                            }
                            result.Entity.OtherMachinery = operatorVerificationInfo.OtherMachineDescription;
                        }
                        break;
                    case RoleType.Business:
                        //获取最近一次产业商角色验证信息
                        var businessVerificationInfo = _businessVerificationRepository.GetAll<DateTime>(p => p.UserId == UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                        if (businessVerificationInfo != null)
                        {
                            result.Entity.Description = businessVerificationInfo.Introduction ?? "";
                            var tempDicCodeList = businessVerificationInfo.PurchaseType.Replace("、", ",");
                            var dicCodeList = tempDicCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (dicCodeList.Length > 0)
                            {
                                var dicCodeIntArray = new List<int>();
                                for (int i = 0; i < dicCodeList.Length; i++)
                                {
                                    dicCodeIntArray.Add(int.Parse(dicCodeList[i]));
                                }
                                dicCodeIntArray = dicCodeIntArray.OrderBy(p => p).ToList();
                                result.Entity.PurchaseTypeCodes = string.Join(",", dicCodeList);
                                long tempTotalCount = 0;
                                result.Entity.PurchaseTypeNames = string.Join(",", _sysDictionaryRespository.GetAll<int>(
                                    p => dicCodeIntArray.Contains(p.Code),
                                    p => p.Code, null, 1, dicCodeIntArray.Count, out tempTotalCount)
                                    .Select(p => p.DisplayName).ToArray());
                            }
                        }
                        break;
                }
                return new JsonResultEx(result);
            }
        }
        #endregion

        private string GetAreaCodePath(T_USER user)
        {
            if (user == null)
            {
                return string.Empty;
            }

            string res = string.Empty;

            res = user.Province ?? string.Empty;
            res += "|" + user.City ?? string.Empty;
            res += "|" + user.Region ?? string.Empty;
            res += "|" + user.Township ?? string.Empty;
            res += "|" + user.Village ?? string.Empty;

            return res;
        }

        #region 修改用户的个人信息
        /// <summary>
        /// 修改用户个人信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Name">用户名称</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult SaveProfile(SaveProfileInput input)
        {
            using (var result = new ResponseResult<UserProfileModel>())
            {

                var user = _userService.GetByKey(input.UserId);
                if (user != null)
                {
                    user.DetailedAddress = input.DetailAddress;
                    user.UserName = input.Name;
                    var addresses = input.Address.Split('|');
                    if (addresses.Count() > 0)
                    {
                        user.Province = addresses[0];
                    }
                    if (addresses.Count() > 1)
                    {
                        user.City = addresses[1];
                    }
                    if (addresses.Count() > 2)
                    {
                        user.Region = addresses[2];
                    }
                    if (addresses.Count() > 3)
                    {
                        user.Township = addresses[3];
                    }
                    if (addresses.Count() > 4)
                    {
                        user.Village = addresses[4];
                    }

                    var currentUserType = (UserTypes)user.Type;
                    if ((currentUserType == UserTypes.QQUser || currentUserType == UserTypes.WeChatUser) && input.PhoneNumber.IsNullOrEmpty() == false)
                    {
                        user.PhoneNumber = input.PhoneNumber;
                    }

                    _userService.Update(user);

                    long totalCount = 0;
                    //用户的额外信息更新
                    switch (input.RoleType)
                    {
                        case RoleType.Farmer:
                            //获取最近一次大农户角色验证信息
                            var farmerVerificationInfo = _farmerVerificationRepository.GetAll<DateTime>(p => p.UserId == input.UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                            if (farmerVerificationInfo != null)
                            {
                                farmerVerificationInfo.PurchasedProducts = "" + input.PurchasedProductsQuantity;
                                farmerVerificationInfo.Land = input.Land;
                                _farmerVerificationRepository.Update(farmerVerificationInfo);
                            }
                            break;
                        case RoleType.MachineryOperator:
                            //获取最近一次农机手角色验证信息
                            var operatorVerificationInfo = _operatorRepository.GetAll<DateTime>(p => p.UserId == input.UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                            if (operatorVerificationInfo != null)
                            {
                                if (input.Machinery.IsNullOrEmpty() == false && input.Machinery.StartsWith("["))
                                {
                                    operatorVerificationInfo.Machinery = input.Machinery;
                                }
                                operatorVerificationInfo.OtherMachineDescription = input.OtherMachinery;
                                _operatorRepository.Update(operatorVerificationInfo);
                            }
                            //根据农机信息获取对应的服务能力
                            var machineList = JsonHelper.FromJsonTo<List<DuPont.Models.Models.ProductInfo>>(input.Machinery);
                            var machineNameList = machineList.Select(p => p.Name).ToList();
                            var machineInfoList = _sysDictionaryRespository.GetAll(p => machineNameList.Contains(p.DisplayName));

                            if (machineInfoList.Count() == 0)
                                return ResponseErrorWithJson(result, "农机数据错误!");
                            var machineCodeList = machineInfoList.Select(p => p.Code).Distinct().ToList();

                            var demandTypeIdList = _machineDemandTypeService.GetAll(p => machineCodeList.Contains(p.MachineId)).Select(p => p.DemandTypeId).Distinct().ToList();
                            var userRoleDemandTypeLevelMapping = demandTypeIdList.ToDictionary(p => p, p => Convert.ToInt32(ConfigHelper.GetAppSetting("OperatorLevel")));

                            var verificationSuccess = this._roleVerificationRepository.UpdateOperatorVerification(input.UserId, userRoleDemandTypeLevelMapping);                           
                            break;
                        case RoleType.Business:
                            //获取最近一次产业商角色验证信息
                            var businessVerificationInfo = _businessVerificationRepository.GetAll<DateTime>(p => p.UserId == input.UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                            if (businessVerificationInfo != null)
                            {
                                if (input.Description.IsNullOrEmpty() == false)
                                {
                                    businessVerificationInfo.Introduction = input.Description;
                                }
                                if (input.PurchaseType.IsNullOrEmpty() == false)
                                {
                                    businessVerificationInfo.PurchaseType = input.PurchaseType;
                                }
                                _businessVerificationRepository.Update(businessVerificationInfo);
                            }
                            break;
                    }
                }

                result.IsSuccess = true;
                return Json(result);
            }
        }
        #endregion

        #region 获取用户已审核的角色列表
        /// <summary>
        /// 获取用户已审核的角色列表
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult RoleList(Int64 UserId)
        {
            using (var result = new ResponseResult<List<T_ROLE>>())
            {
                var roles = this._userService.GetRoles(UserId);
                result.Entity = roles;
                result.IsSuccess = true;
                return Json(result);
            }
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserId">用户编号</param>      
        /// <author>ww</author>
        [HttpPost]
        public new JsonResult UserInfo(long UserId)
        {
            using (var result = new ResponseResult<DuPont.API.Models.Account.UserProfileModel>())
            {
                var user = _userService.GetByKey(UserId);
                //获取用户的角色关联信息
                var roleInfo = _userService.GetRoleRelationInfo(UserId);
                if (user == null)
                {
                    result.IsSuccess = false;
                    result.Message = "用户不存在!";
                    return new JsonResultEx(result);
                }

                if (roleInfo != null && roleInfo.Count > 0)
                {
                    //角色数量
                    var roleInfoCount = roleInfo.Count;
                    //星星总数
                    int sumlevel = 0;
                    for (int i = 0; i < roleInfoCount; i++)
                    {
                        var currentRoleInfo = roleInfo[i];
                        var role = (Entity.Enum.RoleType)currentRoleInfo.RoleID;
                        //初始化角色    
                        var current_Role_Level = new RoleBehavior()
                        {
                            Name = role.GetDescription(),
                            Level = 0
                        };

                        int level = 0;
                        if (currentRoleInfo != null)
                        {
                            level = (int)currentRoleInfo.Star;
                            sumlevel += level;
                            current_Role_Level.Level = level;
                            result.Entity.Level = sumlevel / roleInfoCount;//两个角色的星星总数/角色总数就是这个用户的平均星星数
                            result.Entity.Behaviors.Add(current_Role_Level);
                        }
                    }

                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "角色受限!";
                    return new JsonResultEx(result);
                }
                if (user != null)
                {
                    string addressCodesWithPipe = GetAreaCodePath(user);
                    string addressWithPipe = _areaRepository.GetAreaNamesBy(addressCodesWithPipe);
                    result.Entity.RealName = user.UserName ?? "";
                    result.Entity.PhoneNumber = user.PhoneNumber;
                    result.Entity.Address = addressWithPipe.Replace("|", "") + " " + user.DetailedAddress ?? string.Empty;
                    result.Entity.Credit = result.Entity.Level;//改成个人信息上方的星级
                    result.Entity.DuPontMoney = user.DPoint ?? 0;
                    result.Entity.OriginalAddress = addressWithPipe;
                    result.Entity.OriginalAddressCode = addressCodesWithPipe;
                    result.Entity.UserType = user.Type;
                }
                long totalCount = 0;
                //用户的额外信息

                //获取最近一次大农户角色验证信息
                var farmerVerificationInfo = _farmerVerificationRepository.GetAll<DateTime>(p => p.UserId == UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                if (farmerVerificationInfo != null)
                {
                    var purchasedProducts = 0;
                    if (int.TryParse(farmerVerificationInfo.PurchasedProducts, out purchasedProducts))
                    {
                        result.Entity.PurchasedProductsQuantity = purchasedProducts;
                    }
                    result.Entity.Land = farmerVerificationInfo.Land ?? 0;
                    result.Entity.LandAuditState = farmerVerificationInfo.LandAuditState;
                }

                //获取最近一次农机手角色验证信息
                var operatorVerificationInfo = _operatorRepository.GetAll<DateTime>(p => p.UserId == UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                if (operatorVerificationInfo != null)
                {
                    if (operatorVerificationInfo.Machinery.IsNullOrEmpty() == false && operatorVerificationInfo.Machinery.StartsWith("["))
                    {
                        result.Entity.Machinery = JsonHelper.FromJsonTo<List<DuPont.Models.Models.ProductInfo>>(operatorVerificationInfo.Machinery);
                    }
                    result.Entity.OtherMachinery = operatorVerificationInfo.OtherMachineDescription;
                }

                //获取最近一次产业商角色验证信息
                var businessVerificationInfo = _businessVerificationRepository.GetAll<DateTime>(p => p.UserId == UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                if (businessVerificationInfo != null)
                {
                    result.Entity.Description = businessVerificationInfo.Introduction ?? "";
                    var tempDicCodeList = businessVerificationInfo.PurchaseType.Replace("、", ",");
                    var dicCodeList = tempDicCodeList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (dicCodeList.Length > 0)
                    {
                        var dicCodeIntArray = new List<int>();
                        for (int i = 0; i < dicCodeList.Length; i++)
                        {
                            dicCodeIntArray.Add(int.Parse(dicCodeList[i]));
                        }
                        dicCodeIntArray = dicCodeIntArray.OrderBy(p => p).ToList();
                        result.Entity.PurchaseTypeCodes = string.Join(",", dicCodeList);
                        long tempTotalCount = 0;
                        result.Entity.PurchaseTypeNames = string.Join(",", _sysDictionaryRespository.GetAll<int>(
                            p => dicCodeIntArray.Contains(p.Code),
                            p => p.Code, null, 1, dicCodeIntArray.Count, out tempTotalCount)
                            .Select(p => p.DisplayName).ToArray());
                    }
                }
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region 修改用户信息
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="UserId">用户编号</param>
        /// <param name="Name">用户名称</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <author>ww</author>
        [HttpPost]
        public JsonResult SaveUserInfo(SaveProfileInput input)
        {
            using (var result = new ResponseResult<UserProfileModel>())
            {
                //用户编号
                long UserId = input.UserId;
                #region 基本信息
                var user = _userService.GetByKey(UserId);
                if (user != null)
                {
                    user.DetailedAddress = input.DetailAddress;
                    user.UserName = input.Name;
                    var addresses = input.Address.Split('|');
                    if (addresses.Count() > 0)
                    {
                        user.Province = addresses[0];
                    }
                    if (addresses.Count() > 1)
                    {
                        user.City = addresses[1];
                    }
                    if (addresses.Count() > 2)
                    {
                        user.Region = addresses[2];
                    }
                    if (addresses.Count() > 3)
                    {
                        user.Township = addresses[3];
                    }
                    if (addresses.Count() > 4)
                    {
                        user.Village = addresses[4];
                    }

                    var currentUserType = (UserTypes)user.Type;
                    if ((currentUserType == UserTypes.QQUser || currentUserType == UserTypes.WeChatUser) && input.PhoneNumber.IsNullOrEmpty() == false)
                    {
                        user.PhoneNumber = input.PhoneNumber;
                    }

                    _userService.Update(user);
                }
                #endregion
                #region 额外信息
                //获取用户的角色关联信息
                var roleInfo = _userService.GetRoleRelationInfo(UserId);
                if (roleInfo != null)
                {
                    long totalCount = 0;
                    if (roleInfo.Where(x => x.RoleID == 3 || x.RoleID == 4).Count() > 0)
                    {
                        //获取最近一次大农户角色验证信息
                        var farmerVerificationInfo = _farmerVerificationRepository.GetAll<DateTime>(p => p.UserId == input.UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                        if (farmerVerificationInfo != null)
                        {
                            farmerVerificationInfo.PurchasedProducts = "" + input.PurchasedProductsQuantity;
                            farmerVerificationInfo.Land = input.Land;
                            _farmerVerificationRepository.Update(farmerVerificationInfo);
                        }
                        //获取最近一次农机手角色验证信息
                        var operatorVerificationInfo = _operatorRepository.GetAll<DateTime>(p => p.UserId == input.UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                        if (operatorVerificationInfo != null)
                        {
                            if (input.Machinery.IsNullOrEmpty() == false && input.Machinery.StartsWith("["))
                            {
                                operatorVerificationInfo.Machinery = input.Machinery;
                            }
                            operatorVerificationInfo.OtherMachineDescription = input.OtherMachinery;
                            _operatorRepository.Update(operatorVerificationInfo);
                            //根据农机信息获取对应的服务能力
                            var machineList = JsonHelper.FromJsonTo<List<DuPont.Models.Models.ProductInfo>>(input.Machinery);
                            var machineNameList = machineList.Select(p => p.Name).ToList();
                            var machineInfoList = _sysDictionaryRespository.GetAll(p => machineNameList.Contains(p.DisplayName));

                            if (machineInfoList.Count() == 0)
                                return ResponseErrorWithJson(result, "农机数据错误!");
                            var machineCodeList = machineInfoList.Select(p => p.Code).Distinct().ToList();

                            var demandTypeIdList = _machineDemandTypeService.GetAll(p => machineCodeList.Contains(p.MachineId)).Select(p => p.DemandTypeId).Distinct().ToList();
                            var userRoleDemandTypeLevelMapping = demandTypeIdList.ToDictionary(p => p, p => Convert.ToInt32(ConfigHelper.GetAppSetting("OperatorLevel")));

                            var verificationSuccess = this._roleVerificationRepository.UpdateOperatorVerification(input.UserId, userRoleDemandTypeLevelMapping);                           
                        }
                    }
                    else
                    {
                        //获取最近一次产业商角色验证信息
                        var businessVerificationInfo = _businessVerificationRepository.GetAll<DateTime>(p => p.UserId == input.UserId && p.AuditState == 1, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                        if (businessVerificationInfo != null)
                        {
                            if (input.Description.IsNullOrEmpty() == false)
                            {
                                businessVerificationInfo.Introduction = input.Description;
                            }
                            if (input.PurchaseType.IsNullOrEmpty() == false)
                            {
                                businessVerificationInfo.PurchaseType = input.PurchaseType;
                            }
                            _businessVerificationRepository.Update(businessVerificationInfo);
                        }
                    }
                    //角色是否大农户和农机手
                }//角色不为空
                #endregion//用户的额外信息更新
                result.IsSuccess = true;
                return Json(result);
            }

        }
        #endregion

        #region "E田登录换取Token"
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="Userid">手机号码</param>
        /// <param name="Password">登录密码</param>
        /// <returns>返回结果</returns>
        [HttpPost]
        public JsonResult GetToken(string Userid, string Password)
        {
            ResponseResult<TokenModel> result = new ResponseResult<TokenModel>();
            if (string.IsNullOrEmpty(Userid) || string.IsNullOrEmpty(Password))
            {
                result.IsSuccess = false;
                result.Message = "帐号和密码不能为空！";
                return Json(result);
            }
            long totalCount = 0;
            IUserToken _userTokenService = new DuPont.Repository.User_TokenRepository();
            var userinfo= _userTokenService.GetAll<DateTime>(p => p.UserName == Userid && p.Password==Password && p.IsDeleted == false, null,p=>Convert.ToDateTime(p.LastLoginTime), 1, 1, out totalCount).FirstOrDefault();
            
            if (userinfo != null)
            {
                userinfo.Token = Guid.NewGuid().ToString("N").ToUpper();
                userinfo.LastLoginTime = DateTime.Now;
                userinfo.ModifiedTime = DateTime.Now;
                int row = _userTokenService.Update(userinfo);
                if (row <= 0)
                {
                    result.Message = "登录验证更新失败！";
                    result.IsSuccess = false;
                }
                else if (userinfo.IsDeleted)
                {
                    result.Message = ResponseStatusCode.UserIsLock.GetDescription();
                    result.IsSuccess = false;
                }
                else
                {
                    result.Message = "欢迎登录！";
                    result.Entity.Token = userinfo.Token;
                    result.Entity.Userid = Userid;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "帐号号或密码输入错误！";
            }
            return Json(result);
        }
        #endregion

        #region 更新大农户订单状态
        /// <summary>
        /// 更新30天后，没评价的订单为完成的订单
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateFarmerRequirementState()
        {
            try
            {
                using (var result = new ResponseResult<object>())
                {
                    IFarmerRequirement _farmerRequirement = new DuPont.Repository.FarmerRequirementRepository();
                    List<FarmerDemand> farmerlist = new List<FarmerDemand>();
                    long TotalNums = 0;
                    //修改的记录数
                    int rows = 0;
                    farmerlist = _fpdrepository.GetFarmerDemandList(1, 9999, 0, out TotalNums);
                    DateTime nowtime = DateTime.Now;
                    DateTime dtThis = new DateTime(Convert.ToInt32(nowtime.Year), Convert.ToInt32(nowtime.Month), Convert.ToInt32(nowtime.Day));
                    for (int i = 0; i < farmerlist.Count; i++)
                    {
                        DateTime ReceiveDate = farmerlist[i].ReceiveDate;
                        DateTime dtLast = new DateTime(Convert.ToInt32(ReceiveDate.Year), Convert.ToInt32(ReceiveDate.Month), Convert.ToInt32(ReceiveDate.Day));
                        //接单时间到现在相隔的天数
                        int interval = new TimeSpan(dtThis.Ticks - dtLast.Ticks).Days;
                        //超期天数
                        string ExtendedTime = ConfigHelper.GetAppSetting("ExtendedTime");
                        if (interval > Convert.ToInt32(ExtendedTime))
                        {
                            var entityId = Convert.ToInt64(farmerlist[i].DemandId);
                            var entity = _farmerRequirement.GetByKey(entityId);
                            if (entity.PublishStateId == 100505)//系统关闭
                            {
                                break;
                            }
                            entity.PublishStateId = 100505;
                            entity.ModifiedTime = dtThis;
                            _farmerRequirement.Update(entity);
                            rows++;
                        }
                    }
                    result.Message = "执行成功";
                    if (rows >= 0)
                    {
                        result.IsSuccess = true;
                        result.TotalNums = rows;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.TotalNums = rows;
                    }
                    return new JsonResultEx(result);
                }
            }
            catch (Exception ex)
            {
                string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "UpdateFarmerRequirementState";
                IOHelper.WriteLogToFile(logErrstring+ ex.Message, HttpContext.Server.MapPath("~/App_Data/Log"));
                return Json(new { Message=ex.Message});
            }
        }
        #endregion

        #region 更新产业商订单状态
        /// <summary>
        /// 到订单日期后，状态变系统关闭
        /// </summary>
        /// <returns></returns>
        public JsonResult UpdateBusinessRequirementState()
        {
            try
            {
                using (var result = new ResponseResult<object>())
                {
                    IBusiness _businessRequirement = new DuPont.Repository.BusinessRepository();
                    List<PublishedModel> businesslist = new List<PublishedModel>();
                    long TotalNums = 0;
                    //修改的记录数
                    int rows = 0;
                    businesslist = _fpdrepository.GetBusinessDemandList(1,9999, 0, out TotalNums);
                    DateTime nowtime = DateTime.Now;
                    DateTime dtThis = new DateTime(Convert.ToInt32(nowtime.Year), Convert.ToInt32(nowtime.Month), Convert.ToInt32(nowtime.Day));
                    for (int i = 0; i < businesslist.Count; i++)
                    {
                        string ReceiveDate = null;
                        //判断是否到了期望日期
                        string tempdate = businesslist[i].Dates;
                        if (tempdate.Contains(',') || tempdate.Contains('-'))
                        {
                            string[] tempdates = tempdate.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (tempdates.Length > 1)
                            {                               
                                ReceiveDate = tempdates[tempdates.Length - 1].ToString();
                            }
                            else
                            {
                                ReceiveDate = tempdates[0].ToString();                               
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
                                ReceiveDate = Convert.ToDateTime(tempdates[tempdates.Length - 1].ToString()).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                ReceiveDate = Convert.ToDateTime(tempdates[0].ToString()).ToString("yyyy-MM-dd");                                
                            }
                        }
                        else
                        {
                            ReceiveDate = Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd");                            
                        }
                        if(ReceiveDate!=null)
                        {
                            //期望日期
                            DateTime dtLast = Convert.ToDateTime(ReceiveDate);
                            //期望日期到现在相隔的天数
                            int interval = new TimeSpan(dtThis.Ticks - dtLast.Ticks).Days;                           
                            if (interval >0)
                            {
                                var entityId = Convert.ToInt64(businesslist[i].Id);
                                var entity = _businessRequirement.GetByKey(entityId);
                                if (entity.PublishStateId == 100505)//系统关闭
                                {
                                    break;
                                }
                                entity.PublishStateId = 100505;
                                entity.ModifiedTime = dtThis;
                                _businessRequirement.Update(entity);
                                rows++;
                            }
                        }                     
                       
                    }
                    result.Message = "执行成功";
                    if (rows >= 0)
                    {
                        result.IsSuccess = true;
                        result.TotalNums = rows;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.TotalNums = rows;
                    }
                    return new JsonResultEx(result);
                }
            }
            catch (Exception ex)
            {
                string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "UpdateBusinessRequirementState";
                IOHelper.WriteLogToFile(logErrstring + ex.Message, HttpContext.Server.MapPath("~/App_Data/Log"));
                return Json(new { Message = ex.Message });
            }
        }
        #endregion
    }
}