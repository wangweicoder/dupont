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
using DuPont.Admin.Presentation.Attributes;
using DuPont.Admin.Presentation.Config;
using DuPont.Admin.Presentation.Filters;
using DuPont.Admin.Presentation.Models;
using DuPont.Entity.Enum;
using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
namespace DuPont.Admin.Presentation.Controllers
{
#if (!DEBUG)
    [SysAuth]
#endif
    
    public class VerificationController : BaseController
    {
        /// <summary>
        /// 获取请求的api地址
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string GetPostUrl(string methodName)
        {
            return bgApiServerUrl + this.GetType().Name.Replace("Controller", "") + "/" + methodName;
        }


        #region "角色认证信息详情"
        // GET: /Verification/
        /// <summary>
        /// Details the specified verification identifier.
        /// </summary>
        /// <param name="verificationId">The verification identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>ActionResult.</returns>
         [Validate]  
        public ActionResult Detail(int verificationId, RoleType roleId)
        {
            var userId = GetLoginInfo().User.Id;

            var roleType = (RoleType)roleId;
            string viewName = string.Empty;
            string remoteApiUrl = PageConfig.GetRemoteApiUrl();//获取APP接口服务的地址

            var postParas = new Dictionary<string, string>() 
                { 
                    {"UserId",userId.ToString()},
                    {"verificationId",verificationId.ToString()},
                    {"roleId",roleType.ToString()},
                    {"remoteApiUrl",remoteApiUrl}
                };

            object viewModel = null;

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            switch (roleType)
            {
                case RoleType.Farmer:
                    viewName = "Farmer_Detail";
                    var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<FarmerVerificationInfoViewModel>>(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());
                    if (responseResult != null && responseResult.IsSuccess)
                    {
                        viewModel = responseResult.Entity;
                    }
                    else
                    {
                        ThrowException(responseResult.State.Id, responseResult.Message);
                    }
                    break;
                case RoleType.MachineryOperator:
                    viewName = "Machinery_Operator_Detail";
                    var machineOperatorVerificationInfo = RestSharpHelper.PostWithStandard<ResponseResult<MachineOperatorVerificationInfoViewModel>>(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());
                    if (machineOperatorVerificationInfo != null && machineOperatorVerificationInfo.IsSuccess)
                    {
                        viewModel = machineOperatorVerificationInfo.Entity;
                    }
                    else
                    {
                        ThrowException(machineOperatorVerificationInfo.State.Id, machineOperatorVerificationInfo.Message);
                    }
                    break;
                case RoleType.Business:
                    viewName = "Business_Detail";
                    var businessVerificationInfo = RestSharpHelper.PostWithStandard<ResponseResult<BusinessVerificationInfoViewModel>>(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());
                    if (businessVerificationInfo != null && businessVerificationInfo.IsSuccess)
                    {
                        viewModel = businessVerificationInfo.Entity;
                    }
                    else
                    {
                        ThrowException(businessVerificationInfo.State.Id, businessVerificationInfo.Message);
                    }
                    break;
                case RoleType.SuperAdmin:
                case RoleType.Admin:
                case RoleType.Dealer:
                case RoleType.Unknown:
                default:
                    return null;
            }

            return View(viewName, viewModel);
        } 
        #endregion

        #region "角色认证信息列表"
        [NoCache]        
        public ActionResult List(WhereModel model, int pageIndex = 1, int pageSize = 10)
        {
            var userId = GetLoginInfo().User.Id;
            var wheremodel = new WhereModel()
               {
                   RoleId = Convert.ToInt32(string.IsNullOrEmpty(Request["RoleId"]) ? "0" : Request["RoleId"]),
                   Province = string.IsNullOrEmpty(Request["Province"]) ? "0" : Request["Province"],
                   City = string.IsNullOrEmpty(Request["City"]) ? "0" : Request["City"],
                   Region = string.IsNullOrEmpty(Request["Region"]) ? "0" : Request["Region"],
                   PhoneNumber = string.IsNullOrEmpty(Request["PhoneNumber"]) ? "" : Request["PhoneNumber"].ToString(),
               };

            var str_startTime = Request["StartTime"];
            var str_endTime = Request["EndTime"];

            if (!string.IsNullOrEmpty(str_startTime))
            {
                str_startTime = str_startTime.Replace("年", "-").Replace("月", "-").Replace("日", "");
            }
            if (!string.IsNullOrEmpty(str_endTime))
            {
                str_endTime = str_endTime.Replace("年", "-").Replace("月", "-").Replace("日", "");
            }

            DateTime dt_now = DateTime.Now;
            DateTime startTime = dt_now;
            DateTime endTime = dt_now;
            if (string.IsNullOrEmpty(str_startTime))
            {
                startTime = dt_now.AddMonths(-3);
            }
            else
            {
                startTime = DateTime.Parse(str_startTime);
            }

            if (string.IsNullOrEmpty(str_endTime))
            {
                endTime = dt_now;
            }
            else
            {
                endTime = DateTime.Parse(str_endTime);
            }


            wheremodel.StartTime = startTime;
            wheremodel.EndTime = new DateTime(endTime.Year,
                                                 endTime.Month,
                                                 endTime.Day,
                                                 23,
                                                 59,
                                                 59);

            var postParas = new Dictionary<string, string>() 
                { 
                    {"UserId",userId.ToString()},
                    {"pageIndex",pageIndex.ToString()},
                    {"pageSize",pageSize.ToString()},
                    {"RoleId",wheremodel.RoleId.ToString()},
                    {"Province",wheremodel.Province},
                    {"City",wheremodel.City.ToString()},
                    {"Region",wheremodel.Region.ToString()},
                    {"PhoneNumber",wheremodel.PhoneNumber.ToString()},
                    {"StartTime",wheremodel.StartTime.ToString()},
                    {"EndTime",wheremodel.EndTime.ToString()}
                };


            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<RoleVerificationViewModelWithoutPager>>(GetPostUrl("Search"), postParas, GetCertificationFilePath(), GetCertificationPwd());

            if (responseResult != null && responseResult.IsSuccess)
            {
                var roleVerification = new RoleVerificationViewModel();
                roleVerification.Pager = new PagedList<string>(new string[0], responseResult.PageIndex, responseResult.PageSize, (int)responseResult.TotalNums);
                roleVerification.PendingAuditList = responseResult.Entity.PendingAuditList;
                roleVerification.Wheremodel = wheremodel;
                return View("List", roleVerification);
            }
            return View();
        } 
        #endregion

        #region "获取地区列表"
        /// <summary>
        /// 通过父类编号获取子类编号并序列化为json
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
         [Validate]  
        public JsonResult GetAreaChild(string ParentAId)
        {
            var postParas = new Dictionary<string, string>();
            postParas.Add("ParentAId", ParentAId);
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<List<AreaViewModel>>>(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());

            if (responseResult != null && responseResult.IsSuccess)
            {
                return new JsonResultEx(responseResult);
            }
            else
            {
                return new JsonResultEx(new List<AreaViewModel>());
            }
        } 
        #endregion

        #region "回绝指定用户的角色申请"
        [HttpPost]
        [MultiButton("Reject")]
        public ActionResult Reject(long verificationId, RoleType roleType)
        {
            var auditUser = GetLoginInfo();
            if (auditUser == null)
            {
                return null;
            }
            long auditUserId = auditUser.User.Id;
            var postParas = new Dictionary<string, string>() 
                { 
                    {"verificationId",verificationId.ToString()},
                    {"roleType",roleType.ToString()},
                    {"UserId",auditUserId.ToString()}
                };

            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<RoleStateViewModel>>(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());

            if (responseResult != null && responseResult.IsSuccess)
            {
                TempData["Message"] = responseResult.Message;
                return RedirectToAction("List");
            }
            else
            {
                TempData["Error"] = responseResult.Message;
                return RedirectToAction("Detail", new { responseResult.Entity.verificationId, roleId = (RoleType)responseResult.Entity.roleId });
            }
        } 
        #endregion

        #region "同意指定用户的大农户角色申请"
        [HttpPost]
        [MultiButton("ApproveForFarmer")]
        public ActionResult ApproveForFarmer(long verificationId, byte star = 1)
        {
            var auditUser = GetLoginInfo();
            if (auditUser == null)
            {
                return null;
            }
            long auditUserId = auditUser.User.Id;
            var postParas = new Dictionary<string, string>() 
                { 
                    {"verificationId",verificationId.ToString()},
                    {"star",star.ToString()},
                    {"UserId",auditUserId.ToString()}
                };

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<RoleStateViewModel>>(GetPostUrl("ApproveForFarmer"), postParas, GetCertificationFilePath(), GetCertificationPwd());
            if (responseResult == null)
            {
                return RedirectToAction("List");
            }
            if (!responseResult.IsSuccess)
            {
                TempData["Error"] = responseResult.Message;
                return RedirectToAction("Detail", new { verificationId, roleId = RoleType.Farmer });
            }
            else
            {
                TempData["Message"] = responseResult.Message;
            }
            return RedirectToAction("List");
        } 
        #endregion

        #region "同意指定用户的农机手角色申请"
        [HttpPost]
        [MultiButton("ApproveForOperator")]
        public ActionResult ApproveForOperator(long verificationId, Dictionary<int, int> demandLevelInfoList)
        {
            var auditUser = GetLoginInfo();
            if (auditUser == null)
            {
                return null;
            }
            long auditUserId = GetLoginInfo().User.Id;

            var postParas = new Dictionary<string, string>() 
                { 
                    {"verificationId",verificationId.ToString()},
                    {"UserId",auditUserId.ToString()}
                };
            for (int i = 0; i < demandLevelInfoList.Count; i++)
            {
                var keyValuePair = demandLevelInfoList.ElementAt(i);
                postParas.Add("demandLevelInfoList[" + i + "].Key", keyValuePair.Key.ToString());
                postParas.Add("demandLevelInfoList[" + i + "].Value", keyValuePair.Value.ToString());
            }

            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<RoleStateViewModel>>(GetPostUrl("ApproveForOperator"), postParas, GetCertificationFilePath(), GetCertificationPwd());
            if (responseResult == null)
            {
                return RedirectToAction("List");
            }
            if (!responseResult.IsSuccess)
            {
                TempData["Error"] = responseResult.Message;
                return RedirectToAction("Detail", new { verificationId, roleId = RoleType.Farmer });
            }
            else
            {
                TempData["Message"] = responseResult.Message;
            }
            return RedirectToAction("List");
        } 
        #endregion

        #region "同意指定用户的产业商角色申请"
        /// <summary>
        /// Approves for business.
        /// </summary>
        /// <param name="verificationId">The verification identifier.</param>
        /// <param name="demandLevelInfoList">The demand level information list.</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        [MultiButton("ApproveForBusiness")]
        public ActionResult ApproveForBusiness(long verificationId, Dictionary<int, int> demandLevelInfoList)
        {

            var auditUser = GetLoginInfo();
            long auditUserId = auditUser.User.Id;

            var postParas = new Dictionary<string, string>() 
                { 
                    {"verificationId",verificationId.ToString()},
                    {"auditUserId",auditUserId.ToString()}
                };

            for (int i = 0; i < demandLevelInfoList.Count; i++)
            {
                var keyValuePair = demandLevelInfoList.ElementAt(i);
                postParas.Add("demandLevelInfoList[" + i + "].Key", keyValuePair.Key.ToString());
                postParas.Add("demandLevelInfoList[" + i + "].Value", keyValuePair.Value.ToString());
            }

            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<RoleStateViewModel>>(GetPostUrl("ApproveForBusiness"), postParas, GetCertificationFilePath(), GetCertificationPwd());
            if (responseResult == null)
            {
                return RedirectToAction("List");
            }
            if (!responseResult.IsSuccess)
            {
                TempData["Error"] = responseResult.Message;
                return RedirectToAction("Detail", new { verificationId, roleId = RoleType.Farmer });
            }
            else
            {
                TempData["Message"] = responseResult.Message;
            }
            return RedirectToAction("List");
        } 
        #endregion

    }
}