
using DuPont.Admin.Presentation.Attributes;
using DuPont.Extensions;
using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Models.Dtos.Background.Question;
using DuPont.Models.Dtos.Foreground.Expert;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace DuPont.Admin.Presentation.Controllers
{
     [Validate] 
    public class QuestionController : BaseController
    {


        #region "问题列表"
        [HttpGet]
        public ActionResult List(SearchQuestionInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);

            if (input.PageIndex == 0)
                input.PageIndex = 1;

            if (input.PageSize == 0)
                input.PageSize = 10;

            var parameter = ModelHelper.GetPropertyDictionary<SearchQuestionInput>(input);
            var result = PostStandardWithSameControllerAction<List<SearchQutionOutput>>(this, parameter);

            var model = new MultiModel<List<SearchQutionOutput>>(result.IsSuccess, input.PageIndex, input.PageSize, (int)result.TotalNums, result.Entity);
            return View(model);
        }
        #endregion

        #region "公开问题"
        public JsonResult Open(params long[] Ids)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl, true);
            var result = PostStandardWithSameControllerAction<object>(this,
                new Dictionary<string, string>() { { "Ids", string.Join(",", Ids) } });
            return new JsonResultEx(result);
        }
        #endregion

        #region "取消公开问题"
        public JsonResult CancelOpen(params long[] Ids)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl, true);
            var result = PostStandardWithSameControllerAction<object>(this,
                new Dictionary<string, string>() { { "Ids", string.Join(",", Ids) } });
            return new JsonResultEx(result);
        }
        #endregion

        #region "删除问题"
        public JsonResult Delete(params long[] Ids)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl, true);
            var result = PostStandardWithSameControllerAction<object>(this,
                new Dictionary<string, string>() { { "Ids", string.Join(",", Ids) } });
            return new JsonResultEx(result);
        }
        #endregion

        #region "回复列表"
        public JsonResult ReplyList(ExpertQuestionReplyListSearchInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl, true);
            var parameters = GetPostParameters();
            var responseResult = PostStandardWithSameControllerAction<List<ExpertQuestionReplyListOutput>>(this, parameters);
            return new JsonResultEx(responseResult);
        }
        #endregion

        #region "问题详情"
        [HttpGet]
        public ActionResult Detail(long questionId, int PageIndex = 1, int replyPageSize = 10)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"questionId",questionId.ToString()},
                {"replyPageIndex",PageIndex.ToString()},
                {"replyPageSize",replyPageSize.ToString()}
            };

            var responseResult = PostStandardWithSameControllerAction<QuestionDetailWithReplyListOutput>(this, parameters);
            var model = new SingleModel<QuestionDetailWithReplyListOutput>
            {
                Data = responseResult.Entity,
                IsSuccess = responseResult.IsSuccess
            };

            ViewBag.PageIndex = PageIndex;
            ViewBag.PageSize = replyPageSize;
            ViewBag.RecordCount = responseResult.TotalNums;
            return View(model);
        }
        #endregion

        #region "添加到轮播"
        [HttpPost]
        public JsonResult AddToCarousel(params long[] Ids)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl, true);
            var result = PostStandardWithSameControllerAction<object>(this,
                new Dictionary<string, string>() { { "Ids", string.Join(",", Ids) } });
            return new JsonResultEx(result);
        }
        #endregion

        #region "取消轮播"
        [HttpPost]
        public JsonResult CancelCarousel(params long[] Ids)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl, true);
            var result = PostStandardWithSameControllerAction<object>(this,
                new Dictionary<string, string>() { { "Ids", string.Join(",", Ids) } });
            return new JsonResultEx(result);
        }
        #endregion

        #region "导出问题列表为Excel"
        [HttpGet]
        public ActionResult ExportExcelWithQuestionList(SearchQuestionInput input)
        {
            CheckPermission(GetLoginInfo().User.Id, CurrentUrl);
            RestSharp.RestClient client = new RestSharp.RestClient();
            RestSharp.RestRequest request = new RestSharp.RestRequest();
            client.BaseUrl = new Uri(GetCurrentUrl(this));

            foreach (var para in ModelHelper.GetPropertyDictionary<SearchQuestionInput>(input))
            {
                request.AddParameter(para.Key, para.Value.IsNullOrEmpty() ? null : para.Value);
            }

            if (request.Parameters.Count(p => p.Name == DataKey.UserId) == 0)
            {
                request.AddParameter(DataKey.UserId, GetLoginInfo().User.Id.ToString());
            }

            var responseResult = client.ExecuteAsGet(request, "GET");
            if (responseResult.Content == "no data")
            {
                return Content("<script>alert('没有符合条件的数据可被导出!');history.go(-1)</script>");
            }
            var contentDispositionHeader = responseResult.Headers.First(p => p.Name == "Content-Disposition").Value.ToString().Replace(" ", string.Empty);
            var attachFileName = contentDispositionHeader.Replace("attachment;filename=", string.Empty);
            return File(responseResult.RawBytes, responseResult.ContentType, attachFileName);
        }
        #endregion
    }
}