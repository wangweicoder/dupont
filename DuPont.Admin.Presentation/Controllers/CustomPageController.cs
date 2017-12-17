using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
    /// <summary>
    /// 自定义错误处理
    /// </summary>
    public class CustomPageController : Controller
    {
        // GET: CustomPage
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 404错误界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Page404()
        {
            return View();
        }

        /// <summary>
        /// 500错误界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Page500()
        {
            return View();
        }

        /// <summary>
        /// 其它错误界面
        /// </summary>
        /// <returns></returns>
        public ActionResult PageDefault()
        {
            return View();
        }
    }
}