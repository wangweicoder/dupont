
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DuPont.Global.ActionResults
{
    public class NotLoginResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Redirect("~/account/login");
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.End();
        }
    }
}
