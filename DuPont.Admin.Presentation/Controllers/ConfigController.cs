using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Dtos.Background.Config;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
     [Validate]   
    public class ConfigController : BaseController
    {

        [HttpGet]
        public ActionResult List()
        {
            CheckPermission(GetLoginInfo().User.Id, GetCurrentUrl(this));
            var result = PostStandardWithSameControllerAction<List<SystemSettingViewModel>>(this);
            if (result.IsSuccess)
            {
                return View(result.Entity);
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            CheckPermission(GetLoginInfo().User.Id, GetCurrentUrl(this));
            var parameter=new Dictionary<string,string>(){
                {"id",id.ToString()}
            };
            var result = PostStandardWithSameControllerAction<SystemSettingViewModel>(this, parameter);
            if (result.IsSuccess)
            {
                return View(result.Entity);
            }
            else
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public ActionResult Update(SystemSettingViewModel input)
        {
            CheckPermission(GetLoginInfo().User.Id, GetCurrentUrl(this));
            var parameters = ModelHelper.GetPropertyDictionary<SystemSettingViewModel>(input);
            var result = PostStandardWithSameControllerAction<object>(this, parameters);
            if (result.IsSuccess)
            {
                TempData["Message"] = result.Message;
                return RedirectToAction("List");
            }
            else
            {
                TempData["Error"] = result.Message;
                return View("Detail", input);
            }
        }
    }
}