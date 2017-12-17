

using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Dtos.Foreground.Expert;
using DuPont.Models.Enum;
using DuPont.Presentation.Controllers;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Presentation.Areas.Expert.Controllers
{
    [Validate]
    public class QuestionController : BaseController
    {
        public static readonly string uploadFileBasePath = ConfigHelper.GetAppSetting(DataKey.ArticleStaticPageBasePath) + "/api/";

        #region "创建问题"
        [HttpPost]
        public ActionResult Create(ExpertQuestionInput question)
        {
            var parameters = GetPostParameters();

            var responseResult = PostStandardWithSameControllerAction<object>(this, parameters);
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "问题列表"
        [HttpPost]
        public JsonResult List(ExpertQuestionSearchInput searchInput)
        {
            var parameters = GetPostParameters();

            var responseResult = PostStandardWithSameControllerAction<List<ExpertQuestionListOutput>>(this, parameters);
            if (responseResult.IsSuccess && responseResult.Entity.Count > 0)
            {
                foreach (var item in responseResult.Entity)
                {
                    if (!string.IsNullOrEmpty(item.PictureUrl))
                    {
                        item.PictureUrl = uploadFileBasePath + item.PictureUrl;
                    }
                }
            }
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "问题详情"
        [HttpPost]
        public JsonResult Detail(long QuestionId)
        {
            var parameters = GetPostParameters();

            var responseResult = PostStandardWithSameControllerAction<ExpertQuestionDetailOutput>(this, parameters);
            if (responseResult.IsSuccess && responseResult.Entity.PictureUrlList.Count > 0)
            {
                for (int i = 0; i < responseResult.Entity.PictureUrlList.Count; i++)
                {
                    responseResult.Entity.PictureUrlList[i] = uploadFileBasePath + responseResult.Entity.PictureUrlList[i];
                }
            }
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "回复列表"
        [HttpPost]
        public JsonResult ReplyList(ExpertQuestionReplyListSearchInput input)
        {
            var parameters = GetPostParameters();
            var responseResult = PostStandardWithSameControllerAction<List<ExpertQuestionReplyListOutput>>(this, parameters);
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "提问列表"
        [HttpPost]
        public JsonResult MyList(ExpertMyQuestionListInput input)
        {
            var parameters = GetPostParameters();
            var responseResult = PostStandardWithSameControllerAction<List<ExpertMyQuestionListOutput>>(this, parameters);
            if (responseResult.IsSuccess && responseResult.Entity.Count > 0)
            {
                foreach (var item in responseResult.Entity)
                {
                    if (!string.IsNullOrEmpty(item.PictureUrl))
                    {
                        item.PictureUrl = uploadFileBasePath + item.PictureUrl;
                    }
                }
            }
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "我的回答列表"
        [HttpPost]
        public ActionResult MyReplyList(ExpertMyReplyListInput input)
        {
            var parameters = GetPostParameters();
            var responseResult = PostStandardWithSameControllerAction<List<ExpertMyReplyListOutput>>(this, parameters);
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "回答问题"
        [HttpPost]
        public JsonResult Reply(ExpertQuestionReplyInput input)
        {
            var parameters = GetPostParameters();
            var responseResult = PostStandardWithSameControllerAction<object>(this, parameters);
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "对问题点赞"
        [HttpPost]
        public JsonResult PraiseReply(long ReplyId)
        {
            var parameters = GetPostParameters();
            var responseResult = PostStandardWithSameControllerAction<object>(this, parameters);
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "对回复进行采纳（同意）"
        [HttpPost]
        public JsonResult Agree(ExpertQuestionReplyAgreeInput input)
        {
            var parameters = GetPostParameters();
            var responseResult = PostStandardWithSameControllerAction<object>(this, parameters);
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "问题列表轮播图接口"
        [HttpPost]
        public JsonResult CarouselPictures(long UserId)
        {
            var parameters = GetPostParameters();

            var responseResult = PostStandardWithSameControllerAction<List<ExpertQuestionCarouselOutput>>(this, parameters);
            if (responseResult.IsSuccess && responseResult.Entity.Count > 0)
            {
                foreach (var item in responseResult.Entity)
                {
                    if (!string.IsNullOrEmpty(item.PictureUrl))
                    {
                        item.PictureUrl = uploadFileBasePath + item.PictureUrl;
                    }
                }
            }
            return new JsonResultEx(responseResult);
        }

        #endregion
    }
}
