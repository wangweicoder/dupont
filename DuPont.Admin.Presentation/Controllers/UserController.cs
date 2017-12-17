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

using DuPont.Admin.Presentation.Filters;
using DuPont.Admin.Presentation.Models;
using DuPont.Entity.Enum;
using DuPont.Extensions;
using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using DuPont.Models.Dtos;
using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Models.ViewModel;
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
    
    public class UserController : BaseController
    {
        #region "Public Methods"

        #region "Index"
        //
        // GET: /User/
        public ActionResult Index()
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            return View();
        }
        #endregion

        #region "List"
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [NoCache]
        [Validate]
        public ActionResult List(int pageIndex = 1, int pageSize = 10)
        {
            WhereModel wheremodel;
            Dictionary<string, string> postParas;
            GetSearchParameters(pageIndex, pageSize, out wheremodel, out postParas);
            var responseObj = RestSharpHelper.PostWithStandard<ResponseResult<UserModelWithNoPager>>(GetCurrentUrl(this), postParas, GetCertificationFilePath(), GetCertificationPwd());

            if (responseObj != null)
            {
                if (responseObj.IsSuccess)
                {
                    var usermodel = new UserModel();
                    usermodel.Pager = new PagedList<string>(new string[0], responseObj.PageIndex, responseObj.PageSize, (int)responseObj.TotalNums);
                    usermodel.PendingAuditList = responseObj.Entity.PendingAuditList;
                    usermodel.Wheremodel = wheremodel;
                    ViewBag.IsSuperOrAdministrator = GetLoginInfo().IsSuperOrAdministrator;
                    return View(usermodel);
                }
                else
                {
                    ThrowException(responseObj.State.Id, responseObj.Message);
                }
            }
            return View();
        }

        private void GetSearchParameters(int pageIndex, int pageSize, out WhereModel wheremodel, out Dictionary<string, string> postParas)
        {
            wheremodel = new WhereModel()
            {
                RoleId = Convert.ToInt32(Request["RoleId"].DefaultIfEmpty("0")),
                Province = Request["Province"].DefaultIfEmpty("0"),
                City = Request["City"].DefaultIfEmpty("0"),
                Region = Request["Region"].DefaultIfEmpty("0"),
                PhoneNumber = Request["PhoneNumber"].DefaultIfEmpty(string.Empty),

            };

            var str_startTime = Request["StartTime"];
            var str_endTime = Request["EndTime"];
            var str_UserTypeId = Request["UserTypeId"];

            if (str_startTime.IsNullOrEmpty() == false)
            {
                str_startTime = str_startTime.Replace("年", "-").Replace("月", "-").Replace("日", "");
            }
            if (str_endTime.IsNullOrEmpty() == false)
            {
                str_endTime = str_endTime.Replace("年", "-").Replace("月", "-").Replace("日", "");
            }

            DateTime dt_now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
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


            int userTypeId = 0;
            if (str_UserTypeId.IsNullOrEmpty() == false && int.TryParse(str_UserTypeId, out userTypeId))
            {
                wheremodel.UserTypeId = int.Parse(str_UserTypeId);
            }

            wheremodel.StartTime = startTime;
            wheremodel.EndTime = new DateTime(endTime.Year, endTime.Month, endTime.Day, 23, 59, 59);
           

            postParas = new Dictionary<string, string>();
            postParas.Add("pageIndex", pageIndex.ToString());
            postParas.Add("pageSize", pageSize.ToString());
            if (Request["RoleId"] == null)
            {
                postParas.Add("RoleId", "0");
                postParas.Add("Province", "0");
                postParas.Add("City", "0");
                postParas.Add("Region", "0");
                postParas.Add("PhoneNumber", "");
                postParas.Add("StartTime", wheremodel.StartTime.ToString());
                postParas.Add("EndTime", wheremodel.EndTime.ToString());
            }
            else
            {
                postParas.Add("RoleId", Request["RoleId"].DefaultIfEmpty("0"));
                postParas.Add("Province", Request["Province"].DefaultIfEmpty("0"));
                postParas.Add("City", Request["City"].DefaultIfEmpty("0"));
                postParas.Add("Region", Request["Region"].DefaultIfEmpty("0"));
                postParas.Add("PhoneNumber", Request["PhoneNumber"].DefaultIfEmpty(string.Empty));
                postParas.Add("StartTime", wheremodel.StartTime.ToString());
                postParas.Add("EndTime", wheremodel.EndTime.ToString());
            }


            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            if (wheremodel.UserTypeId != null)
            {
                postParas.Add("UserTypeId", wheremodel.UserTypeId.Value.ToString());
            }
        }
        #endregion

        #region "GetUpdData"
        /// <summary>
        /// 获取要修改的数据
        /// </summary>
        /// <returns></returns>
         [HttpGet]
        public ActionResult GetUpdData(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction("List");
            }
            var postParas = new Dictionary<string, string>();
            postParas.Add("id", id.ToString());
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseObj = PostStandardWithSameControllerAction<UserInfoModel>(this, postParas);
            //var responseObj = JsonHelper.FromJsonTo<ResponseResult<UserInfoModel>>(responseString);
            if (responseObj != null)
            {
                if (!responseObj.IsSuccess)
                {
                    ThrowException(responseObj.State.Id, responseObj.Message);
                }
            }

            return View(responseObj.Entity);
        }
        #endregion

        #region "UpdUserInfo"
        /// <summary>
        /// 修改用户的基本信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Validate]   
        public ActionResult UpdUserInfo(T_USER model, Dictionary<int, byte?> demandLevelInfoList)
        {
            var postParas = new Dictionary<string, string>();
            //postParas.Add("UserId", GetLoginInfo().User.Id.ToString());
            postParas.Add("UserId", model.Id.ToString());
            Dictionary<string, string> usermodel = Utility.ModelHelper.GetPropertyDictionary<T_USER>(model);
            foreach (var item in usermodel)
            {
                postParas.Add(item.Key, item.Value);
            }
            for (var item = 0; item < demandLevelInfoList.Count; item++)
            {
                var tempItem = demandLevelInfoList.ElementAt(item);
                postParas.Add("demandLevelInfoList[" + item + "].Key", tempItem.Key.ToString());
                postParas.Add("demandLevelInfoList[" + item + "].Value", tempItem.Value.ToString());
            }
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseString = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), postParas, certification, certificationPwd);
            var responseObj = JsonHelper.FromJsonTo<ResponseResult<object>>(responseString);
            if (responseObj.IsSuccess == true)
            {
                TempData["Message"] = "修改成功!";
                return RedirectToAction("List");
            }
            else
            {
                ModelState.AddModelError("", responseObj.Message);
                return View("GetUpdData");
            }
        }
        #endregion

        #region UpdateLand
        [Validate]
        [HttpPost]
        /// <summary>
        /// 认证亩数
        /// </summary>
        /// <param name="Land">土地亩数</param>
        /// <param name="Id">用户编号</param>
        /// <author>ww</author>
        public JsonResult UpdateLand(string Land, long UserId = 0)
        {
            //ww
            if (UserId == 0)
            {
                return Json(new { success = true, responseText = "审核失败" });
            }
            var postParas = new Dictionary<string, string>();
            postParas.Add("Id", UserId.ToString());
            postParas.Add("Land", Land.ToString());
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseObj = PostStandardWithSameControllerAction<object>(this, postParas);
            if (responseObj != null)
            {
                if (responseObj.IsSuccess)
                {                   
                    return Json(new { code = 1, responseText = "审核成功" });
                }
            }
            else
            {
                return Json(new { code = 0, responseText = "审核失败" });
            }
            return Json(new { code = 0, responseText = "审核失败" });
        }

        #endregion

        #region "AddUser"
        /// <summary>
        /// 添加角色界面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUser()
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            ViewBag.IsSuperAdmin = GetLoginInfo().User.IsSuperAdmin;
            return View();
        }

        /// <summary>
        /// 添加经销商
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Validate]
        [HttpPost]
        public JsonResult AddUser(UserViewModel model)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl, true);

            if (ModelState.IsValid)
            {
                if (model.Province == "0" || model.City == "0" || model.Region == "0")
                {
                    return Json(new { success = false, responseText = "省、市、区县为必选字段!" });
                }


                var postParas = new Dictionary<string, string>();
                postParas.Add("AuditUserId", GetLoginInfo().User.Id.ToString());
                Dictionary<string, string> usermodel = Utility.ModelHelper.GetPropertyDictionary<UserViewModel>(model);
                foreach (var item in usermodel)
                {
                    postParas.Add(item.Key, item.Value);
                }
                //证书的路径
                var certification = GetCertificationFilePath();
                //证书的密码
                var certificationPwd = GetCertificationPwd();

                if (postParas.ContainsKey(DataKey.UserId) == false)
                {
                    postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
                }
                var responseString = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), postParas, certification, certificationPwd);
                var responseObj = JsonHelper.FromJsonTo<ResponseResult<object>>(responseString);
                if (responseObj.IsSuccess == true)
                {
                    //TempData["Message"] = "添加成功!";
                    //return RedirectToAction("List");
                    HttpContext.Items[DataKey.RemoveSessionCookie] = "yes";
                    Session.RemoveAll();
                    return Json(new { success = true, responseText = "添加成功" });
                }
                else
                {
                    //ModelState.AddModelError("", responseObj.Message);
                    return Json(new { success = false, responseText = responseObj.Message });
                }
            }
            else
            {
                //ModelState.AddModelError("", "添加失败-参数无效！");
                //return View();
                var error = ModelState.Values.Where(ms => ms.Errors.Count > 0).First().Errors.First();
                return Json(new { success = false, responseText = error.ErrorMessage });
            }
        }
        #endregion

        #region "AddManger"
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <returns></returns>
        [Validate]
        [HttpPost]
        public JsonResult AddManger(UserViewModel model)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl, true);

            if (ModelState.IsValid)
            {
                var postParas = new Dictionary<string, string>();
                postParas.Add("AuditUserId", GetLoginInfo().User.Id.ToString());

                Dictionary<string, string> usermodel = Utility.ModelHelper.GetPropertyDictionary<UserViewModel>(model);
                foreach (var item in usermodel)
                {
                    postParas.Add(item.Key, item.Value);
                }

                //证书的路径
                var certification = GetCertificationFilePath();
                //证书的密码
                var certificationPwd = GetCertificationPwd();

                if (postParas.ContainsKey(DataKey.UserId) == false)
                {
                    postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
                }
                var responseString = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), postParas, certification, certificationPwd);
                var responseObj = JsonHelper.FromJsonTo<ResponseResult<object>>(responseString);
                if (responseObj.IsSuccess == true)
                {
                    //TempData["Message"] = "添加成功!";
                    //return RedirectToAction("List");
                    HttpContext.Items[DataKey.RemoveSessionCookie] = "yes";
                    Session.RemoveAll();
                    return Json(new { success = true, responseText = "添加成功" });
                }
                else
                {
                    // TempData["Error"] = responseObj.Message;
                    ModelState.AddModelError("", responseObj.Message);
                    return Json(new { success = false, responseText = responseObj.Message });
                }
            }

            var error = ModelState.Values.Where(ms => ms.Errors.Count > 0).First().Errors.First();
            return Json(new { success = false, responseText = error.ErrorMessage });
        }
        #endregion

        #region "获取带专家标识的用户列表"
        [HttpGet]
        [Validate]
        public ActionResult ExpertList(SearchExpertListInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);

            if (input.PageIndex == 0)
                input.PageIndex = 1;

            if (input.PageSize == 0)
                input.PageSize = 10;

            var parameters = ModelHelper.GetPropertyDictionary<SearchExpertListInput>(input);
            var result = PostStandardWithSameControllerAction<List<SearchExpertListOutput>>(this, parameters);
            var model = new MultiModel<List<SearchExpertListOutput>>(result.IsSuccess, input.PageIndex, input.PageSize, (int)result.TotalNums, result.Entity);
            return View(model);
        }
        #endregion

        #region "指定为专家"
        [HttpPost]
        [Validate]
        public JsonResult GrantExpert(params long[] userIds)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var result = PostStandardWithSameControllerAction<object>(this, new Dictionary<string, string>() { { "userIds", string.Join(",", userIds) } });
            return new JsonResultEx(result);
        }
        #endregion

        #region "取消为专家"
        [HttpPost]
        [Validate]
        public JsonResult CancelExpert(params long[] userIds)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var result = PostStandardWithSameControllerAction<object>(this, new Dictionary<string, string>() { { "userIds", string.Join(",", userIds) } });
            return new JsonResultEx(result);
        }
        #endregion

        #region "前台账号锁定/解锁"
        [HttpGet]
        [Validate]
        public ActionResult StateManage(SearchExpertListInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);

            if (input.PageIndex == 0)
                input.PageIndex = 1;

            if (input.PageSize == 0)
                input.PageSize = 10;

            var parameters = ModelHelper.GetPropertyDictionary<SearchExpertListInput>(input);
            var result = PostStandardWithSameControllerAction<List<SearchUserListWithStateOutput>>(this, parameters);
            var model = new MultiModel<List<SearchUserListWithStateOutput>>(result.IsSuccess, input.PageIndex, input.PageSize, (int)result.TotalNums, result.Entity);
            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateLockState(bool isLock, params long[] userIds)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var parameter = new Dictionary<string, string>(){
                { "userIds", string.Join(",", userIds) },
                {"isLock",isLock.ToString()}
            };

            var result = PostStandardWithControllerAction<object>("User", "UpdateLockState", parameter);
            return new JsonResultEx(result);
        }
        #endregion

        #region "后台用户列表"
        [Validate]
        [HttpGet]
        public ActionResult BackgroundUserList(SearchBackgroundUserListInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);

            var parameters = ModelHelper.GetPropertyDictionary<SearchBackgroundUserListInput>(input);
            var result = PostStandardWithSameControllerAction<List<SearchBackgroundUserListOutput>>(this, parameters);
            var searchModel = new SearchModel<SearchBackgroundUserListInput, List<SearchBackgroundUserListOutput>>(result.IsSuccess, input, result.Entity, (int)result.TotalNums);
            return View(searchModel);
        }
        #endregion

        #region "后台账号锁定/解锁"
        [HttpPost]
        public JsonResult UpdateBackgroundUserLockState(bool isLock, params long[] userIds)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var parameter = new Dictionary<string, string>(){
                { "userIds", string.Join(",", userIds) },
                {"isLock",isLock.ToString()}
            };

            var result = PostStandardWithControllerAction<object>("User", "UpdateBackgroundUserLockState", parameter);
            return new JsonResultEx(result);
        }
        #endregion

        #region "导出注册用户列表为Excel"
        [HttpGet]
        [Validate]
        public ActionResult ExportExcelWithUserList(SearchUserListInputDto input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            RestSharp.RestClient client = new RestSharp.RestClient();
            RestSharp.RestRequest request = new RestSharp.RestRequest();
            client.BaseUrl = new Uri(GetCurrentUrl(this));
            WhereModel wheremodel;
            wheremodel = new WhereModel()
            {
                RoleId = Convert.ToInt32(Request["RoleId"].DefaultIfEmpty("0")),
                Province = Request["Province"].DefaultIfEmpty("0"),
                City = Request["City"].DefaultIfEmpty("0"),
                Region = Request["Region"].DefaultIfEmpty("0"),
                PhoneNumber = Request["PhoneNumber"].DefaultIfEmpty(string.Empty),

            };
            Dictionary<string, string> parameters;
            GetSearchParameters(input.pageIndex.Value, input.pageSize.Value, out wheremodel, out parameters);
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

        #region "导出后台用户列表为Excel"
        [HttpGet]
        [Validate]
        public ActionResult ExportExcelWithBackgroundUserList(SearchBackgroundUserListInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            RestSharp.RestClient client = new RestSharp.RestClient();
            RestSharp.RestRequest request = new RestSharp.RestRequest();
            client.BaseUrl = new Uri(GetCurrentUrl(this));

            foreach (var para in ModelHelper.GetPropertyDictionary<SearchBackgroundUserListInput>(input))
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

        #endregion "Public Methods"
    }
}