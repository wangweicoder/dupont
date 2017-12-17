// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-06-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-06-2015
// ***********************************************************************
// <copyright file="VerificationController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Interface;
using DuPont.Models;
using System.Linq;
using System.Web.Mvc;
using DuPont.Extensions;
using DuPont.Entity.Enum;
using Webdiyer.WebControls.Mvc;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using DuPont.Attributes;

using DuPont.Config;

using DuPont.Utility.LogModule.Model;
using DuPont.Filters;
using DuPont.Global.ActionResults;
using DuPont.Global.Exceptions;
using DuPont.Models.Models;
using AutoMapper;
namespace DuPont.Controllers
{
    [CustomHandleErrorWithErrorJson]
    public class VerificationController : BaseController
    {
        private IRoleVerification roleVerificationRepository;
        private IAdminUser adminUserRepository;
        private IFarmerVerficationInfoRepository farmerVerificationRepository;
        private IOperatorInfoVerifciationRepository machineryOperatorVerificationRepository;
        private IBusinessVerificationRepository businessVerificationRepository;
        private IArea areaRepository;
        private ISuppliers_Sarea suppliers_SareaRespository;
        private IFileInfoRepository fileInfoRepository;
        private ISys_Dictionary sysDictionaryRespository;
        public VerificationController(IPermissionProvider permissionManage,
            IRoleVerification roleVerificationRepository,
            IAdminUser adminUserRepository,
            IFarmerVerficationInfoRepository farmerVerificationRepository,
            IOperatorInfoVerifciationRepository machineryOperatorVerificationRepository,
            IBusinessVerificationRepository businessVerificationRepository,
            IArea areaRepository,
            IFileInfoRepository fileInfoRepository,
            ISys_Dictionary sysDictionaryRespository,
            ISuppliers_Sarea suppliers_SareaRespository, IUser user)
            : base(permissionManage, adminUserRepository)
        {
            this.suppliers_SareaRespository = suppliers_SareaRespository;
            this.roleVerificationRepository = roleVerificationRepository;
            this.adminUserRepository = adminUserRepository;
            this.farmerVerificationRepository = farmerVerificationRepository;
            this.machineryOperatorVerificationRepository = machineryOperatorVerificationRepository;
            this.businessVerificationRepository = businessVerificationRepository;
            this.areaRepository = areaRepository;
            this.fileInfoRepository = fileInfoRepository;
            this.sysDictionaryRespository = sysDictionaryRespository;
        }

        // GET: /Verification/
        /// <summary>
        /// Details the specified verification identifier.
        /// </summary>
        /// <param name="verificationId">The verification identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        public JsonResult Detail(Int64 UserId, int verificationId, RoleType roleId, string remoteApiUrl)
        {
            //检查访问权限
            CheckPermission();

            //非管理员时检查角色
            if (this.UserInfo.IsSuperAdmin == false)
            {
                var myRoleList = this.adminUserRepository.GetRoles(UserId);
                if (myRoleList != null && myRoleList.Count > 0)
                {
                    var isAdmin = Array.IndexOf(myRoleList.Select(role => role.RoleID).ToArray(), (int)RoleType.Admin) > -1;
                    if (!isAdmin)
                    {
                        var suppliers_SareaModel = suppliers_SareaRespository.GetManageArea(UserId);
                        var bo = false;
                        object viewModel = null;
                        if (suppliers_SareaModel != null && suppliers_SareaModel.Count > 0)//判断是否有管理区域
                        {
                            switch (roleId)
                            {
                                case RoleType.Farmer:
                                    var farmerVerificationInfo = this.farmerVerificationRepository.GetByKey(verificationId);
                                    viewModel = new FarmerVerificationInfoViewModel();
                                    ClassValueCopyHelper.Copy(viewModel, farmerVerificationInfo);
                                    bo = suppliers_SareaModel.Any(model =>
                                    {
                                        return farmerVerificationInfo.Province == model.AID
                                            || farmerVerificationInfo.City == model.AID
                                            || farmerVerificationInfo.Region == model.AID;
                                    });
                                    break;
                                case RoleType.MachineryOperator:
                                    var machineOperVerificationInfo = this.machineryOperatorVerificationRepository.GetByKey(verificationId);
                                    viewModel = new MachineOperatorVerificationInfoViewModel();
                                    ClassValueCopyHelper.Copy(viewModel, machineOperVerificationInfo);
                                    bo = suppliers_SareaModel.Any(model =>
                                    {
                                        return machineOperVerificationInfo.Province == model.AID
                                            || machineOperVerificationInfo.City == model.AID
                                            || machineOperVerificationInfo.Region == model.AID;
                                    });
                                    break;
                                case RoleType.Business:
                                    var businessVerificationInfo = this.businessVerificationRepository.GetByKey(verificationId);
                                    viewModel = new BusinessVerificationInfoViewModel();
                                    ClassValueCopyHelper.Copy(viewModel, businessVerificationInfo);
                                    bo = suppliers_SareaModel.Any(model =>
                                    {
                                        return businessVerificationInfo.Province == model.AID
                                            || businessVerificationInfo.City == model.AID
                                            || businessVerificationInfo.Region == model.AID;
                                    });
                                    break;
                            }
                        }
                        if (!bo)
                        {
                            var result = new ResponseResult<FarmerVerificationInfoViewModel>();
                            result.IsSuccess = false;
                            result.Message = "没有符合条件的数据";
                            return Json(result);
                        }
                    }

                }
            }


            var roleType = (RoleType)roleId;
            string viewName = string.Empty;
            switch (roleType)
            {
                case RoleType.Farmer:
                    viewName = "Farmer_Detail";

                    var result = new ResponseResult<FarmerVerificationInfoViewModel>();

                    var farmerVerificationInfo = this.farmerVerificationRepository.GetByKey(verificationId);
                    FarmerVerificationInfoViewModel viewModel = new FarmerVerificationInfoViewModel();
                    viewModel.RemoteApiUrl = remoteApiUrl;
                    ClassValueCopyHelper.Copy(viewModel, farmerVerificationInfo);
                    //获取用户的省市区域信息

                    string addressCodes = string.Format("{0}|{1}|{2}|{3}|{4}", farmerVerificationInfo.Province, farmerVerificationInfo.City, farmerVerificationInfo.Region, farmerVerificationInfo.Township, farmerVerificationInfo.Village);
                    var addressCodeNames = this.areaRepository.GetAreaNamesBy(addressCodes).Split('|');

                    viewModel.UserName = farmerVerificationInfo.RealName;
                    viewModel.PhoneNumber = farmerVerificationInfo.PhoneNumber;
                    viewModel.DetailAddress = farmerVerificationInfo.DetailAddress;
                    if (addressCodeNames != null && addressCodeNames.Length == 5)
                    {
                        viewModel.ProvinceName = addressCodeNames[0];
                        viewModel.CityName = addressCodeNames[1];
                        viewModel.RegionName = addressCodeNames[2];
                        viewModel.TownshipName = addressCodeNames[3];
                        viewModel.VillageName = addressCodeNames[4];
                    }
                    result.Entity = viewModel;
                    result.IsSuccess = true;

                    return new JsonResultEx(result);
                case RoleType.MachineryOperator:
                    viewName = "Machinery_Operator_Detail";

                    var mresult = new ResponseResult<MachineOperatorVerificationInfoViewModel>();

                    var machineryOperatorVerificationInfo = this.machineryOperatorVerificationRepository.GetByKey(verificationId);
                    var operatorViewModel = new MachineOperatorVerificationInfoViewModel();
                    operatorViewModel.RemoteApiUrl = remoteApiUrl;
                    ClassValueCopyHelper.Copy(operatorViewModel, machineryOperatorVerificationInfo);
                    //获得农机列表
                    if (string.IsNullOrEmpty(machineryOperatorVerificationInfo.Machinery) == false)
                    {

                        var machineList = JsonHelper.FromJsonTo<List<DuPont.Models.Models.ProductInfo>>(machineryOperatorVerificationInfo.Machinery);
                        operatorViewModel.MachineList = machineList;
                    }

                    //获取农机手提供的图片信息
                    if (string.IsNullOrEmpty(machineryOperatorVerificationInfo.PicturesIds) == false)
                    {
                        var picIdstrs = machineryOperatorVerificationInfo.PicturesIds.Split(',');
                        var picIds = new long[picIdstrs.Length];
                        int index = 0;
                        picIdstrs.Select(m =>
                        {
                            long fileId = 0;
                            if (long.TryParse(m, out fileId))
                            {
                                picIds[index++] = fileId;
                            }
                            return m;
                        }).Count();

                        var fileList = this.fileInfoRepository.GetAll(m => picIds.Contains(m.Id));
                        if (fileList != null)
                        {
                            operatorViewModel.Pictures = fileList.Select(m => m.Path).ToArray();
                        }
                    }
                    operatorViewModel.UserName = machineryOperatorVerificationInfo.RealName;
                    operatorViewModel.PhoneNumber = machineryOperatorVerificationInfo.PhoneNumber;
                    //获取省市区县信息
                    var addressNames = this.areaRepository.GetAreaNamesBy(string.Format("{0}|{1}|{2}|{3}|{4}", machineryOperatorVerificationInfo.Province, machineryOperatorVerificationInfo.City, machineryOperatorVerificationInfo.Region, machineryOperatorVerificationInfo.Township, machineryOperatorVerificationInfo.Village));
                    if (addressNames != null && addressNames != "")
                    {
                        var addrArray = addressNames.Split('|');
                        operatorViewModel.ProvinceName = addrArray[0];
                        operatorViewModel.CityName = addrArray[1];
                        operatorViewModel.RegionName = addrArray[2];
                        operatorViewModel.Township = addrArray[3];
                        operatorViewModel.Village = addrArray[4];
                    }
                    operatorViewModel.DetailAddress = machineryOperatorVerificationInfo.DetailAddress;
                    operatorViewModel.SkillList = this.sysDictionaryRespository.GetAll(m => m.ParentCode == 100100).ToList();

                    mresult.Entity = operatorViewModel;
                    mresult.IsSuccess = true;

                    return new JsonResultEx(mresult);
                case RoleType.Business:
                    viewName = "Business_Detail";

                    var bresult = new ResponseResult<BusinessVerificationInfoViewModel>();

                    var businessVerificationInfo = this.businessVerificationRepository.GetByKey(verificationId);
                    var businessViewModel = new BusinessVerificationInfoViewModel();
                    businessViewModel.RemoteApiUrl = remoteApiUrl;
                    ClassValueCopyHelper.Copy(businessViewModel, businessVerificationInfo);

                    //获取产业商提供的图片信息
                    if (string.IsNullOrEmpty(businessVerificationInfo.PictureIds) == false)
                    {
                        var picIdstrs = businessVerificationInfo.PictureIds.Split(',');
                        var picIds = new long[picIdstrs.Length];
                        int index = 0;
                        picIdstrs.Select(m =>
                        {
                            long fileId = 0;
                            if (long.TryParse(m, out fileId))
                            {
                                picIds[index++] = fileId;
                            }
                            return m;
                        }).Count();

                        var fileList = this.fileInfoRepository.GetAll(m => picIds.Contains(m.Id));
                        if (fileList != null && fileList.Count > 0)
                        {
                            businessViewModel.Pictures = fileList.Select(m => m.Path).ToArray();
                        }
                    }
                    businessViewModel.UserName = businessVerificationInfo.RealName;
                    businessViewModel.PhoneNumber = businessVerificationInfo.PhoneNumber;
                    //获取省市区县信息
                    addressNames = this.areaRepository.GetAreaNamesBy(string.Format("{0}|{1}|{2}|{3}|{4}", businessVerificationInfo.Province, businessVerificationInfo.City, businessVerificationInfo.Region, businessVerificationInfo.Township, businessVerificationInfo.Village));
                    if (addressNames != null && addressNames != "")
                    {
                        var addrArray = addressNames.Split('|');
                        businessViewModel.ProvinceName = addrArray[0];
                        businessViewModel.CityName = addrArray[1];
                        businessViewModel.RegionName = addrArray[2];
                        businessViewModel.TownshipName = addrArray[3];
                        businessViewModel.VillageName = addrArray[4];
                    }
                    businessViewModel.DetailAddress = businessVerificationInfo.DetailAddress;
                    businessViewModel.SkillList = this.sysDictionaryRespository.GetAll(m => m.ParentCode == 100200).ToList();

                    bresult.Entity = businessViewModel;
                    bresult.IsSuccess = true;

                    return new JsonResultEx(bresult);
            }
            return null;
        }

        public JsonResult List(Int64 userId, int pageIndex = 1, int pageSize = 10)
        {
            //检查访问权限
            CheckPermission();

            using (var result = new ResponseResult<RoleVerificationViewModel>())
            {
                //获取登录用户所管理地区信息
                var whereModel = ManagementRange(userId);

                long recordCount;

                var modelList = this.roleVerificationRepository.GetAll(pageIndex, pageSize, out recordCount, whereModel);

                modelList.Select(m =>
                {
                    var addressNames = this.areaRepository.GetAreaNamesBy(string.Format("{0}|{1}|{2}", m.Province, m.City, m.Region));
                    var addressNamesArray = addressNames.Split('|');
                    m.Province = addressNamesArray[0];
                    m.City = addressNamesArray[1];
                    m.Region = addressNamesArray[2];
                    return m;
                }).Count();
                var verificationViewModel = new RoleVerificationViewModel()
                {
                    Pager = new PagedList<string>(new string[0], pageIndex, pageSize, (int)recordCount),
                    PendingAuditList = modelList,
                };
                result.TotalNums = recordCount;
                result.PageIndex = pageIndex;
                result.PageSize = pageSize;
                result.IsSuccess = true;
                result.Entity = verificationViewModel;

                return new JsonResultEx(result);
            }
        }
        public WhereModel ManagementRange(Int64 userId)
        {

            var whereModel = new WhereModel()
            {
                RoleId = 0,
                PhoneNumber = string.Empty,
                Province = "0",
                City = "0",
                Region = "0",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now

            };

            var str_startTime = Request["StartTime"];
            var str_endTime = Request["EndTime"];
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            if (string.IsNullOrEmpty(str_startTime) || !DateTime.TryParse(str_startTime, out startTime))
            {
                startTime = DateTime.Now.AddMonths(-3);
            }

            if (string.IsNullOrEmpty(str_endTime) || !DateTime.TryParse(str_endTime, out endTime))
            {
                endTime = DateTime.Now;
            }

            whereModel.StartTime = startTime;
            whereModel.EndTime = endTime;


            if (this.UserInfo.IsSuperAdmin)
                return whereModel;//如果是超级管理员  直接 弹出，

            var UserId = userId;
            var roleList = adminUserRepository.GetRoles(UserId);
            if (roleList.Any(role => role.RoleID == (int)RoleType.Admin))
                return whereModel;//如果是管理员  直接 弹出，

            var bo = false;
            foreach (var role in roleList)
            {
                if (role.RoleID == (int)RoleType.Dealer) { bo = true; }

            }
            if (bo)
            {
                var suppliers_SareaModel = suppliers_SareaRespository.GetManageArea(UserId);
                var list = new List<string>();
                if (suppliers_SareaModel != null && suppliers_SareaModel.Count > 0)//判断是否有管理区域
                {

                    for (int i = 0; i < suppliers_SareaModel.Count; i++)
                    {
                        list.Add(suppliers_SareaModel[i].AID);
                    }
                }
                whereModel.SuppliersWhere = list;
                if (whereModel.SuppliersWhere.Count == 0)
                {
                    list.Add("aaa");
                }
                return whereModel;
            }

            return null;

        }
        /// <summary>
        /// 通过父类编号获取子类编号并序列化为json
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public JsonResult GetAreaChild(string ParentAId)
        {
            using (ResponseResult<List<AreaViewModel>> result = new ResponseResult<List<AreaViewModel>>())
            {
                //获取用户信息
                if (this.UserInfo.IsSuperAdmin)
                {
                    var areaChilds = areaRepository.GetAreaChilds(ParentAId);
                    result.Entity = areaChilds;
                    result.IsSuccess = true;
                    return new JsonResultEx(result);
                }

                //获得用户角色信息
                var roleList = this.adminUserRepository.GetRoles(UserId);
                //是否拥有管理员角色
                if (roleList.Any(role => role.RoleID == (int)RoleType.Admin))
                {
                    var areaChilds = areaRepository.GetAreaChilds(ParentAId);
                    result.Entity = areaChilds;
                    result.IsSuccess = true;
                    return new JsonResultEx(result);
                }

                //是否拥有经销商角色
                else if (roleList.Any(role => role.RoleID == (int)RoleType.Dealer))
                {
                    //获取经销商可管理的区域
                    var areaChilds = areaRepository.GetManageArea(ParentAId, UserId);
                    if (areaChilds == null || areaChilds.Count == 0)
                        return new JsonResultEx(result);
                    else
                    {
                        result.IsSuccess = true;
                        result.Entity = Mapper.Map<List<AreaViewModel>>(areaChilds);
                        return new JsonResultEx(result);
                    }
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        /// <summary>
        /// 根据检索条件查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Search(Int64 userId, WhereModel model, int pageIndex = 1, int pageSize = 10)
        {
            using (var result = new ResponseResult<RoleVerificationViewModelWithoutPager>())
            {

                var whereModel = ManagementRange(userId);
                model.SuppliersWhere = whereModel.SuppliersWhere;

                long recordCount = 0;

                var items = this.roleVerificationRepository.GetAll(pageIndex, pageSize, out recordCount, model);
                items.Select(m =>
                {
                    var addressNames = this.areaRepository.GetAreaNamesBy(string.Format("{0}|{1}|{2}", m.Province, m.City, m.Region));
                    var addressNamesArray = addressNames.Split('|');
                    m.Province = addressNamesArray[0];
                    m.City = addressNamesArray[1];
                    m.Region = addressNamesArray[2];
                    return m;
                }).Count();
                RoleVerificationViewModelWithoutPager verificationViewModel = null;
                if (items != null)
                {
                    verificationViewModel = new RoleVerificationViewModelWithoutPager()
                    {
                        PendingAuditList = items,
                    };
                    result.TotalNums = recordCount;
                    result.PageIndex = pageIndex;
                    result.PageSize = pageSize;
                    result.IsSuccess = true;
                    result.Entity = verificationViewModel;
                }

                //将提交过来的条件保存在ViewBag中
                //ViewBag.where = model;
                return new JsonResultEx(result);
            }
        }

        [HttpPost]
        //[MultiButton("Reject")]
        public JsonResult Reject(long UserId, long verificationId, RoleType roleType)
        {
            var res = new ResponseResult<RoleStateViewModel>();

            bool result = false;
            long auditUserId = UserId;
            switch (roleType)
            {
                case RoleType.Farmer:
                    result = this.roleVerificationRepository.RejectVerification(verificationId, auditUserId, RoleType.Farmer);
                    break;
                case RoleType.MachineryOperator:
                    result = this.roleVerificationRepository.RejectVerification(verificationId, auditUserId, RoleType.MachineryOperator);
                    break;
                case RoleType.Business:
                    result = this.roleVerificationRepository.RejectVerification(verificationId, auditUserId, RoleType.Business);
                    break;
            }
            if (result == true)
            {
                res.Message = "拒绝了【" + roleType.GetDescription() + "】角色的认证!";
                res.IsSuccess = true;
            }
            else res.IsSuccess = false;

            return new JsonResultEx(res);
        }

        [HttpPost]
        public JsonResult ApproveForFarmer(long UserId, long verificationId, byte star)
        {
            var res = new ResponseResult<RoleStateViewModel>();

            long auditUserId = UserId;
            var farmerVerificationInfo = this.farmerVerificationRepository.GetByKey(verificationId);
            if (farmerVerificationInfo == null)
            {
                res.Message = "记录不存在!";
                //return RedirectToAction("Detail", new { verificationId, roleId = RoleType.Farmer });
                var model = new RoleStateViewModel();
                model.verificationId = verificationId;
                model.roleId = RoleType.Farmer;
                res.IsSuccess = false;
                res.Entity = model;
                return new JsonResultEx(res);
            }
            var roles = this.adminUserRepository.GetRoles(farmerVerificationInfo.UserId);
            if (roles != null)
            {
                int[] roleArray = roles.Select(r => r.RoleID).ToArray();
                if (Array.IndexOf(roleArray, (int)RoleType.Farmer) > -1)
                {
                    //TempData["Error"] = "已经拥有大农户角色!";
                    res.Message = "已经拥有大农户角色!";
                    var model = new RoleStateViewModel();
                    model.verificationId = verificationId;
                    model.roleId = RoleType.Farmer;
                    res.IsSuccess = false;
                    res.Entity = model;
                    return new JsonResultEx(res);
                }
            }
            var result = this.roleVerificationRepository.ApproveFarmerVerification(verificationId, auditUserId, star);
            if (result == true)
            {
                res.Message = "同意了【" + RoleType.Farmer.GetDescription() + "】角色的认证!";
                res.IsSuccess = true;
            }
            return new JsonResultEx(res);
        }

        [HttpPost]
        public JsonResult ApproveForOperator(long UserId, long verificationId, Dictionary<int, int> demandLevelInfoList)
        {
            var res = new ResponseResult<RoleStateViewModel>();

            long auditUserId = UserId;
            var dics = demandLevelInfoList.Where(d => d.Value != 0).ToDictionary(m => m.Key, m => m.Value);
            if (dics.Count == 0)
            {
                res.Message = "请至少选择一个已评星的技能";
                var model = new RoleStateViewModel();
                model.verificationId = verificationId;
                model.roleId = RoleType.MachineryOperator;
                res.IsSuccess = false;
                res.Entity = model;
                return new JsonResultEx(res);
            }
            var machineryOperatorVerificationInfo = this.machineryOperatorVerificationRepository.GetByKey(verificationId);
            if (machineryOperatorVerificationInfo == null)
            {
                res.Message = "记录不存在!";
                var model = new RoleStateViewModel();
                model.verificationId = verificationId;
                model.roleId = RoleType.MachineryOperator;
                res.IsSuccess = false;
                res.Entity = model;
                return new JsonResultEx(res);
            }

            var roles = this.adminUserRepository.GetRoles(machineryOperatorVerificationInfo.UserId);
            if (roles != null)
            {
                int[] roleArray = roles.Select(r => r.RoleID).ToArray();
                if (Array.IndexOf(roleArray, (int)RoleType.MachineryOperator) > -1)
                {
                    res.Message = "已经拥有农机手角色!";
                    var model = new RoleStateViewModel();
                    model.verificationId = verificationId;
                    model.roleId = RoleType.MachineryOperator;
                    res.IsSuccess = false;
                    res.Entity = model;
                    return new JsonResultEx(res);
                }
            }

            var result = this.roleVerificationRepository.ApproveOperatorVerification(verificationId, auditUserId, dics);
            if (result == true)
            {
                res.Message = "同意了【" + RoleType.MachineryOperator.GetDescription() + "】角色的认证!";
                res.IsSuccess = true;
            }

            return new JsonResultEx(res);

        }

        [HttpPost]
        public JsonResult ApproveForBusiness(Int64 auditUserId, long verificationId, Dictionary<int, int> demandLevelInfoList)
        {

            var res = new ResponseResult<RoleStateViewModel>();

            var dics = demandLevelInfoList.Where(d => d.Value != 0).ToDictionary(m => m.Key, m => m.Value);
            if (dics.Count == 0)
            {
                res.Message = "请至少选择一个已评星的技能";
                var model = new RoleStateViewModel();
                model.verificationId = verificationId;
                model.roleId = RoleType.Business;
                res.IsSuccess = false;
                res.Entity = model;
                return new JsonResultEx(res);
            }
            var businessVerificationInfo = this.businessVerificationRepository.GetByKey(verificationId);
            if (businessVerificationInfo == null)
            {
                res.Message = "记录不存在!";
                var model = new RoleStateViewModel();
                model.verificationId = verificationId;
                model.roleId = RoleType.Business;
                res.IsSuccess = false;
                res.Entity = model;
                return new JsonResultEx(res);
            }
            var roles = this.adminUserRepository.GetRoles(businessVerificationInfo.UserId);
            if (roles != null)
            {
                int[] roleArray = roles.Select(r => r.RoleID).ToArray();
                if (Array.IndexOf(roleArray, (int)RoleType.Business) > -1)
                {
                    //TempData["Error"] = "已经拥有产业商角色!";
                    //return RedirectToAction("Detail", new { verificationId, roleId = RoleType.Business });
                    res.Message = "已经拥有产业商角色!";
                    var model = new RoleStateViewModel();
                    model.verificationId = verificationId;
                    model.roleId = RoleType.Business;
                    res.IsSuccess = false;
                    res.Entity = model;
                    return new JsonResultEx(res);
                }
            }
            var result = this.roleVerificationRepository.ApproveBusinessVerification(verificationId, auditUserId, demandLevelInfoList);
            if (result == true)
            {
                //TempData["Message"] = "同意了【" + RoleType.Business.GetDescription() + "】角色的认证!";
                res.Message = "同意了【" + RoleType.Business.GetDescription() + "】角色的认证!";
                res.IsSuccess = true;
            }
            return new JsonResultEx(res);
        }
    }
}