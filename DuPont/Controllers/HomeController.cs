using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return Json(new
            {
                Location = "Admin_Api",
                State = "ok"
            },JsonRequestBehavior.AllowGet);
        }
    }
}