

using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
     [Validate]
    /// <summary>
    /// APP管理
    /// </summary>
    public class AppManageController : BaseController
    {

        /// <summary>
        /// APP管理首页界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上传APP文件界面
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult UploadAppFile(int state = 0)
        {
            ViewBag.State = state;
            return View();
        }

        /// <summary>
        /// 上传APP文件
        /// </summary>
        /// <param name="versionCode"></param>
        /// <param name="version"></param>
        /// <param name="platform"></param>
        /// <param name="isOpen"></param>
        /// <param name="changeLog"></param>
        /// <param name="appFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAppFile(string versionCode, string version, string platform, string isOpen, string changeLog, HttpPostedFileBase appFile)
        {
            try
            {
                string folder = Server.MapPath("~/") + @"uploadfiles";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string currentFolder = folder + @"\" + GetFolderNameByDate();
                string path = "uploadfiles/" + GetFolderNameByDate();
                if (!Directory.Exists(currentFolder))
                {
                    Directory.CreateDirectory(currentFolder);
                }

                //string extensionName = appFile.FileName.Split('.').LastOrDefault();


                //string guid = Guid.NewGuid().ToString();
                //string fileName = guid + "." + extensionName;
                string fileName = appFile.FileName;
                string fileBasePath = currentFolder + @"\";
                appFile.SaveAs(fileBasePath + fileName);

                //获得当前登录用户信息
                var logInfo = base.GetLoginInfo();

                //插入数据库
                Dictionary<string, string> postParas = new Dictionary<string, string>();

                postParas.Add("version", version);
                postParas.Add("versionCode", versionCode);
                postParas.Add("platform", platform);
                postParas.Add("isOpen", isOpen);
                postParas.Add("changeLog", changeLog);
                postParas.Add("filePath", fileBasePath + fileName);
                postParas.Add("userId", logInfo.User.Id.ToString());

                //证书的路径
                var certificationUrl = GetCertificationFilePath();
                //证书的密码
                var certificationPwd = GetCertificationPwd();

                if (postParas.ContainsKey(DataKey.UserId) == false)
                {
                    postParas.Add(DataKey.UserId, GetLoginInfo().User.Id.ToString());
                }

                var res = HttpAsynchronousTool.CustomHttpWebRequestPost(bgApiServerUrl + "AppManage/UploadAPPFile", postParas, null, null);

                var resObj = JsonHelper.FromJsonTo<ResponseResult<Object>>(res);

                if (resObj.IsSuccess)
                {
                    return RedirectToAction("UploadAppFile", new { state = 1 });
                    //成功
                }
                else
                {
                    return RedirectToAction("UploadAppFile", new { state = 2 });
                    //失败
                }
            }
            catch (Exception)
            {
                return null;
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
    }
}