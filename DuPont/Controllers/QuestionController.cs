using AutoMapper;



using DuPont.Extensions;
using DuPont.Global.ActionResults;
using DuPont.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

using DuPont.Global.Exceptions;
using WebGrease.Css.Extensions;
using DuPont.Models.Dtos.Background.Question;
using DuPont.Models.Models;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace DuPont.Controllers
{
    public class QuestionController : BaseController
    {
        private readonly IExpertQuestion _expertQuestionService;
        private readonly IExpertQuestionReply _expertQuestionReplyService;
        private readonly IUser _userService;
        public QuestionController(IPermissionProvider permissionManage, IAdminUser adminUserRepository,
            IExpertQuestion expertQuestionService, IUser userService, IExpertQuestionReply expertQuestionReplyService)
            : base(permissionManage, adminUserRepository)
        {
            _expertQuestionService = expertQuestionService;
            _userService = userService;
            _expertQuestionReplyService = expertQuestionReplyService;
        }

        #region "问题列表"
        [HttpPost]
        public JsonResult List(SearchQuestionInput input)
        {
            CheckPermission();

            using (var result = new ResponseResult<List<SearchQutionOutput>>())
            {
                var predicate = PredicateBuilder.True<T_QUESTION>();
                if (!string.IsNullOrEmpty(input.Keywords))
                {
                    predicate = predicate.And(m => m.Title.Contains(input.Keywords));
                }

                if (input.IsOpen.HasValue)
                    predicate = predicate.And(m => m.IsOpen == input.IsOpen);

                if (input.IsDeleted.HasValue)
                {
                    predicate = predicate.And(m => m.IsDeleted == input.IsDeleted);
                }

                long totalCount;
                var list = _expertQuestionService.GetAll<DateTime>(predicate, null, m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount);
                result.Entity = Mapper.Map<List<SearchQutionOutput>>(list);
                var enumerable = list as T_QUESTION[] ?? list.ToArray();
                if (enumerable.Any())
                {
                    //提取用户编号
                    var userIdList = enumerable.Select(m => m.UserId).Distinct().ToArray();
                    var userList = _userService.GetAll(m => userIdList.Contains(m.Id));
                    var users = userList as T_USER[] ?? userList.ToArray();
                    foreach (var question in result.Entity)
                    {
                        var user = users.First(m => m.Id == question.UserId);
                        question.CreateUser = string.IsNullOrEmpty(user.UserName) ? user.LoginUserName : user.UserName;
                    }
                }

                SetJosnResult<List<SearchQutionOutput>>(result, input.PageIndex, input.PageSize,
                    totalCount, "获取问题列表成功!");

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "公开问题"
        public JsonResult Open(string Ids)
        {
            CheckPermission();
            if (string.IsNullOrEmpty(Ids))
                throw new CustomException("lds");

            var idsStringArray = Ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idsLongArray = new long[idsStringArray.Length];
            for (var i = 0; i < idsStringArray.Length; i++)
            {
                idsLongArray[i] = long.Parse(idsStringArray[i]);
            }

            using (var result = new ResponseResult<object>())
            {
                var list = _expertQuestionService.GetAll(m => idsLongArray.Contains(m.Id));
                var entities = list as T_QUESTION[] ?? list.ToArray();

                if (list != null && entities.Any())
                {
                    foreach (var question in entities)
                    {
                        question.IsOpen = true;
                    }

                    _expertQuestionService.Update(entities);
                }

                result.Message = "公开问题成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "取消公开问题"
        public JsonResult CancelOpen(string Ids)
        {
            CheckPermission();
            if (string.IsNullOrEmpty(Ids))
                throw new CustomException("lds");

            var idsStringArray = Ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idsLongArray = new long[idsStringArray.Length];
            for (var i = 0; i < idsStringArray.Length; i++)
            {
                idsLongArray[i] = long.Parse(idsStringArray[i]);
            }

            using (var result = new ResponseResult<object>())
            {
                var list = _expertQuestionService.GetAll(m => idsLongArray.Contains(m.Id));
                var entities = list as T_QUESTION[] ?? list.ToArray();

                if (list != null && entities.Any())
                {
                    foreach (var question in entities)
                    {
                        question.IsOpen = false;
                    }

                    _expertQuestionService.Update(entities);
                }

                result.Message = "取消公开问题成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "删除问题"
        public JsonResult Delete(string Ids)
        {
            CheckPermission();
            if (string.IsNullOrEmpty(Ids))
                throw new CustomException("lds");

            var idsStringArray = Ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idsLongArray = new long[idsStringArray.Length];
            for (var i = 0; i < idsStringArray.Length; i++)
            {
                idsLongArray[i] = long.Parse(idsStringArray[i]);
            }

            using (var result = new ResponseResult<object>())
            {
                var list = _expertQuestionService.GetAll(m => idsLongArray.Contains(m.Id));
                var entities = list as T_QUESTION[] ?? list.ToArray();

                if (list != null && entities.Any())
                {
                    foreach (var question in entities)
                    {
                        question.IsDeleted = true;
                    }

                    _expertQuestionService.Update(entities);
                }

                result.Message = "删除问题成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "问题详情"
        [HttpPost]
        public JsonResult Detail(long questionId, int replyPageIndex, int replyPageSize)
        {
            CheckPermission();
            using (var result = new ResponseResult<QuestionDetailWithReplyListOutput>())
            {
                //获取问题详情
                var question = _expertQuestionService.GetByKey(questionId);
                if (question == null)
                    throw new CustomException("问题不存在!");

                if (question.IsDeleted)
                    throw new CustomException("问题已被删除!");

                var model = new QuestionDetailWithReplyListOutput
                {
                    CreateTime = question.CreateTime,
                    LastModifiedTime = question.LastModifiedTime,
                    QuestionId = question.Id,
                    Title = question.Title,
                    Description = question.Description
                };
                var user = _userService.GetByKey(question.UserId);
                model.CreateUser = user.UserName.DefaultIfEmpty(user.LoginUserName);

                long totalCount;
                var list = _expertQuestionReplyService.GetAll<DateTime?>(m => m.QuestionId == questionId,
                    m => m.LastModifiedTime, null, replyPageIndex, replyPageSize, out totalCount);

                var questionReplys = list as T_QUESTION_REPLY[] ?? list.ToArray();
                if (questionReplys.Any())
                {
                    //提取用户编号
                    var userIdList = questionReplys.Select(m => m.UserId).Distinct().ToArray();
                    var userList = _userService.GetAll(m => userIdList.Contains(m.Id));
                    model.ReplyList = Mapper.Map<List<QuestionReply>>(list);
                    var enumerable = userList as T_USER[] ?? userList.ToArray();
                    foreach (var reply in model.ReplyList)
                    {
                        var replyUser = enumerable.First(m => m.Id == reply.UserId);
                        reply.UserName = string.IsNullOrEmpty(replyUser.UserName) ? replyUser.LoginUserName : replyUser.UserName;
                    }
                }
                result.Entity = model;
                SetJosnResult<QuestionDetailWithReplyListOutput>(result, replyPageIndex, replyPageSize, totalCount, "获取问题详情成功!");
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "添加到轮播"
        [HttpPost]
        public JsonResult AddToCarousel(string Ids)
        {
            CheckPermission();
            if (string.IsNullOrEmpty(Ids))
                throw new CustomException("lds");

            var idsStringArray = Ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idsLongArray = new long[idsStringArray.Length];
            for (var i = 0; i < idsStringArray.Length; i++)
            {
                idsLongArray[i] = long.Parse(idsStringArray[i]);
            }

            using (var result = new ResponseResult<object>())
            {
                var list = _expertQuestionService.GetAll(m => idsLongArray.Contains(m.Id));
                var entities = list as T_QUESTION[] ?? list.ToArray();

                if (list != null && entities.Any())
                {
                    foreach (var question in entities)
                    {
                        question.IsPutOnCarousel = true;
                    }

                    _expertQuestionService.Update(entities);
                }

                result.Message = "添加到轮播图成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "取消到轮播"
        [HttpPost]
        public JsonResult CancelCarousel(string Ids)
        {
            CheckPermission();
            if (string.IsNullOrEmpty(Ids))
                throw new CustomException("lds");

            var idsStringArray = Ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var idsLongArray = new long[idsStringArray.Length];
            for (var i = 0; i < idsStringArray.Length; i++)
            {
                idsLongArray[i] = long.Parse(idsStringArray[i]);
            }

            using (var result = new ResponseResult<object>())
            {
                var list = _expertQuestionService.GetAll(m => idsLongArray.Contains(m.Id));
                var entities = list as T_QUESTION[] ?? list.ToArray();

                if (list != null && entities.Any())
                {
                    foreach (var question in entities)
                    {
                        question.IsPutOnCarousel = false;
                    }

                    _expertQuestionService.Update(entities);
                }

                result.Message = "取消轮播成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "导出问题列表为Excel"
        [HttpGet]
        public ActionResult ExportExcelWithQuestionList(SearchQuestionInput input)
        {
            CheckPermission();
            input.PageIndex = 1;
            if (input.PageSize > 10000)
            {
                input.PageSize = 10000;
            }

            var predicate = PredicateBuilder.True<T_QUESTION>();
            if (!string.IsNullOrEmpty(input.Keywords))
            {
                predicate = predicate.And(m => m.Title.Contains(input.Keywords));
            }

            if (input.IsOpen.HasValue)
                predicate = predicate.And(m => m.IsOpen == input.IsOpen);

            if (input.IsDeleted.HasValue)
            {
                predicate = predicate.And(m => m.IsDeleted == input.IsDeleted);
            }

            long totalCount;
            var list = _expertQuestionService.GetAll<DateTime>(predicate, null, m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount);
            List<SearchQutionOutput> questionListOutput = Mapper.Map<List<SearchQutionOutput>>(list);
            var enumerable = list as T_QUESTION[] ?? list.ToArray();
            if (enumerable.Any())
            {
                //提取用户编号
                var userIdList = enumerable.Select(m => m.UserId).Distinct().ToArray();
                var userList = _userService.GetAll(m => userIdList.Contains(m.Id));
                var users = userList as T_USER[] ?? userList.ToArray();

                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                // 创建一张工作薄。
                var workSheet = workbook.CreateSheet("专家咨询问题列表");
                var headerRow = workSheet.CreateRow(0);
                var tableHeaderTexts = new string[] { "问题编号","标题","内容","开放状态","删除状态","提问者","创建时间","更新时间" };

                ICellStyle style = workbook.CreateCellStyle();
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style.FillPattern = FillPattern.SolidForeground;


                //生成列头
                for (int i = 0; i < tableHeaderTexts.Length; i++)
                {
                    var currentCell = headerRow.CreateCell(i);

                    currentCell.SetCellValue(tableHeaderTexts[i]);
                    currentCell.CellStyle = style;
                }

                var currentRoeIndex = 0;
                foreach (var question in questionListOutput)
                {
                    currentRoeIndex++;
                    var user = users.First(m => m.Id == question.UserId);
                    question.CreateUser = string.IsNullOrEmpty(user.UserName) ? user.LoginUserName : user.UserName;
                    var dataRow = workSheet.CreateRow(currentRoeIndex);
                    var idCell = dataRow.CreateCell(0);
                    var titleCell = dataRow.CreateCell(1);
                    var contentCell = dataRow.CreateCell(2);
                    var openStateCell = dataRow.CreateCell(3);
                    var deleteStateCell = dataRow.CreateCell(4);
                    var questionerCell = dataRow.CreateCell(5);
                    var createTimeCell = dataRow.CreateCell(6);
                    var updateTimeCell = dataRow.CreateCell(7);

                    idCell.SetCellValue(question.Id);
                    titleCell.SetCellValue(question.Title);
                    contentCell.SetCellValue(question.Description);
                    openStateCell.SetCellValue(question.IsOpen?"已开放":"未开放");
                    deleteStateCell.SetCellValue(question.IsDeleted?"已删除":"正常");
                    questionerCell.SetCellValue(question.CreateUser);
                    createTimeCell.SetCellValue(question.CreateTime.ToString("yyyy.MM.dd"));
                    updateTimeCell.SetCellValue(question.LastModifiedTime.ToString("yyyy.MM.dd"));
                }
                workbook.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=QuestionList" + (DateTime.Now.ToString("yyyyMMddHHmmss")) + ".xls"));
                Response.BinaryWrite(ms.ToArray()); workbook = null;
                return File(ms, "application/ms-excel");
            }
            else
            {
                return Content("no data");
            }
        }
        #endregion
    }
}