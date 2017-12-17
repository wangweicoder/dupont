

using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Dtos.Background.LearningWorld;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
    [Validate]   
    public class DemandGetController : BaseController
    {
        /// <summary>
        /// 获取请求的api地址
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string GetPostUrl(string methodName)
        {
            return bgApiServerUrl + this.GetType().Name.Replace("Controller", "") + "/" + methodName;
        }
        // GET: DemandGet
        [HttpGet]
        public ActionResult SelectList(string code)
        {
            var responseResult = PostStandardWithControllerAction<List<ArticleCategory>>("DemandGet","DictionaryList",
                 new Dictionary<string, string> { { "code", code } });
            return new JsonResultEx(responseResult, true);
        }

    }
}