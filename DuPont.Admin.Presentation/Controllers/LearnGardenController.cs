using DuPont.Admin.Presentation.Filters;
using DuPont.Admin.Presentation.Models;
using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Dtos.Background.LearningWorld;
using DuPont.Models.Models;
using DuPont.Utility;
// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-14-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-14-2015
// ***********************************************************************
// <copyright file="LearnGardenController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Admin.Presentation.Controllers
{
    /// <summary>
    /// 学习园地管理
    /// </summary>
#if (!DEBUG)
    [SysAuth]
#endif
    [ValidateInput(false)]
    [Validate]
    public class LearnGardenController : BaseController
    {
        #region "文章列表"
        [HttpGet]
        public ActionResult ArticleList(ArticleListSearchInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var result = PostStandardWithSameControllerAction<List<Article>>(this, ModelHelper.GetPropertyDictionary<ArticleListSearchInput>(input));
            var viewModel = new ListViewModel<Article>(result.IsSuccess, result.PageIndex, result.PageSize, (int)result.TotalNums, result.Entity);
            if (viewModel.Success)
            {
                foreach (var article in result.Entity)
                {
                    article.Content = PageValidate.RemoveHtmlTagAndSepecialChar(article.Content);
                }
            }
            return View(viewModel);
        }
        #endregion

        #region "添加文章"
        [HttpGet]
        public ActionResult AddArticle()
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            ViewBag.PageTitle = "新增文章";
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddArticle(ArticleInput input, string CatName)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            ViewBag.PageTitle = "新增文章";
            if (CheckInputWhenReturnActionResult() == false)
            {
                return View(input);
            }

            var parameters = ModelHelper.GetPropertyDictionary<ArticleInput>(input);

            var responseResult = PostStandardWithSameControllerAction<object>(this, parameters);
            if (responseResult.IsSuccess)
            {
                TempData["Message"] = "添加成功!";

                //静态化处理
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Templates/ArticleDetail.html"));
                var articleHtmlPath = Server.MapPath("~/Articles/");
                if (!Directory.Exists(articleHtmlPath))
                {
                    Directory.CreateDirectory(articleHtmlPath);
                }

                string htmlResult = content.Replace("@PageTitle", input.Title)
                    .Replace("@Title", input.Title)
                    .Replace("@CatName", CatName)
                    .Replace("@CreateTime", DateTime.Now.ToString("yyyy.MM.dd"))
                    .Replace("@UpdateTime", DateTime.Now.ToString("yyyy.MM.dd"))
                    .Replace("@Content", input.Content);

                System.IO.File.WriteAllText(articleHtmlPath + "article_" + responseResult.Entity + ".html", htmlResult, Encoding.UTF8);

                //.Replace("@Click",input)

                return RedirectToAction("ArticleList");
            }
            else
            {
                TempData["Error"] = responseResult.Message;
                return View(input);
            }
        }
        #endregion

        #region "编辑文章"
        [HttpGet]
        public ActionResult EditArticle(long articleId)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            return View();
        }

        [HttpPost]
        public ActionResult EditArticle(ArticleInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            return View();
        }
        #endregion

        #region "查看文章详情"
        [HttpGet]
        public ActionResult Article(long id)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var result = PostStandardWithSameControllerAction<Article>(this, new Dictionary<string, string>() { { "id", id.ToString() } });
            return View(result.Entity);
        }
        #endregion

        #region "删除文章"
        [HttpPost]
        public int DeleteArticle(params int[] articleId)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            if (articleId != null)
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("articleIds", string.Join(",", articleId));

                var result = PostStandardWithSameControllerAction<object>(this, parameters);

                if (result.IsSuccess)
                {
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        #region "删除示范农场"
        [HttpPost]
        public int DeleteFarm(params int[] farmId)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            if (farmId != null)
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("farmIds", string.Join(",", farmId));

                var result = PostStandardWithSameControllerAction<object>(this, parameters);

                if (result.IsSuccess)
                {
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        #region "添加文章到轮播图"
        [HttpPost]
        public int AddArticleToCarousel(params int[] articleId)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            if (articleId != null)
            {
                var parameters = new Dictionary<string, string>();
                parameters.Add("articleIds", string.Join(",", articleId));

                var result = PostStandardWithSameControllerAction<object>(this, parameters);

                if (result.IsSuccess)
                {
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        #region "添加示范农场"
        [HttpGet]
        public ActionResult AddDemonstateFarm()
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            return View();
        }

        [HttpPost]
        public ActionResult AddDemonstateFarm(DemonstateFarmDetailViewModel input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var parameter = ModelHelper.GetPropertyDictionary<DemonstateFarmDetailViewModel>(input);
            var result = PostStandardWithSameControllerAction<object>(this, parameter);
            if (result.IsSuccess)
            {
                TempData["Message"] = "添加成功!";
                return RedirectToAction("FarmList");
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return View(input);
        }
        #endregion

        #region "编辑示范农场"
        [HttpGet]
        public ActionResult EditDemonstateFarm(int farmId)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var parameter = new Dictionary<string, string>{
                {"farmId",farmId.ToString()}
            };
            var result = PostStandardWithControllerAction<DemonstateFarmDetailViewModel>("LearnGarden", "DemonstateFarmDetail", parameter);
            if (result.IsSuccess)
            {
                return View(result.Entity);
            }
            else
            {
                return Content("<script>alert('" + result.Message + "');history.go(-1)</script>");
            }
        }

        [HttpPost]
        public ActionResult EditDemonstateFarm(DemonstateFarmDetailViewModel input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            var parameter = ModelHelper.GetPropertyDictionary<object>(input);
            var result = PostStandardWithControllerAction<object>("LearnGarden", "UpdateDemonstateFarmDetail", parameter);
            if (result.IsSuccess)
            {
                TempData["Message"] = "修改成功!";
                return RedirectToAction("FarmList");
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return View(input);
        }
        #endregion

        #region "添加示范农场展区"
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult AddExhibitionAreaForDemonstateFarm()
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddExhibitionAreaForDemonstateFarm(FarmAreaInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            if (CheckInputWhenReturnActionResult() == false)
            {
                return View(input);
            }

            var parameters = ModelHelper.GetPropertyDictionary<FarmAreaInput>(input);

            var responseResult = PostStandardWithSameControllerAction<object>(this, parameters);
            if (responseResult.IsSuccess)
            {
                TempData["Message"] = "添加成功!";

                //静态化处理
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Templates/FarmAreaDetail.html"));
                var articleHtmlPath = Server.MapPath("~/Articles/");
                if (!Directory.Exists(articleHtmlPath))
                {
                    Directory.CreateDirectory(articleHtmlPath);
                }

                string htmlResult = content.Replace("@PageTitle", input.Name)
                    .Replace("@Title", input.Name)
                    .Replace("@CreateTime", DateTime.Now.ToString("yyyy.MM.dd"))
                    .Replace("@Content", input.Content);

                System.IO.File.WriteAllText(articleHtmlPath + "article_farm_area_" + responseResult.Entity + ".html", htmlResult, Encoding.UTF8);

                return View(input);
            }
            else
            {
                TempData["Error"] = responseResult.Message;
                return View(input);
            }
        }
        #endregion

        #region "示范农场列表"
        [HttpGet]
        public ActionResult FarmList(FarmListInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            if (input.PageIndex == 0)
            {
                input.PageIndex = 1;
                input.PageSize = 10;
            }

            var parameter = ModelHelper.GetPropertyDictionary<FarmListInput>(input);
            var result = PostStandardWithSameControllerAction<List<FarmListOutput>>(this, parameter);
            if (result.IsSuccess)
            {
                var model = new MultiModel<List<FarmListOutput>>(result.IsSuccess, input.PageIndex, input.PageSize, (int)result.TotalNums, result.Entity);
                return View(model);
            }

            return View();
        }
        #endregion

        #region "示范农场报名列表"
        [HttpGet]
        public ActionResult FarmBookList(FarmBookListInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            if (input.PageIndex == 0)
            {
                input.PageIndex = 1;
                input.PageSize = 10;
            }

            var parameter = ModelHelper.GetPropertyDictionary<FarmBookListInput>(input);
            var result = PostStandardWithSameControllerAction<FarmBookListOutput>(this, parameter);
            if (result.IsSuccess)
            {
                var model = new MultiModel<FarmBookListOutput>(result.IsSuccess, input.PageIndex, input.PageSize, (int)result.TotalNums, result.Entity);
                return View(model);
            }

            return View();
        }
        #endregion

        #region "示范农场展区列表"
        [HttpPost]
        public JsonResult ExhibitionAreaList(int farmId)
        {
            var parameter = new Dictionary<string, string>
            {
                {"farmId",farmId.ToString()}
            };
            var result = PostStandardWithSameControllerAction<List<ExhibitionAreaListOutput>>(this, parameter);
            return new JsonResultEx(result);
        }
        #endregion

        #region "删除示范农场展区"
        [HttpPost]
        public JsonResult DeleteExhibitionArea(int farmId, int areaId)
        {
            var parameter = new Dictionary<string, string>
            {
                {"farmId",farmId.ToString()},
                {"areaId",areaId.ToString()}
            };

            var result = PostStandardWithSameControllerAction<object>(this, parameter);
            return new JsonResultEx(result);
        }
        #endregion

    }
}