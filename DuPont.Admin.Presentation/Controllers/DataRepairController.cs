using DuPont.Global.Filters.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
    [Validate]   
    public class DataRepairController : BaseController
    {

        public ActionResult Index()
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            return View();
        }


        /// <summary>
        /// 修复农机手的机器与服务能力的映射关系
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RepairOperatorMachineAndDemandTypeMapping()
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var responseResult = PostStandardWithSameControllerAction<object>(this);
            if (responseResult.IsSuccess)
            {
                return Content("ok");
            }

            return Content(responseResult.Message);

        }

        #region "修复以前注册用户角色等级星数"
        [HttpPost]
        public ActionResult RepairOriginalUserStar()
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var responseResult = PostStandardWithSameControllerAction<object>(this);
            if (responseResult.IsSuccess)
            {
                return Content("ok");
            }

            return Content(responseResult.Message);
        }

        #endregion
    }
}