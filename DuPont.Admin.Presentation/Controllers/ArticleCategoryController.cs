using DuPont.Admin.Presentation.Filters;



using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Utility;
using System.Collections.Generic;
using System.Web.Mvc;
using DuPont.Global.Exceptions;
using DuPont.Models.Dtos.Background.LearningWorld;
using DuPont.Models.Enum;
using DuPont.Models.Models;

namespace DuPont.Admin.Presentation.Controllers
{

    [Validate]
    public class ArticleCategoryController : BaseController
    {
        #region "文章分类列表"
        [SysAuth]
        [HttpGet]
        public ActionResult List()
        {
            var responseResult = PostStandardWithSameControllerAction<List<ArticleCategory>>(this);
            var viewModel = new MultiModel<List<ArticleCategory>>(responseResult.IsSuccess, responseResult.PageIndex, responseResult.PageSize, (int)responseResult.TotalNums, responseResult.Entity);
            return View(viewModel);
        }

        [SysAuth]
        [HttpGet]
        public JsonResult ListJson()
        {
            var responseResult = PostStandardWithControllerAction<List<ArticleCategory>>("ArticleCategory", "List", null);
            return new JsonResultEx(responseResult, true);
        }
        #endregion

        #region "添加文章分类"
        [SysAuth]
        [HttpGet]
        public ActionResult Add()
        {
            var viewModel = new SingleModel<ArticleCategory>()
             {
                 ActionType = ActionType.Add
             };
            return View(viewModel);
        }

        [SysAuth]
        [HttpPost]
        public ActionResult Add(ArticleCategory articleCategory)
        {
            if (!CheckInputWhenReturnActionResult())
                return RedirectToAction("Add");

            var responseResult = PostStandardWithSameControllerAction<object, ArticleCategory>(this,
                articleCategory);
            if (responseResult.IsSuccess)
            {
                TempData["Message"] = "添加成功!";
                return RedirectToAction("List");
            }

            TempData["Error"] = responseResult.Message;
            return View(articleCategory);
        }
        #endregion

        #region "编辑文章分类"
        [SysAuth]
        [HttpGet]
        public ActionResult Edit(int CatId)
        {
            var responseResult = PostStandardWithControllerAction<ArticleCategory>(
                "ArticleCategory", "Detail",
                new Dictionary<string, string> { { "catId", CatId.ToString() } });
            if (responseResult.IsSuccess)
            {
                var viewModel = new SingleModel<ArticleCategory>()
                 {
                     IsSuccess = true,
                     ActionType = ActionType.Edit,
                     Data = responseResult.Entity
                 };

                return View("Add", viewModel);
            }

            TempData["Error"] = responseResult.Message;
            return RedirectToAction("List");
        }

        [SysAuth]
        [HttpPost]
        public ActionResult Edit(ArticleCategory input)
        {
            var responseResult = PostStandardWithSameControllerAction<object, ArticleCategory>(this,
                input);

            if (responseResult.IsSuccess)
            {
                TempData["Message"] = "修改成功!";
                return RedirectToAction("List");
            }

            TempData["Error"] = responseResult.Message;
            return View(input);
        }
        #endregion

        #region "删除文章分类"
        [SysAuth]
        [HttpGet]
        public ActionResult Delete(int CatId)
        {
            if (CatId == 1)
                throw new CustomException("默认分类不可删除!");

            var responseResult = PostStandardWithSameControllerAction<object>(this,
                new Dictionary<string, string> { { "catId", CatId.ToString() } });

            if (!responseResult.IsSuccess)
            {
                TempData["Error"] = responseResult.Message;
                return RedirectToAction("List");
            }

            TempData["Message"] = "删除成功!";
            return RedirectToAction("List");
        }
        #endregion

    }
}