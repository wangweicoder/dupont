using DuPont.Attributes;
using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Web.Mvc;

namespace DuPont.Controllers
{
    [CustomHandleErrorWithErrorJson]
    public class APPManageController : BaseController
    {
        private IApp_Version _appVersionRepository;
        public APPManageController(IPermissionProvider permissionManage, IApp_Version appVersionRepository)
            : base(permissionManage)
        {
            this._appVersionRepository = appVersionRepository;
        }

        public JsonResult UploadAppFile(string versionCode, string version, string platform, string isOpen, string changeLog, string filePath, string userId)
        {
            using (ResponseResult<Object> result = new ResponseResult<Object>())
            {
                try
                {
                    bool SaveRes = _appVersionRepository.SaveAppFile(new T_APP_VERSION
                    {
                        Version = version,
                        VersionCode = int.Parse(versionCode),
                        DownloadURL = filePath,
                        ChangeLog = changeLog,
                        CREATE_USER = int.Parse(userId),
                        CREATE_DATE = DateTime.Now,
                        UPDATE_USER = int.Parse(userId),
                        UPDATE_DATE = DateTime.Now,
                        Platform = platform,
                        IsOpen = Convert.ToBoolean(isOpen)
                    });

                    if (SaveRes)
                    {
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "数据库更新失败";
                    }
                }
                catch (Exception)
                {

                }
                return Json(result);
            }
        }
    }
}