


using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Dtos.Foreground.LearningWorld;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Presentation.Controllers
{
    [Validate]
    public class LearningWorldController : BaseController
    {
        #region "获取导航菜单列表"
        [HttpPost]
        public JsonResult NavMenu()
        {
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<List<NavigateMenuItemOutput>>>(GetCurrentUrl(this), null, GetCertificationFilePath(), GetCertificationPwd());
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "获取轮播图片列表"
        [HttpPost]
        public JsonResult CarouselPictures(CarouselPictureSearchInput input)
        {
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<List<CarouselPictureOutput>>>(GetCurrentUrl(this), GetPostParameters(), GetCertificationFilePath(), GetCertificationPwd());
            if (responseResult.IsSuccess)
            {
                if (responseResult.IsSuccess && responseResult.Entity != null)
                {
                    var articBasePath = ConfigHelper.GetAppSetting(DataKey.ArticleStaticPageBasePath);
                    foreach (var article in responseResult.Entity)
                    {
                        article.ArticleUrl = articBasePath + article.ArticleUrl;
                    }
                }
            }
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "获取文章列表"
        [HttpPost]
        public JsonResult ArticleList(ArticleListSearchInput input)
        {
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<List<ArticleListSearchOutput>>>(GetCurrentUrl(this), GetPostParameters(), GetCertificationFilePath(), GetCertificationPwd());
            if (responseResult.IsSuccess && responseResult.Entity != null)
            {
                var articleBaseHost = ConfigHelper.GetAppSetting(DataKey.ArticleStaticPageBasePath);
                foreach (var article in responseResult.Entity)
                {

                    article.Url = articleBaseHost + article.Url;
                    if (!(article.PictureUrl.IndexOf(Request.Url.Authority) > -1))
                    {
                        article.PictureUrl = "";
                    }
                }
            }
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "文章统计"
        [HttpPost]
        public JsonResult Article_Statistics(long ArticleId)
        {
            var responseResult = RestSharpHelper.PostWithStandard<ResponseResult<object>>(GetCurrentUrl(this), GetPostParameters(), GetCertificationFilePath(), GetCertificationPwd());
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "农场列表"
        [HttpPost]
        public JsonResult FarmList(FarmListInput input)
        {
            var parameter = ModelHelper.GetPropertyDictionary<FarmListInput>(input);
            var result = PostStandardWithSameControllerAction<List<FarmListOutput>>(this, parameter);
            return new JsonResultEx(result);
        }
        #endregion

        #region "农场详情"
        [HttpPost]
        public JsonResult FarmDetail(FarmDetailInput input)
        {
            var parameter = ModelHelper.GetPropertyDictionary<FarmDetailInput>(input);
            var result = PostStandardWithSameControllerAction<FarmDetailOutput>(this, parameter);
            if (!result.IsSuccess || result.Entity == null || result.Entity.AreaList == null)
                return new JsonResultEx(result);
            var articleBaseHost = ConfigHelper.GetAppSetting(DataKey.ArticleStaticPageBasePath);
            foreach (var area in result.Entity.AreaList.Where(area => !string.IsNullOrEmpty(area.Url)))
            {
                area.Url = articleBaseHost + area.Url;
                if (Request.Url != null && !(area.Url.IndexOf(Request.Url.Authority, StringComparison.Ordinal) > -1))
                {
                    area.Url = "";
                }
            }
            return new JsonResultEx(result);
        }
        #endregion

        #region "农场预约"
        [HttpPost]
        public JsonResult FarmBooking(FarmBookingInput input)
        {
            var parameter = ModelHelper.GetPropertyDictionary<FarmBookingInput>(input);
            var result = PostStandardWithSameControllerAction<object>(this, parameter);
            return new JsonResultEx(result);
        }
        #endregion

    }
}
