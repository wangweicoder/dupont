using AutoMapper;
using DuPont.Global.ActionResults;
using DuPont.Global.Exceptions;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using DuPont.Models.Dtos.Background.LearningWorld;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Controllers
{
    [Validate]
    public class ArticleCategoryController : BaseController
    {
        private readonly IArticleCategory _articleCategoryService;
        private readonly IArticle _articleService;
        public ArticleCategoryController(IPermissionProvider permissionProvider,
            IAdminUser adminUserRepository, IArticleCategory articleCategoryService, IArticle articleService)
            : base(permissionProvider, adminUserRepository)
        {
            _articleCategoryService = articleCategoryService;
            _articleService = articleService;
        }

        #region "获取文章分类列表"
        [HttpPost]
        public JsonResult List()
        {
            CheckPermission();
            using (var result = new ResponseResult<List<ArticleCategory>>())
            {
                var categoryList = _articleCategoryService.GetAll(m=>m.Name!="示范农场");
                result.Entity = Mapper.Map<List<ArticleCategory>>(categoryList);
                result.TotalNums = result.Entity.Count;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "添加文章分类"
        [HttpPost]
        public JsonResult Add(ArticleCategory input)
        {
            CheckPermission();

            using (var result = new ResponseResult<object>())
            {
                var articleCategory = Mapper.Map<T_ARTICLE_CATEGORY>(input);
                _articleCategoryService.Insert(articleCategory);
                result.Entity = articleCategory.CategoryId;
                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "修改文章分类"
        [HttpPost]
        public JsonResult Edit(ArticleCategory input)
        {
            CheckPermission();

            using (var result = new ResponseResult<object>())
            {
                var articleCategory = Mapper.Map<T_ARTICLE_CATEGORY>(input);
                var effectCount = _articleCategoryService.Update(articleCategory);
                result.Entity = effectCount;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "获取指定分类"
        public JsonResult Detail(int catId)
        {
            CheckPermission();

            using (var result = new ResponseResult<ArticleCategory>())
            {
                var articleCategory = _articleCategoryService.GetByKey(catId);
                result.Entity = Mapper.Map<ArticleCategory>(articleCategory);
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "删除文章分类"
        [HttpPost]
        public JsonResult Delete(int catId)
        {
            CheckPermission();

            using (var result = new ResponseResult<object>())
            {
                var hasArticlesUnderThisCategory = _articleService.Count(m => m.CatId == catId) > 0;
                if (hasArticlesUnderThisCategory)
                    throw new CustomException("该分类下有文章,不可删除!");

                var effectCount = _articleCategoryService.Delete("CategoryId", catId);
                result.Entity = effectCount;
                return new JsonResultEx(result);
            }
        }
        #endregion


    }
}