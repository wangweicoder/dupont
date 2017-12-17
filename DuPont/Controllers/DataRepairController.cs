using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Interface;
using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Utility;
using DuPont.Models.Models;
using DuPont.Global.ActionResults;

namespace DuPont.Controllers
{
    public class DataRepairController : BaseController
    {
        protected readonly IUser _userService;
        protected readonly IOperatorInfoVerifciationRepository _operatorVerificationService;
        protected readonly IMachineDemandTypeRL _machineDemandTypeRLService;
        protected readonly ISys_Dictionary _sysDictionaryService;
        protected readonly IUserRoleDemandTypeLevelRL _userRoleDemandTypeLevelRLService;
        protected readonly IFarmerRequirement _farmerDemandService;
        protected readonly IUser_Role _userRoleService;
        protected readonly IBusiness _businessService;
        protected readonly IBusinessDemand_Response _businessDemandResponseService;
        protected readonly IFarmerDemandResponse _farmerDemandResponse;


        public DataRepairController(
            IPermissionProvider permissionManage,
            IAdminUser adminUserRepository,
            IUser userService,
            IOperatorInfoVerifciationRepository operatorVerificationService,
            IMachineDemandTypeRL machineDemandTypeRLService,
            ISys_Dictionary sysDictionaryService,
            IUserRoleDemandTypeLevelRL userRoleDemandTypeLevelRLService,
            IFarmerRequirement farmerDemandService,
            IUser_Role userRoleService,
            IBusiness businessService,
            IBusinessDemand_Response businessDemandResponseService,
            IFarmerDemandResponse farmerDemandResponse)
            : base(permissionManage, adminUserRepository)
        {
            _userService = userService;
            _operatorVerificationService = operatorVerificationService;
            _machineDemandTypeRLService = machineDemandTypeRLService;
            _sysDictionaryService = sysDictionaryService;
            _userRoleDemandTypeLevelRLService = userRoleDemandTypeLevelRLService;
            _farmerDemandService = farmerDemandService;
            _userRoleService = userRoleService;
            _businessService = businessService;
            _businessDemandResponseService = businessDemandResponseService;
            _farmerDemandResponse = farmerDemandResponse;
        }

        #region "修复农机手的机器与服务能力的映射关系"
        /// <summary>
        /// 修复农机手的机器与服务能力的映射关系
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RepairOperatorMachineAndDemandTypeMapping()
        {
            if (UserInfo == null || UserInfo.IsSuperAdmin == false)
                throw new UnauthorizedAccessException();

            using (var result = new ResponseResult<object>())
            {
                long recordCount = 0;
                var operatorRoleId = (int)RoleType.MachineryOperator;
                //获取所有农机手数据
                var operatorList = _userService.GetUserList(1, int.MaxValue, out recordCount, new Models.Models.WhereModel
                {
                    RoleId = operatorRoleId,
                });

                if (operatorList.Count > 0)
                {
                    foreach (var operatorItem in operatorList)
                    {
                        long totalCount = 0;
                        //获取该用户的角色申请信息
                        var operatorVerificationInfo = _operatorVerificationService.GetAll<DateTime>(
                            p => p.UserId == operatorItem.UserId && p.AuditState == 1
                            , null
                            , p => p.CreateTime
                            , 1
                            , 1
                            , out totalCount).FirstOrDefault();

                        if (operatorVerificationInfo == null)
                            continue;

                        //获取该用户的角色申请信息
                        var machineInfoString = operatorVerificationInfo.Machinery;
                        if (machineInfoString.IsNullOrEmpty() || machineInfoString.StartsWith("[") == false)
                            continue;

                        //获取大家户所拥有的机器
                        var machineList = JsonHelper.FromJsonTo<List<ProductInfo>>(machineInfoString);
                        var machineNameList = machineList.Select(
                            p => p.Name)
                            .Distinct()
                            .ToList();

                        var machineIdList = _sysDictionaryService.GetAll(
                            p => machineNameList.Contains(p.DisplayName))
                            .Select(p => p.Code)
                            .Distinct()
                            .ToList();

                        //将机器匹配上对应的服务能力
                        var operatorDemandTypeIdList = _machineDemandTypeRLService.GetAll(
                              p => machineIdList.Contains(p.MachineId))
                              .Select(p => p.DemandTypeId)
                              .Distinct()
                              .ToList();

                        //删除用户农机角色下的所有服务能力
                        _userRoleDemandTypeLevelRLService.Delete(
                            p => p.UserId == operatorItem.UserId
                                && p.RoleId == operatorRoleId);

                        //将与机器匹配后的能力列表记录到数据库
                        _userRoleDemandTypeLevelRLService.Insert(
                            from demandTypeID in operatorDemandTypeIdList
                            select new T_USER_ROLE_DEMANDTYPELEVEL_RELATION
                            {
                                CreateTime = DateTime.Now,
                                RoleId = operatorRoleId,
                                DemandId = demandTypeID,
                                Star = 1,
                                UserId = operatorItem.UserId
                            }
                        );
                    }
                }

                result.IsSuccess = true;
                result.Message = "农机手角色服务能力数据修复成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "修复以前注册用户角色等级星数"
        [HttpPost]
        public ActionResult RepairOriginalUserStar()
        {
            if (UserInfo == null || UserInfo.IsSuperAdmin == false)
                throw new UnauthorizedAccessException();

            using (var result = new ResponseResult<object>())
            {
                var farmerRoleId = (int)RoleType.Farmer;
                //获取所有的大农户用户
                var farmerList = _userRoleService.GetAll(p => p.RoleID == farmerRoleId && p.MemberType);
                //获取大农户可以发给产业商的需求类型编号列表
                var farmerPublishedForBusinessDemandList = _sysDictionaryService.GetAll(p => p.ParentCode == 100800);
                var forBusinessDemandCodeList = farmerPublishedForBusinessDemandList.Select(p => p.Code).ToList();

                if (farmerList != null && farmerList.Count() > 0)
                {
                    foreach (var farmer in farmerList)
                    {
                        //获取大农户发布的所有针对产业商需求
                        var farmerPublishedDemandsForBusiness = _farmerDemandService.GetAll(p =>
                            forBusinessDemandCodeList.Contains(p.DemandTypeId)
                            && p.CreateUserId == farmer.UserID);

                        var totalCommentCount = 0;
                        var totalCommentStartCount = 0;

                        if (farmerPublishedDemandsForBusiness != null && farmerPublishedDemandsForBusiness.Count() > 0)
                        {

                            foreach (var demand in farmerPublishedDemandsForBusiness)
                            {
                                //获取该需求的响应记录，大农户发布的需求，响应记录只有一条
                                var farmerDemandReplyList = _farmerDemandService.GetDemandReplyList(demand.Id);
                                if (farmerDemandReplyList != null && farmerDemandReplyList.Count() > 0)
                                {
                                    var farmerDemandReplyDetail = farmerDemandReplyList.FirstOrDefault();
                                    if (farmerDemandReplyDetail.Score > 0)
                                    {
                                        totalCommentCount++;
                                        totalCommentStartCount += farmerDemandReplyDetail.Score;
                                    }
                                }
                            }
                        }

                        //获取产业商发布给大农户的需求
                        var businessDemandReplyList = _businessDemandResponseService.GetAll(p => p.UserId == farmer.UserID);
                        if (businessDemandReplyList != null && businessDemandReplyList.Count() > 0)
                        {
                            foreach (var demand in businessDemandReplyList)
                            {
                                if (demand.Score > 0)
                                {
                                    totalCommentCount++;
                                    totalCommentStartCount += demand.Score;
                                }
                            }
                        }

                        if (totalCommentCount > 0)
                        {
                            _userRoleService.Update(p => p.UserID == farmer.UserID && p.RoleID == farmerRoleId && p.MemberType, t => new T_USER_ROLE_RELATION
                            {
                                TotalReplyCount = totalCommentCount,
                                TotalStarCount = totalCommentStartCount,
                                Star = totalCommentStartCount / totalCommentCount
                            });
                        }
                    }
                }

                var operatorRoleId = (int)RoleType.MachineryOperator;

                //获取农机手信息
                var operatorList = _userRoleService.GetAll(p => p.MemberType && p.RoleID == operatorRoleId);
                if (operatorList != null && operatorList.Count() > 0)
                {
                    foreach (var operatorUser in operatorList)
                    {
                        var totalCommentCount = 0;
                        var totalCommentStartCount = 0;
                        //获取农机手响应记录
                        var farmerDemandResponseList = _farmerDemandResponse.GetAll(p => p.UserId == operatorUser.UserID);
                        if (farmerDemandResponseList != null && farmerDemandResponseList.Count() > 0)
                        {
                            foreach (var demand in farmerDemandResponseList)
                            {
                                if (demand.Score > 0)
                                {
                                    totalCommentCount++;
                                    totalCommentStartCount += demand.Score;
                                }
                            }
                        }

                        if (totalCommentCount > 0)
                        {
                            _userRoleService.Update(p => p.UserID == operatorUser.UserID && p.RoleID == operatorRoleId && p.MemberType, t => new T_USER_ROLE_RELATION
                            {
                                TotalReplyCount = totalCommentCount,
                                TotalStarCount = totalCommentStartCount,
                                Star = totalCommentStartCount / totalCommentCount
                            });
                        }
                    }
                }

                result.IsSuccess = true;
                result.Message = "用户等级数据修复成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion
    }
}