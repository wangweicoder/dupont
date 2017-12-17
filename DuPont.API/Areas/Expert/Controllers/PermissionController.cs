using DuPont.API.Filters;
using DuPont.Global.ActionResults;
using DuPont.Interface;
using DuPont.Models.Dtos.Foreground.Expert;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.API.Areas.Expert.Controllers
{

#if(!DEBUG)
    [AccessAuthorize]
#endif
    public class PermissionController : Controller
    {
        private IExpertPermission _expertPermissionService;
        public PermissionController(IExpertPermission expertPermissionService)
        {
            _expertPermissionService = expertPermissionService;
        }

        //
        // GET: /Expert/Permission/
        public ActionResult Check(long UserId)
        {
            var isExpert = _expertPermissionService.Count(m => m.IsEnabled && m.UserId == UserId);

            using (var result = new ResponseResult<ExpertPermissionCheckOutput>())
            {
                result.Entity.IsExpert = isExpert;

                return new JsonResultEx(result);
            }
        }
    }
}