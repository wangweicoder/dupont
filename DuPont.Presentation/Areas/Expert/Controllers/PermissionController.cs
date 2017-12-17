using DuPont.Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Dtos.Foreground.Expert;

namespace DuPont.Presentation.Areas.Expert.Controllers
{
    [Validate]
    public class PermissionController : BaseController
    {

        #region "专家权限检查"
        [HttpPost]
        public ActionResult Check(long UserId)
        {
            var parameters = GetPostParameters();
            var responseResult = PostStandardWithSameControllerAction<ExpertPermissionCheckOutput>(this, parameters);
            return new JsonResultEx(responseResult);
        } 
        #endregion


    }
}
