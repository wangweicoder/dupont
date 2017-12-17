

using DuPont.Global.ActionResults;
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
    public class DemandGetController : BaseController
    {
        private IArticleCategory _articleCategoryService;
        private ISys_Dictionary _sys_dictionary;
        public DemandGetController(IPermissionProvider permissionProvider,
            IAdminUser adminUserRepository, IArticleCategory articleCategoryService, ISys_Dictionary sys_dictionary)
            : base(permissionProvider, adminUserRepository)
        {
            _articleCategoryService = articleCategoryService;
            _sys_dictionary = sys_dictionary;
        }
        // GET: DemandGet
        [HttpPost]
        public JsonResult DictionaryList(int code)
        {
            using (var result = new ResponseResult<List<ArticleCategory>>())
            {
                var entitys = _sys_dictionary.GetAll().Where(a => a.ParentCode == code).Select(model => new ArticleCategory
                {
                    CatId = model.Code,
                    CatName = model.DisplayName
                }).ToList();
                result.Entity = entitys;
                return new JsonResultEx(result);
            }
        }
    }
}