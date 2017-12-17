using DuPont.Admin.Presentation.Filters;
// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-14-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-14-2015
// ***********************************************************************
// <copyright file="DemandController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Admin.Presentation.Models;

using DuPont.Utility;
using Webdiyer.WebControls.Mvc;

using DuPont.Admin.Presentation.Attributes;
using DuPont.Models.Dtos.Background.Demand;
using DuPont.Extensions;
using DuPont.Models.Enum;
using DuPont.Global.Filters.ActionFilters;
namespace DuPont.Admin.Presentation.Controllers
{
#if (!DEBUG)
    [SysAuth]
#endif
    [Validate]
    /// <summary>
    /// 需求管理
    /// </summary>
    public class DemandController : BaseController
    {
        #region "产业商需求列表"
        public ActionResult BusinessList(BusinessSeachModel model)
        {
            if (CheckInputWhenReturnActionResult())
            {
                var responseResult = PostStandardWithSameControllerAction<List<BusinessListModel>,
                    BusinessSeachModel>(this, model);
                if (responseResult != null && responseResult.IsSuccess)
                {
                    var roleVerification = new BusinessDemandViewModel();
                    roleVerification.Pager = new PagedList<string>(new string[0], responseResult.PageIndex, responseResult.PageSize, (int)responseResult.TotalNums);
                    roleVerification.PendingAuditList = responseResult.Entity;
                    roleVerification.Wheremodel = new BusinessSeachModel
                    {
                        DemandTypeId = model.DemandTypeId.DefaultIfEmpty("0"),
                        PublishStateId = model.PublishStateId.DefaultIfEmpty("0"),
                        ProvinceAid = model.ProvinceAid.DefaultIfEmpty("0"),
                        CityAid = model.CityAid.DefaultIfEmpty("0"),
                        RegionAid = model.RegionAid.DefaultIfEmpty("0"),
                        IsDeleted=model.IsDeleted
                    };
                    return View("BusinessList", roleVerification);
                }
            }

            return View();
        }
        #endregion

        #region "产业商需求详情"
        public ActionResult BusinessDetail(int DemandId)
        {
            var responseResult = PostStandardWithSameControllerAction<BusinessDetailModel>(this,
                new Dictionary<string, string> { { "DemandId", DemandId.ToString() } });
            if (responseResult != null && responseResult.IsSuccess)
            {
                return View("BusinessDetail", responseResult.Entity);
            }
            return View();
        }
        #endregion

        #region "大农户需求列表"
        public ActionResult FarmerList(FarmerSeachModel model)
        {
            var responseResult = PostStandardWithSameControllerAction<List<DuPont.Models.Dtos.Background.Demand.FarmerDemandViewModel.FarmerDemandModel>, FarmerSeachModel>(this,
                model);
            if (responseResult != null && responseResult.IsSuccess)
            {
                var roleVerification = new FarmerDemandViewModel();
                roleVerification.Pager = new PagedList<string>(new string[0], responseResult.PageIndex, responseResult.PageSize, (int)responseResult.TotalNums);
                roleVerification.PendingAuditList = responseResult.Entity;
                roleVerification.Wheremodel = new FarmerSeachModel
                {
                    DemandTypeId = model.DemandTypeId.DefaultIfEmpty("0"),
                    PublishStateId = model.PublishStateId.DefaultIfEmpty("0"),
                    ProvinceAid = model.ProvinceAid.DefaultIfEmpty("0"),
                    CityAid = model.CityAid.DefaultIfEmpty("0"),
                    RegionAid = model.RegionAid.DefaultIfEmpty("0"),
                    IsDeleted=model.IsDeleted
                };
                return View("FarmerList", roleVerification);
            }
            return View();
        }
        #endregion

        #region "大农户需求详情"
        public ActionResult FarmerDetail(int DemandId)
        {
            var responseResult = PostStandardWithSameControllerAction<FarmerDetailModel>(this,
                   new Dictionary<string, string> { { "DemandId", DemandId.ToString() } });
            if (responseResult != null && responseResult.IsSuccess)
            {
                return View("FarmerDetail", responseResult.Entity);
            }
            return View();
        }
        #endregion

        #region "关闭产业商指定需求"
        [HttpPost]
        public ActionResult BusinessClose(string ids)
        {
            var responseResult = PostStandardWithSameControllerAction<object>(this,
               new Dictionary<string, string> { { "ids", ids } });

            if (!responseResult.IsSuccess)
            {
                TempData["Error"] = "关闭失败";
            }

            TempData["Message"] = "关闭成功!";
            return RedirectToAction("BusinessList");
        }
        #endregion

        #region "关闭大农户指定需求"
        [HttpPost]
        public ActionResult FarmerClose(string ids)
        {
            var responseResult = PostStandardWithSameControllerAction<object>(this,
               new Dictionary<string, string> { { "ids", ids } });

            if (!responseResult.IsSuccess)
            {
                TempData["Error"] = "关闭失败";
            }

            TempData["Message"] = "关闭成功!";
            return RedirectToAction("FarmerList");
        }
        #endregion

        #region "导出产业商需求列表到Excel"
        [HttpGet]
        [Validate]
        public ActionResult ExportExcelWithBusinessList(BusinessSeachModel model)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);

            RestSharp.RestClient client = new RestSharp.RestClient();
            RestSharp.RestRequest request = new RestSharp.RestRequest();
            client.BaseUrl = new Uri(GetCurrentUrl(this));
            Dictionary<string, string> parameters = ModelHelper.GetPropertyDictionary<BusinessSeachModel>(model);
            foreach (var para in parameters)
            {
                request.AddParameter(para.Key, para.Value.IsNullOrEmpty() ? null : para.Value);
            }

            if (request.Parameters.Count(p => p.Name == DataKey.UserId) == 0)
            {
                request.AddParameter(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            var responseResult = client.ExecuteAsGet(request, "GET");
            if (responseResult.Content == "no data")
            {
                return Content("<script>alert('没有符合条件的数据可被导出!');history.go(-1)</script>");
            }
            var contentDispositionHeader = responseResult.Headers.First(p => p.Name == "Content-Disposition").Value.ToString().Replace(" ", string.Empty);
            var attachFileName = contentDispositionHeader.Replace("attachment;filename=", string.Empty);
            return File(responseResult.RawBytes, responseResult.ContentType, attachFileName);
        }
        #endregion

        #region "导出大农户需求列表到Excel"
        public ActionResult ExportExcelWithFarmList(FarmerSeachModel model)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);

            RestSharp.RestClient client = new RestSharp.RestClient();
            RestSharp.RestRequest request = new RestSharp.RestRequest();
            client.BaseUrl = new Uri(GetCurrentUrl(this));
            Dictionary<string, string> parameters = ModelHelper.GetPropertyDictionary<FarmerSeachModel>(model);
            foreach (var para in parameters)
            {
                request.AddParameter(para.Key, para.Value.IsNullOrEmpty() ? null : para.Value);
            }

            if (request.Parameters.Count(p => p.Name == DataKey.UserId) == 0)
            {
                request.AddParameter(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            var responseResult = client.ExecuteAsGet(request, "GET");
            if (responseResult.Content == "no data")
            {
                return Content("<script>alert('没有符合条件的数据可被导出!');history.go(-1)</script>");
            }
            var contentDispositionHeader = responseResult.Headers.First(p => p.Name == "Content-Disposition").Value.ToString().Replace(" ", string.Empty);
            var attachFileName = contentDispositionHeader.Replace("attachment;filename=", string.Empty);
            return File(responseResult.RawBytes, responseResult.ContentType, attachFileName);
        }
        #endregion
    }
}