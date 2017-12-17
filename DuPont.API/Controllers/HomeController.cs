using DuPont.API.Filters;

using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.API.Controllers
{
#if(!DEBUG)
     [AccessAuthorize]
#endif
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public JsonResult TestApi(Int64 cur_time, string encrypt)
        {

            return new JsonResult();
        }
    }
}
