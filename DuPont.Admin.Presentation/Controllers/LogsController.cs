using DuPont.Admin.Presentation.Models;
using DuPont.Interface;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public class LogsController : BaseController
    {

        // GET: Logs
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 日志详情界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Detailed()
        {
            return View();
        }

        /// <summary>
        /// 获取请求的api地址
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string GetPostUrl(string methodName)
        {
            return bgApiServerUrl + this.GetType().Name.Replace("Controller", "") + "/" + methodName;
        }

        /// <summary>
        /// 日志列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult List(int pageIndex = 1, int pageSize = 10)
        {

            var userId = GetLoginInfo().User.Id;
            //请求的地址
            string postUrl = GetPostUrl("List");
            //请求的参数

            var postParas = new Dictionary<string, string>() 
                { 
                    {"pageIndex",pageIndex.ToString()},
                    {"pageSize",pageSize.ToString()},
                    {"userId",userId.ToString()}
                };
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            if (postParas.ContainsKey(DataKey.UserId) == false)
            {
                postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }
            var responseStrResult = HttpAsynchronousTool.CustomHttpWebRequestPost(postUrl, postParas, certification, certificationPwd);
            var responseObjResult = JsonHelper.FromJsonTo<ResponseResult<ListLogViewModelWithoutPager>>(responseStrResult);
            if (responseObjResult.IsSuccess)
            {
                foreach (var item in responseObjResult.Entity.listModel)
                {
                    item.CreateTime = TimeHelper.GetChinaLocalTime(item.CreateTime);
                }
                var logVerification = new ListLogViewModel();
                logVerification.Pager = new PagedList<string>(new string[0], responseObjResult.PageIndex, responseObjResult.PageSize, (int)responseObjResult.TotalNums);
                logVerification.listModel = responseObjResult.Entity.listModel;
                return View(logVerification);
            }
            else
            {
                TempData["Error"] = responseObjResult.Message;
                return null;
            }

        }
        /// <summary>
        /// 详细点击事件
        /// </summary>
        /// <param name="verificationId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult Detail(LogModel Model)
        {
            var auditUser = GetLoginInfo();
            if (auditUser == null)
            {
                return null;
            }
            return View("Detailed", Model);
        }
    }
}