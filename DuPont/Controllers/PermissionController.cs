using DuPont.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Extensions;

using DuPont.Entity.Enum;
using DuPont.Attributes;
using DuPont.Models.Models;
namespace DuPont.Controllers
{
    [CustomHandleErrorWithErrorJson]
    public class PermissionController : BaseController
    {
        private readonly IPermissionProvider permissionProvider;
        public PermissionController(IPermissionProvider permissionProvider)
            : base(permissionProvider)
        {
            this.permissionProvider = permissionProvider;
        }


        [HttpPost]
        public ActionResult CheckPermission(Int64 userId, string url)
        {
            #region 参数验证
            if (userId <= 0)
            {
                throw new ArgumentException("userId");
            }

            if (url.IsNullOrEmpty())
            {
                throw new ArgumentException("url");
            }
            #endregion

            var havePermission = this.permissionProvider.HaveAuthority(userId, url);
            using (var result = new ResponseResult<object>())
            {
                result.IsSuccess = havePermission;
                if (havePermission == false)
                {
                    result.State.Id = (int)ResponseStatusCode.AccessDenied;
                    result.Message = ResponseStatusCode.AccessDenied.GetDescription();
                }

                return Json(result);
            }
        }
    }
}