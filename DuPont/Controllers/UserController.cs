// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-05-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-05-2015
// ***********************************************************************
// <copyright file="UserController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using AutoMapper;
using DuPont.Attributes;
using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Global.ActionResults;
using DuPont.Global.Exceptions;
using DuPont.Interface;
using DuPont.Models;
using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Models;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace DuPont.Controllers
{
    public class UserController : BaseController
    {
        private readonly IArea _areaRepository;
        private readonly IAdminUser _adminUserRepository;
        private readonly IUser _userService;
        private readonly ICommon _commonrepository;
        private readonly IExpertPermission _expertService;
        private readonly IOperatorInfoVerifciationRepository _operatorService;
        //ww
        private readonly IFarmerVerficationInfoRepository _farmerVerificationRepository;
        public UserController(
            IPermissionProvider permissionManage,
            IAdminUser adminUserRepository,
            IUser memberUserRepository,
            IArea areaRepository,
            ICommon commonrepository,
            IExpertPermission expertService,
            IOperatorInfoVerifciationRepository operatorService,
            IFarmerVerficationInfoRepository farmerVerificationRepository//ww
            )
            : base(permissionManage, adminUserRepository)
        {
            _adminUserRepository = adminUserRepository;
            _areaRepository = areaRepository;
            _commonrepository = commonrepository;
            _userService = memberUserRepository;
            _expertService = expertService;
            _operatorService = operatorService;
            _farmerVerificationRepository = farmerVerificationRepository;
        }

        //
        // GET: /User/
        public ActionResult Index()
        {
            //检查访问权限
            return GetCheckPermissionResult();
        }

        #region "获取注册会员列表"
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public JsonResult List(SearchUserListInputDto input)
        {
            //检查访问权限
            CheckPermission();

            using (ResponseResult<UserModel> result = new ResponseResult<UserModel>())
            {
                WhereModel wheremodel = new WhereModel()
                {
                    RoleId = Convert.ToInt32(input.RoleId),
                    Province = input.Province,
                    City = input.City,
                    Region = input.Region,
                    PhoneNumber = input.PhoneNumber,
                    StartTime = Convert.ToDateTime(input.StartTime),
                    EndTime = Convert.ToDateTime(input.EndTime),
                    UserTypeId = input.UserTypeId
                };
                long recordCount;
                var modelList = this._userService.GetUserList(input.pageIndex.Value, input.pageSize.Value, out recordCount, wheremodel);
                modelList.Select(m =>
                {
                    var addressNames = _areaRepository.GetAreaNamesBy(string.Format("{0}|{1}|{2}|{3}|{4}", m.Province, m.City, m.Region, m.Township, m.Village));
                    var addressNamesArray = addressNames.Split('|');
                    m.Province = addressNamesArray[0];
                    m.City = addressNamesArray[1];
                    m.Region = addressNamesArray[2];
                    m.Township = addressNamesArray[3];
                    m.Village = addressNamesArray[4];
                    return m;
                }).Count();
                var usersModel = new UserModel()
                {
                    Pager = new PagedList<string>(new string[0], input.pageIndex.Value, input.pageSize.Value, (int)recordCount),
                    PendingAuditList = modelList,
                    Wheremodel = wheremodel
                };
                result.IsSuccess = true;
                result.Entity = usersModel;
                result.TotalNums = recordCount;
                result.PageIndex = input.pageIndex.Value;
                result.PageSize = input.pageSize.Value;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "导出用户列表为Excel"
        [HttpGet]
        public ActionResult ExportExcelWithUserList(SearchUserListInputDto input)
        {
            CheckPermission();

            WhereModel wheremodel = new WhereModel()
            {
                RoleId = Convert.ToInt32(input.RoleId),
                Province = input.Province,
                City = input.City,
                Region = input.Region,
                PhoneNumber = input.PhoneNumber,
                StartTime = Convert.ToDateTime(input.StartTime),
                EndTime = Convert.ToDateTime(input.EndTime)
            };

            long recordCount;
            var modelList = this._userService.GetUserList(input.pageIndex.Value, input.pageSize.Value, out recordCount, wheremodel);
            var usersModel = new UserModel()
            {
                Pager = new PagedList<string>(new string[0], input.pageIndex.Value, input.pageSize.Value, (int)recordCount),
                PendingAuditList = modelList,
                Wheremodel = wheremodel
            };

            if (modelList != null && modelList.Count > 0)
            {
                var areaCodeDictionary = new Dictionary<string, string>();
                modelList.Select(m =>
                {
                    if (ValidatorAreaCode(m.Province) && !areaCodeDictionary.ContainsKey(m.Province))
                        areaCodeDictionary.Add(m.Province, string.Empty);

                    if (ValidatorAreaCode(m.City) && !areaCodeDictionary.ContainsKey(m.City))
                        areaCodeDictionary.Add(m.City, string.Empty);

                    if (ValidatorAreaCode(m.Region) && !areaCodeDictionary.ContainsKey(m.Region))
                        areaCodeDictionary.Add(m.Region, string.Empty);

                    if (ValidatorAreaCode(m.Township) && !areaCodeDictionary.ContainsKey(m.Township))
                        areaCodeDictionary.Add(m.Township, string.Empty);

                    if (ValidatorAreaCode(m.Village) && !areaCodeDictionary.ContainsKey(m.Village))
                        areaCodeDictionary.Add(m.Village, string.Empty);

                    return m;
                }).Count();
                var areaCodeList = areaCodeDictionary.Keys.ToList();
                var areaInfoList = _areaRepository.GetAll(p => areaCodeList.Contains(p.AID));

                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                // 创建一张工作薄。
                var workSheet = workbook.CreateSheet("注册会员列表");
                var headerRow = workSheet.CreateRow(0);
                var tableHeaderTexts = new string[] { "用户编号", "姓名", "电话", "省", "市", "区/县", "乡镇", "村", "亩数","用户类型", "注册时间" };
                ICellStyle style = workbook.CreateCellStyle();
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style.FillPattern = FillPattern.SolidForeground;


                //生成列头
                for (int i = 0; i < tableHeaderTexts.Length; i++)
                {
                    var currentCell = headerRow.CreateCell(i);

                    currentCell.SetCellValue(tableHeaderTexts[i]);
                    currentCell.CellStyle = style;
                }

                var currentRoeIndex = 0;
                //给用地区信息赋上说明
                foreach (var user in modelList)
                {
                    currentRoeIndex++;
                    var provinceInfo = areaInfoList.FirstOrDefault(p => p.AID == user.Province);
                    var cityInfo = areaInfoList.FirstOrDefault(p => p.AID == user.City);
                    var regionInfo = areaInfoList.FirstOrDefault(p => p.AID == user.Region);
                    var townshipInfo = areaInfoList.FirstOrDefault(p => p.AID == user.Township);
                    var villageInfo = areaInfoList.FirstOrDefault(p => p.AID == user.Village);
                    
                    if (provinceInfo != null)
                        user.Province = provinceInfo.DisplayName;
                    else
                        user.Province = string.Empty;

                    if (cityInfo != null)
                        user.City = cityInfo.DisplayName;
                    else
                        user.City = string.Empty;

                    if (regionInfo != null)
                        user.Region = regionInfo.DisplayName;
                    else
                        user.Region = string.Empty;

                    if (townshipInfo != null)
                        user.Township = townshipInfo.DisplayName;
                    else
                        user.Township = string.Empty;

                    if (villageInfo != null)
                        user.Village = villageInfo.DisplayName;
                    else
                        user.Village = string.Empty;

                    var dataRow = workSheet.CreateRow(currentRoeIndex);
                    var userIdCell = dataRow.CreateCell(0);
                    var userNameCell = dataRow.CreateCell(1);
                    var phoneNumberCell = dataRow.CreateCell(2);
                    var provinceCell = dataRow.CreateCell(3);
                    var cityCell = dataRow.CreateCell(4);
                    var regionCell = dataRow.CreateCell(5);
                    var townshipCell = dataRow.CreateCell(6);
                    var villageCell = dataRow.CreateCell(7);
                    var landCell = dataRow.CreateCell(8);
                    var userTypeCell = dataRow.CreateCell(9);
                    var registryTimeCell = dataRow.CreateCell(10);

                    userIdCell.SetCellValue(user.UserId);
                    userNameCell.SetCellValue(user.UserName);
                    phoneNumberCell.SetCellValue(user.PhoneNumber);
                    provinceCell.SetCellValue(user.Province);
                    cityCell.SetCellValue(user.City);
                    regionCell.SetCellValue(user.Region);
                    townshipCell.SetCellValue(user.Township);
                    villageCell.SetCellValue(user.Village);
                    landCell.SetCellValue(Convert.ToString(user.Land));
                    userTypeCell.SetCellValue(((UserTypes)user.Type).GetDescription());
                    registryTimeCell.SetCellValue(user.CreateTime.ToString("yyyy.MM.dd"));
                }

                workbook.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=MemberUserList" + (DateTime.Now.ToString("yyyyMMddHHmmss")) + ".xls"));
                Response.BinaryWrite(ms.ToArray()); workbook = null;
                return File(ms, "application/ms-excel");
            }
            else
            {
                return Content("no data");
            }
        }
        #endregion

        #region "用户列表/用户详情"
        /// <summary>
        /// 获取要修改的数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUpdData(int id = 0)
        {
            using (var result = new ResponseResult<UserInfoModel>())
            {

                if (id == 0)
                {
                    result.IsSuccess = false;
                    result.Message = string.Format("{0}-" + ResponeString.InvalidParameter, id);
                    return Json(result);
                }
                //1获取所有的角色类别对应的的技能
                var skillinfo = this._userService.GetSkill().ToList();
                //2根据userid获取到该用户的基本信息
                var user = this._userService.GetByKey(id);

                //3根据userid获取该用户的技能以及每个技能的星星数
                var oneuserskillinfo = this._userService.GetOneUserSkill(id).ToList();
                //4获取该用户的角色类别列表
                var rolelist = this._userService.GetRoleList(id).OrderBy(x => x.RoleId).ToList();//ww按roleid从小到大排序
                //5将所有model数据合成一个model返回给视图
                var vm_userInfo = new UserInfoModel()
                {
                    SkillInfo = skillinfo,
                    OneUserSkillInfo = oneuserskillinfo,
                    RoleList = rolelist,
                };

                long totalCount;
                //查询亩数ww
                var farmerVerificationInfo = _farmerVerificationRepository.GetAll<DateTime>(p => p.UserId == id, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                if (farmerVerificationInfo != null)
                {
                    vm_userInfo.Land = farmerVerificationInfo.Land;
                    vm_userInfo.LandAuditState = farmerVerificationInfo.LandAuditState;
                }
                var machineList = _operatorService.GetAll<DateTime>(p => p.UserId == id, null, p => p.CreateTime, 1, 1, out totalCount);
                if (machineList.Count() > 0)
                {
                    var machinery = machineList.ElementAt(0).Machinery;
                    if (machinery.StartsWith("["))
                    {
                        try
                        {
                            vm_userInfo.MachineList = JsonHelper.FromJsonTo<List<DuPont.Models.Models.ProductInfo>>(machinery);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                vm_userInfo.Id = user.Id;
                vm_userInfo.UserName = user.UserName;
                vm_userInfo.DPoint = user.DPoint;
                vm_userInfo.Province = user.Province;
                vm_userInfo.PhoneNumber = user.PhoneNumber;
                vm_userInfo.City = user.City;
                vm_userInfo.Region = user.Region;
                vm_userInfo.Township = user.Township;
                vm_userInfo.Village = user.Village;

                result.IsSuccess = true;
                result.Entity = vm_userInfo;

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "修改用户的基本信息"
        /// <summary>
        /// 修改用户的基本信息（后台修改用户信息）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdUserInfo(string UserId, Dictionary<int, byte?> demandLevelInfoList)
        {
            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                //2017-1-17修改UserId类型，
                var userInfo = _userService.GetByKey(Convert.ToInt32(UserId));
                T_USER model = new T_USER()
                {
                    Id = Convert.ToInt64(Request.Form["Id"]),
                    UserName = Request.Form["UserName"].ToString(),
                    PhoneNumber = userInfo.PhoneNumber,
                    DPoint = string.IsNullOrEmpty(Request.Form["DPoint"]) ? null : (int?)Convert.ToInt32(Request.Form["DPoint"]),
                    Province = Request.Form["Province"].ToString(),
                    City = Request.Form["City"].ToString(),
                    Region = Request.Form["Region"].ToString(),
                    Township = Request.Form["Township"].ToString(),
                    Village = Request.Form["Village"].ToString()
                };
                if (!string.IsNullOrEmpty(UserId))
                {
                    this._userService.UpdateUserInfo(model, demandLevelInfoList, Convert.ToInt64(UserId));
                    result.IsSuccess = true;
                    result.Message = ResponeString.UpdateOK;
                    result.Entity = 1;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.NotLogin;
                }

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region UpdateLand
        /// <summary>
        /// 认证亩数
        /// </summary>
        /// <param name="Land">土地亩数</param>
        /// <param name="Id">用户编号</param>
        /// <author>ww</author>
        public JsonResult UpdateLand(string Land, string userId, long Id = 0)
        {            
            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                //查询亩数ww
                long totalCount;
                var farmerVerificationInfo = _farmerVerificationRepository.GetAll<DateTime>(p => p.UserId == Id, null, p => p.CreateTime, 1, 1, out totalCount).FirstOrDefault();
                if (farmerVerificationInfo != null)
                {
                    if (!string.IsNullOrWhiteSpace(Land))
                    {
                        farmerVerificationInfo.Land = Convert.ToInt32(Land);
                        farmerVerificationInfo.LandAuditState = 1;//0 初始值，1土地亩数审核通过
                        farmerVerificationInfo.AuditUserId = Convert.ToInt64(userId);
                        farmerVerificationInfo.AuditTime = DateTime.Now;
                    }
                }
                //修改角色认证表数据
                if (!string.IsNullOrEmpty(Id.ToString()))
                {
                    this._farmerVerificationRepository.Update(farmerVerificationInfo);
                    result.IsSuccess = true;
                    result.Message = ResponeString.UpdateOK;
                    result.Entity = 1;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.NotLogin;
                }

                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "添加经销商"
        /// <summary>
        /// 添加user
        /// </summary>
        /// <returns></returns>
        public JsonResult AddUser()
        {
            //检查访问权限
            CheckPermission();

            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                string loginUserName = Request.Form["LoginUserName"].ToString();

                //检查手机号是否存在
                if (this._adminUserRepository.Count(u => u.UserName == loginUserName) > 0)
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.LoginUserNameExist;
                    return Json(result);
                }

                //创建一个后端用户
                var dealerUser = new T_ADMIN_USER()
                {
                    RealName = Request.Form["UserName"].ToString(),
                    UserName = Request.Form["LoginUserName"],
                    Password = Encrypt.MD5Encrypt(Request.Form["Password"].ToString()),
                    Province = Request.Form["Province"].ToString(),
                    City = Request.Form["City"].ToString(),
                    Region = Request.Form["Region"].ToString(),
                    Township = Request.Form["Township"].ToString(),
                    Village = Request.Form["Village"].ToString(),
                    CreateTime = Utility.TimeHelper.GetChinaLocalTime(),
                };

                //生成用户角色关联信息
                var rolemodel = new T_USER_ROLE_RELATION()
                {
                    UserID = 0,
                    RoleID = (int)RoleType.Dealer,
                    AuditUserId = Convert.ToInt32(Request.Form["AuditUserId"].DefaultIfEmpty("0")),
                    CreateTime = Utility.TimeHelper.GetChinaLocalTime(),
                    MemberType = false
                };

                if (this._adminUserRepository.CreateUser(dealerUser, rolemodel))
                {
                    result.IsSuccess = true;
                    result.Message = ResponeString.AddOk;
                    result.Entity = 1;

                    var lognet = new DP_Log()
                    {
                        CreateTime = DateTime.Now,
                        Message = "完成添加经销商" + rolemodel.UserID + "操作",
                        StackTrace = string.Empty
                    };
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.AddFaile;
                }

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "添加管理员"
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <returns></returns>
        public JsonResult AddManger()
        {
            //检查访问权限
            CheckPermission();

            using (ResponseResult<object> result = new ResponseResult<object>())
            {
                string loginUserName = Request.Form["LoginUserName"].ToString();

                //检查手机号是否存在
                if (this._adminUserRepository.Count(u => u.UserName == loginUserName) > 0)
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.LoginUserNameExist;
                    return Json(result);
                }
                var adminUser = new T_ADMIN_USER()
                {
                    RealName = Request.Form["UserName"].ToString(),
                    UserName = Request.Form["LoginUserName"],
                    Password = Encrypt.MD5Encrypt(Request.Form["Password"].ToString()),
                    Province = Request.Form["Province"].ToString(),
                    City = Request.Form["City"].ToString(),
                    Region = Request.Form["Region"].ToString(),
                    Township = Request.Form["Township"].ToString(),
                    Village = Request.Form["Village"].ToString(),
                    CreateTime = Utility.TimeHelper.GetChinaLocalTime(),
                };

                T_USER_ROLE_RELATION rolemodel = new T_USER_ROLE_RELATION()
                {
                    UserID = 1,
                    RoleID = Convert.ToInt32(Request.Form["Role"].DefaultIfEmpty("0")),
                    AuditUserId = Convert.ToInt32(Request.Form["AuditUserId"].DefaultIfEmpty("0")),
                    CreateTime = Utility.TimeHelper.GetChinaLocalTime(),
                    MemberType = false
                };
                if (this._adminUserRepository.CreateUser(adminUser, rolemodel))
                {
                    result.IsSuccess = true;
                    result.Message = ResponeString.AddOk;
                    result.Entity = 1;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.AddFaile;
                }

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "获取带专家标识的用户列表"
        public JsonResult ExpertList(SearchExpertListInput input)
        {
            //检查访问权限
            CheckPermission();
            using (var result = new ResponseResult<List<SearchExpertListOutput>>())
            {
                long totalCount;
                var userList = _userService.GetAll<DateTime>(input, null, m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount);

                var dicArea = new Dictionary<string, string>();
                //提取地区编号
                ExtractAreaIdListFromAddress(userList, dicArea);

                var areaIdArray = dicArea.Keys.ToArray();
                //获取地区信息
                var areaList = _areaRepository.GetAll(m => areaIdArray.Contains(m.AID))
                    .Select(m =>
                {
                    dicArea[m.AID] = m.DisplayName;
                    return m;
                }).Count();

                var userIdArray = userList.Select(m => m.Id).ToArray();

                result.Entity = Mapper.Map<List<SearchExpertListOutput>>(userList);
                //查询是专家的用户
                var expertList = _expertService.GetAll(m => userIdArray.Contains(m.UserId) && m.IsEnabled);
                foreach (var user in result.Entity)
                {
                    if (!string.IsNullOrEmpty(user.Province) && dicArea.ContainsKey(user.Province))
                        user.Province = dicArea[user.Province];

                    if (!string.IsNullOrEmpty(user.City) && dicArea.ContainsKey(user.City))
                        user.City = dicArea[user.City];

                    if (!string.IsNullOrEmpty(user.Region) && dicArea.ContainsKey(user.Region))
                        user.Region = dicArea[user.Region];

                    if (!string.IsNullOrEmpty(user.Township) && dicArea.ContainsKey(user.Township))
                        user.Township = dicArea[user.Township];

                    if (!string.IsNullOrEmpty(user.Village) && dicArea.ContainsKey(user.Village))
                        user.Village = dicArea[user.Village];

                    if (expertList.Any(m => m.UserId == user.UserId))
                        user.IsExpert = true;
                    else
                        user.IsExpert = false;
                }

                SetJosnResult<List<SearchExpertListOutput>>(result, input.PageIndex, input.PageSize,
                    totalCount, "获取用户列表成功(带专家标识)");
                return new JsonResultEx(result);
            }
        }

        private static void ExtractAreaIdListFromAddress(IEnumerable<T_USER> list, Dictionary<string, string> dicArea)
        {
            if (list == null) return;

            foreach (var user in list)
            {
                if (!string.IsNullOrEmpty(user.Province) && !dicArea.ContainsKey(user.Province))
                {
                    dicArea.Add(user.Province, string.Empty);
                }
                if (!string.IsNullOrEmpty(user.City) && !dicArea.ContainsKey(user.City))
                {
                    dicArea.Add(user.City, string.Empty);
                }
                if (!string.IsNullOrEmpty(user.Region) && !dicArea.ContainsKey(user.Region))
                {
                    dicArea.Add(user.Region, string.Empty);
                }
                if (!string.IsNullOrEmpty(user.Township) && !dicArea.ContainsKey(user.Township))
                {
                    dicArea.Add(user.Township, string.Empty);
                }
                if (!string.IsNullOrEmpty(user.Village) && !dicArea.ContainsKey(user.Village))
                {
                    dicArea.Add(user.Village, string.Empty);
                }
            }
        }
        #endregion

        #region "指定为专家"
        [HttpPost]
        public JsonResult GrantExpert(string userIds)
        {
            CheckPermission();
            if (string.IsNullOrEmpty(userIds))
                throw new CustomException("参数不能为空!");

            using (var result = new ResponseResult<object>())
            {
                //提取用户编号列表
                var ary_String_UserId = userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                long[] ary_Int_UserId = new long[ary_String_UserId.Length];
                for (int i = 0; i < ary_String_UserId.Length; i++)
                {
                    ary_Int_UserId[i] = long.Parse(ary_String_UserId[i]);
                }

                //获取是专家的用户
                var userListWithExpert = _expertService.GetAll(m => ary_Int_UserId.Contains(m.UserId));
                var userIdListWithExpert = userListWithExpert.Select(m => m.UserId).ToArray();

                //获取没有专家记录的用户
                var userListNotWithExpert = ary_Int_UserId.Where(m => !userIdListWithExpert.Contains(m)).ToArray();

                //有专家记录的作更新处理
                if (userIdListWithExpert.Count() > 0)
                    _expertService.GrantOrCancelExpert(true, userIdListWithExpert);

                //没有专家记录的作插入处理
                if (userListNotWithExpert.Count() > 0)
                {
                    var newExpertList = new List<T_EXPERT>();
                    userListNotWithExpert.Select(m =>
                    {
                        newExpertList.Add(new T_EXPERT
                        {
                            CreateTime = DateTime.Now,
                            LastModifiedTime = DateTime.Now,
                            IsEnabled = true,
                            UserId = m
                        });
                        return m;
                    }).Count();

                    _expertService.Insert(newExpertList);
                }

                result.Message = "授权专家成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "取消为专家"
        [HttpPost]
        public JsonResult CancelExpert(string userIds)
        {
            CheckPermission();
            if (string.IsNullOrEmpty(userIds))
                throw new CustomException("参数不能为空!");

            using (var result = new ResponseResult<object>())
            {
                var ary_String_UserId = userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                long[] ary_Int_UserId = new long[ary_String_UserId.Length];
                for (int i = 0; i < ary_String_UserId.Length; i++)
                {
                    ary_Int_UserId[i] = long.Parse(ary_String_UserId[i]);
                }
                result.Entity = _expertService.GrantOrCancelExpert(false, ary_Int_UserId);

                result.Message = "取消专家成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "注册用户账号状态管理"
        [HttpPost]
        public JsonResult StateManage(SearchExpertListInput input)
        {
            //检查访问权限
            CheckPermission();
            using (var result = new ResponseResult<List<SearchUserListWithStateOutput>>())
            {
                long totalCount;
                var userList = _userService.GetAll<DateTime>(input, null, m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount);

                var dicArea = new Dictionary<string, string>();
                //提取地区编号
                ExtractAreaIdListFromAddress(userList, dicArea);

                var areaIdArray = dicArea.Keys.ToArray();
                //获取地区信息
                var areaList = _areaRepository.GetAll(m => areaIdArray.Contains(m.AID))
                    .Select(m =>
                {
                    dicArea[m.AID] = m.DisplayName;
                    return m;
                }).Count();

                var userIdArray = userList.Select(m => m.Id).ToArray();

                result.Entity = Mapper.Map<List<SearchUserListWithStateOutput>>(userList);
                foreach (var user in result.Entity)
                {
                    if (!string.IsNullOrEmpty(user.Province) && dicArea.ContainsKey(user.Province))
                        user.Province = dicArea[user.Province];

                    if (!string.IsNullOrEmpty(user.City) && dicArea.ContainsKey(user.City))
                        user.City = dicArea[user.City];

                    if (!string.IsNullOrEmpty(user.Region) && dicArea.ContainsKey(user.Region))
                        user.Region = dicArea[user.Region];

                    if (!string.IsNullOrEmpty(user.Township) && dicArea.ContainsKey(user.Township))
                        user.Township = dicArea[user.Township];

                    if (!string.IsNullOrEmpty(user.Village) && dicArea.ContainsKey(user.Village))
                        user.Village = dicArea[user.Village];
                }

                SetJosnResult<List<SearchUserListWithStateOutput>>(result, input.PageIndex, input.PageSize,
                    totalCount, "获取用户列表成功(带锁定状态)");
                return new JsonResultEx(result);

            }
        }
        #endregion

        #region "前台账号锁定/解锁"
        [HttpPost]
        public JsonResult UpdateLockState(string userIds, bool isLock)
        {
            if (string.IsNullOrEmpty(userIds))
                throw new CustomException("参数不能为空!");

            using (var result = new ResponseResult<object>())
            {
                var ary_String_UserId = userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                long[] ary_Int_UserId = new long[ary_String_UserId.Length];
                for (int i = 0; i < ary_String_UserId.Length; i++)
                {
                    ary_Int_UserId[i] = long.Parse(ary_String_UserId[i]);
                }
                result.Entity = _userService.LockOrUnlock(isLock, ary_Int_UserId);

                result.Message = "用户" + (isLock ? "锁定" : "解锁") + "成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "后台用户列表"
        [HttpPost]
        public ActionResult BackgroundUserList(SearchBackgroundUserListInput input)
        {
            CheckPermission();
            using (var result = new ResponseResult<List<SearchBackgroundUserListOutput>>())
            {
                var predicate = PredicateBuilder.True<T_ADMIN_USER>();
                if (input.Province.IsNullOrEmpty() == false)
                    predicate = predicate.And(p => p.Province == input.Province);

                if (input.City.IsNullOrEmpty() == false)
                    predicate = predicate.And(p => p.City == input.City);

                if (input.Region.IsNullOrEmpty() == false)
                    predicate = predicate.And(p => p.Region == input.Region);

                if (input.RegStartTime.HasValue)
                    predicate = predicate.And(p => p.CreateTime >= input.RegStartTime.Value);

                if (input.RegEndTime.HasValue)
                    predicate = predicate.And(p => p.CreateTime <= input.RegEndTime.Value);

                if (input.IsLock.HasValue)
                    predicate = predicate.And(p => p.IsLock == input.IsLock.Value);

                if (input.UserName.IsNullOrEmpty() == false)
                    predicate = predicate.And(p => p.UserName == input.UserName);

                int totalCount = 0;
                var userList = _adminUserRepository.GetBackgroundUserList(predicate, input.RoleType, input.PageIndex.Value, input.PageSize.Value, out totalCount);
                var areaCodeList = GetBackgroundUserAreaCodeListBy(userList);
                IList<T_AREA> areaList = null;
                if (areaCodeList != null && areaCodeList.Count > 0)
                {
                    areaList = _areaRepository.GetAll(p => areaCodeList.Contains(p.AID));
                    var backgroundUserListOutput = Mapper.Map<List<SearchBackgroundUserListOutput>>(userList);
                    SetSearchBackgroundUserListAreaName(backgroundUserListOutput, areaList);
                    result.Entity = backgroundUserListOutput;
                }


                SetJosnResult<List<SearchBackgroundUserListOutput>>(result, input.PageIndex.Value, input.PageSize.Value, totalCount, "获取" + input.RoleType.GetDescription() + "列表成功!");

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "后台账号锁定/解锁"
        [HttpPost]
        public JsonResult UpdateBackgroundUserLockState(string userIds, bool isLock)
        {
            if (string.IsNullOrEmpty(userIds))
                throw new CustomException("参数不能为空!");

            using (var result = new ResponseResult<object>())
            {
                var ary_String_UserId = userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                long[] ary_Int_UserId = new long[ary_String_UserId.Length];
                for (int i = 0; i < ary_String_UserId.Length; i++)
                {
                    ary_Int_UserId[i] = long.Parse(ary_String_UserId[i]);
                }
                result.Entity = _adminUserRepository.LockOrUnlock(isLock, ary_Int_UserId);

                result.Message = "用户" + (isLock ? "锁定" : "解锁") + "成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "导出后台用户列表为Excel"
        [HttpGet]
        public ActionResult ExportExcelWithBackgroundUserList(SearchBackgroundUserListInput input)
        {
            CheckPermission();
            if (input.PageSize.Value > 10000)
            {
                input.PageSize = 10000;
            }
            var predicate = PredicateBuilder.True<T_ADMIN_USER>();
            if (input.Province.IsNullOrEmpty() == false)
                predicate = predicate.And(p => p.Province == input.Province);

            if (input.City.IsNullOrEmpty() == false)
                predicate = predicate.And(p => p.City == input.City);

            if (input.Region.IsNullOrEmpty() == false)
                predicate = predicate.And(p => p.Region == input.Region);

            if (input.RegStartTime.HasValue)
                predicate = predicate.And(p => p.CreateTime >= input.RegStartTime.Value);

            if (input.RegEndTime.HasValue)
                predicate = predicate.And(p => p.CreateTime <= input.RegEndTime.Value);

            if (input.IsLock.HasValue)
                predicate = predicate.And(p => p.IsLock == input.IsLock.Value);

            if (input.UserName.IsNullOrEmpty() == false)
                predicate = predicate.And(p => p.UserName == input.UserName);

            int totalCount = 0;
            var userList = _adminUserRepository.GetBackgroundUserList(predicate, input.RoleType, input.PageIndex.Value, input.PageSize.Value, out totalCount);
            var areaCodeList = GetBackgroundUserAreaCodeListBy(userList);
            IList<T_AREA> areaList = null;
            List<SearchBackgroundUserListOutput> backgroundUserListOutput = null;
            if (areaCodeList != null && areaCodeList.Count > 0)
            {
                areaList = _areaRepository.GetAll(p => areaCodeList.Contains(p.AID));
                backgroundUserListOutput = Mapper.Map<List<SearchBackgroundUserListOutput>>(userList);
                SetSearchBackgroundUserListAreaName(backgroundUserListOutput, areaList);
            }

            if (userList.Count > 0)
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                // 创建一张工作薄。
                var workSheet = workbook.CreateSheet("注册会员列表");
                var headerRow = workSheet.CreateRow(0);
                var tableHeaderTexts = new string[] { "用户编号", "角色", "真实姓名", "登录账号名", "省", "市", "区/县", "注册时间", "账户状态" };
                ICellStyle style = workbook.CreateCellStyle();
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style.FillPattern = FillPattern.SolidForeground;


                //生成列头
                for (int i = 0; i < tableHeaderTexts.Length; i++)
                {
                    var currentCell = headerRow.CreateCell(i);

                    currentCell.SetCellValue(tableHeaderTexts[i]);
                    currentCell.CellStyle = style;
                }

                var currentRoeIndex = 0;
                foreach (var bgUser in backgroundUserListOutput)
                {
                    currentRoeIndex++;
                    var dataRow = workSheet.CreateRow(currentRoeIndex);
                    var userIdCell = dataRow.CreateCell(0);
                    var roleCell = dataRow.CreateCell(1);
                    var userNameCell = dataRow.CreateCell(2);
                    var loginUserName = dataRow.CreateCell(3);
                    var provinceCell = dataRow.CreateCell(4);
                    var cityCell = dataRow.CreateCell(5);
                    var regionCell = dataRow.CreateCell(6);
                    var registryTimeCell = dataRow.CreateCell(7);
                    var accountStateCell = dataRow.CreateCell(8);

                    userIdCell.SetCellValue(bgUser.Id);
                    roleCell.SetCellValue(bgUser.RoleName);
                    userNameCell.SetCellValue(bgUser.RealName);
                    loginUserName.SetCellValue(bgUser.LoginUserName);
                    provinceCell.SetCellValue(bgUser.ProvinceName);
                    cityCell.SetCellValue(bgUser.CityName);
                    regionCell.SetCellValue(bgUser.RegionName);
                    registryTimeCell.SetCellValue(bgUser.RegisterTime.ToString("yyyy.MM.dd"));
                    accountStateCell.SetCellValue(bgUser.IsLocked ? "已锁定" : "正常");
                }

                workbook.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=BackgroundUserList" + (DateTime.Now.ToString("yyyyMMddHHmmss")) + ".xls"));
                Response.BinaryWrite(ms.ToArray()); workbook = null;
                return File(ms, "application/ms-excel");

            }
            else
            {
                return Content("no data");
            }
        }
        #endregion

        private static bool ValidatorAreaCode(string areaCode)
        {
            return areaCode.IsNullOrEmpty() == false && areaCode.Length > 3;
        }

        private static List<string> GetBackgroundUserAreaCodeListBy(List<BackgroundUserModel> userList)
        {
            if (userList == null || userList.Count == 0) return null;

            var areaCodeDictionary = new Dictionary<string, string>();
            foreach (var user in userList)
            {
                if (user.Province.IsNullOrEmpty() == false && !areaCodeDictionary.ContainsKey(user.Province))
                    areaCodeDictionary.Add(user.Province, string.Empty);

                if (user.City.IsNullOrEmpty() == false && !areaCodeDictionary.ContainsKey(user.City))
                    areaCodeDictionary.Add(user.City, string.Empty);

                if (user.Region.IsNullOrEmpty() == false && !areaCodeDictionary.ContainsKey(user.Region))
                    areaCodeDictionary.Add(user.Region, string.Empty);
            }

            return areaCodeDictionary.Keys.ToList();
        }

        private static void SetSearchBackgroundUserListAreaName(List<SearchBackgroundUserListOutput> userList, IList<T_AREA> areaList)
        {
            if (userList == null || userList.Count == 0 || areaList == null || areaList.Count == 0) return;

            foreach (var user in userList)
            {
                var provinceInfo = areaList.FirstOrDefault(p => p.AID == user.ProvinceCode);
                var cityInfo = areaList.FirstOrDefault(p => p.AID == user.CityCode);
                var regionInfo = areaList.FirstOrDefault(p => p.AID == user.RegionCode);

                if (provinceInfo != null)
                    user.ProvinceName = provinceInfo.DisplayName;

                if (cityInfo != null)
                    user.CityName = cityInfo.DisplayName;

                if (regionInfo != null)
                    user.RegionName = regionInfo.DisplayName;
            }
        }
    }
}