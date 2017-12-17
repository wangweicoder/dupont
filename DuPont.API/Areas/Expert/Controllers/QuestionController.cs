// ***********************************************************************
// Assembly : DuPont.API
// Author : 毛文君
// Created : 12-20-2015
// Tel :15801270290
// QQ :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-22-2015
// ***********************************************************************
// <copyright file="QuestionController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using AutoMapper;
using DuPont.API.Filters;



using DuPont.Global.ActionResults;
using DuPont.Global.Exceptions;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using DuPont.Utility;
using DuPont.Models.Dtos.Foreground.Expert;
using DuPont.Models.Models;
using DuPont.Entity.Enum;

namespace DuPont.API.Areas.Expert.Controllers
{
#if(!DEBUG)
    [AccessAuthorize]
#endif
    [Validate]
    public class QuestionController : Controller
    {
        private readonly IExpertPermission _expertPermissionService;
        private readonly IExpertQuestionReply _expertQuestionReplyService;
        private readonly IExpertQuestion _expertQuestionService;
        private readonly IFileInfoRepository _fileInfoRepository;
        private readonly IUser _userService;
        private readonly INotification _notificationService;
        private readonly IUser_Role _userRoleRelationService;

        public QuestionController(IExpertQuestion expertQuestionService,
            IFileInfoRepository fileInfoRepositoryService,
            IExpertQuestionReply expertQuestionReplyService,
            IExpertPermission expertPermissionService,
            IUser userService,
            INotification notificationService,
            IUser_Role userRoleRelationService
            )
        {
            _expertQuestionService = expertQuestionService;
            _fileInfoRepository = fileInfoRepositoryService;
            _expertQuestionReplyService = expertQuestionReplyService;
            _expertPermissionService = expertPermissionService;
            _userService = userService;
            _notificationService = notificationService;
            _userRoleRelationService = userRoleRelationService;
        }

        #region "创建问题"

        [HttpPost]
        public ActionResult Create(ExpertQuestionInput question)
        {
            using (var result = new ResponseResult<object>())
            {
                var model = Mapper.Map<T_QUESTION>(question);
                var effectCount = _expertQuestionService.Insert(model);
                result.IsSuccess = effectCount > 0;
                result.Message = "问题创建成功!";

                //添加先锋币
                var roleType = (RoleType)question.RoleId;
                if (roleType == RoleType.Farmer)
                {
                    var userInfo = _userService.GetByKey(question.CreateUserId);
                    if (userInfo != null)
                    {
                        userInfo.DPoint = (userInfo.DPoint ?? 0) + 5;
                        _userService.Update(userInfo);
                    }
                }
                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "回答问题"

        [HttpPost]
        public JsonResult Reply(ExpertQuestionReplyInput input)
        {
            using (var result = new ResponseResult<object>())
            {
                var model = Mapper.Map<T_QUESTION_REPLY>(input);
                var question = _expertQuestionService.GetByKey(input.QuestionId);

                //判断该条的问题是否存在
                if (question == null)
                    throw new CustomException("问题不存在!");

                //判断该问题是否已经公开
                if (question.IsOpen)
                    throw new CustomException("该问题已公开,不能执行该操作!");

                result.IsSuccess = _expertQuestionReplyService.Insert(model) > 0;
                if (!result.IsSuccess)
                    return new JsonResultEx(result);

                result.Message = "回答成功!";
                //更新该问题的回复数
                question.ReplyCount += 1;
                _expertQuestionService.Update(question);

                //添加先锋币
                var roleType = (RoleType)question.RoleId;
                if (roleType == RoleType.Farmer)
                {
                    var userInfo = _userService.GetByKey(input.ReplyUserId);
                    if (userInfo != null)
                    {
                        userInfo.DPoint = (userInfo.DPoint ?? 0) + 5;
                        _userService.Update(userInfo);
                    }
                }

                //给问题的提问者推送通知
                _notificationService.Insert(new T_NOTIFICATION
                {
                    MsgContent = "有人回答了你的问题,快去看看吧!",
                    IsPublic = false,
                    TargetUserId = question.UserId,
                    NotificationType=2,
                    NotificationSource="",
                    NotificationSourceId=question.Id,
                    IsOpen=question.IsOpen,                  
                });

                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "问题列表"

        [HttpPost]
        public JsonResult List(ExpertQuestionSearchInput searchInput)
        {
            using (var result = new ResponseResult<List<ExpertQuestionListOutput>>())
            {
                long totalCount;
                var isOpen = searchInput.IsOpen > 0;
                Expression<Func<T_QUESTION, bool>> predicate;
                Expression<Func<T_QUESTION, bool>> carouselPredicate;
                if (searchInput.IsOpen == 0)
                {
                    carouselPredicate = m => !m.IsDeleted && !m.IsOpen && m.IsPutOnCarousel;
                }
                else
                {
                    carouselPredicate = m => !m.IsDeleted && m.IsOpen && m.IsPutOnCarousel;
                }

                var carouselList = _expertQuestionService.GetAll(carouselPredicate, null, m => m.LastModifiedTime, 1, 3,
                    out totalCount, "User");

                var carouselQuestionIdList = carouselList.Select(m => m.Id).ToArray();

                if (!string.IsNullOrEmpty(searchInput.Keywords))
                {
                    predicate =
                        m =>
                            !m.IsDeleted && !carouselQuestionIdList.Contains(m.Id) &&
                            m.Title.Contains(searchInput.Keywords) && m.IsOpen == isOpen;
                }
                else
                {
                    predicate = m => !m.IsDeleted && !carouselQuestionIdList.Contains(m.Id) && m.IsOpen == isOpen;
                }

                var list = _expertQuestionService.GetAll(predicate, null, m => m.LastModifiedTime, searchInput.PageIndex,
                    searchInput.PageSize, out totalCount, "User");
                var enumerable = list as T_QUESTION[] ?? list.ToArray();
                if (enumerable.Any())
                {
                    result.Entity = Mapper.Map<List<ExpertQuestionListOutput>>(list);

                    //从每个问题条目中提取一个图片编号
                    var pictureIdDictionary = new Dictionary<long, long>();
                    GetOnePictureIdFromPerQuestion(enumerable, pictureIdDictionary);

                    var picIdArray = pictureIdDictionary.Values.ToArray();
                    //根据图片编号列表获取图片信息
                    var fileInfoList = _fileInfoRepository.GetAll(m => picIdArray.Contains(m.Id));
                    //图片路径赋值
                    foreach (var item in result.Entity)
                    {
                        if (!pictureIdDictionary.ContainsKey(item.QuestionId)) continue;
                        var fileInfo = fileInfoList.FirstOrDefault(m => m.Id == pictureIdDictionary[item.QuestionId]);
                        if (fileInfo != null)
                        {
                            item.PictureUrl = fileInfo.Path;
                        }
                    }
                }
                result.TotalNums = totalCount;

                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "问题详情"

        [HttpPost]
        // ReSharper disable once InconsistentNaming
        public JsonResult Detail(long QuestionId)
        {
            using (var result = new ResponseResult<ExpertQuestionDetailOutput>())
            {
                var question = _expertQuestionService.GetByKey(QuestionId);
                if (question == null)
                {
                    result.IsSuccess = false;
                    result.Message = "问题不存在!";
                    return new JsonResultEx(result);
                }

                if (question.IsDeleted)
                {
                    result.IsSuccess = false;
                    result.Message = "问题已被删除!";
                    return new JsonResultEx(result);
                }

                //获取问题发起者信息
                var user = _userService.GetByKey(question.UserId);

                result.Entity.QuestionId = question.Id;
                result.Entity.Description = question.Description;
                result.Entity.CreateTime = long.Parse(TimeHelper.GetMilliSeconds(question.CreateTime));
                result.Entity.UserName = string.IsNullOrEmpty(user.UserName) ? user.LoginUserName : user.UserName;
                //取出所有图片编号
                var pictureIdList = GetPictureIdList(question.PictureIds);
                if (pictureIdList != null && pictureIdList.Count > 0)
                {
                    // var pictureIdArray = pictureIdList.ToArray();
                    var fileInfoList = _fileInfoRepository.GetAll(m => pictureIdList.Contains(m.Id));
                    if (fileInfoList != null)
                    {
                        foreach (var picture in fileInfoList)
                        {
                            result.Entity.PictureUrlList.Add(picture.Path);
                        }
                    }
                }
                result.Message = "获取问题详情成功!";
                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "回复列表"

        [HttpPost]
        public JsonResult ReplyList(ExpertQuestionReplyListSearchInput input)
        {
            using (var result = new ResponseResult<List<ExpertQuestionReplyListOutput>>())
            {
                if (input.PageSize > 50)
                {
                    input.PageSize = 10;
                }

                long totalCount;
                //按创建时间倒序
                var list = _expertQuestionReplyService.GetAll<DateTime?>(m => m.QuestionId == input.QuestionId,
                    null,m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount);

                var questionReplys = list as T_QUESTION_REPLY[] ?? list.ToArray();
                if (questionReplys.Any())
                {
                    //提取用户编号
                    var userIdList = questionReplys.Select(m => m.UserId).ToArray();
                    var userList = _userService.GetAll(m => userIdList.Contains(m.Id));
                    result.Entity = Mapper.Map<List<ExpertQuestionReplyListOutput>>(list);
                    foreach (var reply in result.Entity)
                    {
                        var user = userList.First(m => m.Id == reply.ReplyUserId);
                        reply.ReplyUserName = string.IsNullOrEmpty(user.UserName) ? user.LoginUserName : user.UserName;
                    }
                }

                result.TotalNums = totalCount;
                result.Message = "获取回复列表成功";
                result.PageIndex = input.PageIndex;
                result.PageSize = input.PageSize;
                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "提取图片编号列表"

        // ReSharper disable once InconsistentNaming
        private static List<long> GetPictureIdList(string PictureIds)
        {
            if (string.IsNullOrEmpty(PictureIds)) return null;
            var pictureIdArray = new List<long>();

            var tempPictureIds = PictureIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var pictureId = 0;
            if (tempPictureIds.Length <= 0) return pictureIdArray;
            pictureIdArray.AddRange((
                from picId in tempPictureIds
                where int.TryParse(picId, out pictureId)
                select pictureId).Select(dummy => (long)dummy));
            return pictureIdArray;
        }

        #endregion

        #region "提问列表"

        [HttpPost]
        public JsonResult MyList(ExpertMyQuestionListInput input)
        {
            using (var result = new ResponseResult<List<ExpertMyQuestionListOutput>>())
            {
                long totalCount;
                //取得问题列表
                var list = _expertQuestionService.GetAll<DateTime?>(m => !m.IsDeleted && m.UserId == input.UserId,
                    null, m => m.LastModifiedTime, input.PageIndex, input.PageSize, out totalCount, "User");
                result.Entity = Mapper.Map<List<ExpertMyQuestionListOutput>>(list);

                //从每个问题条目中提取一个图片编号
                var pictureIdDictionary = new Dictionary<long, long>();
                var count = list.Select(m =>
                {
                    if (m.PictureIds == null) return m;
                    var tempPictureId = m.PictureIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    int pictureId;
                    if (int.TryParse(tempPictureId, out pictureId))
                    {
                        pictureIdDictionary.Add(m.Id, pictureId);
                    }
                    return m;
                }).Count();

                var picIdArray = pictureIdDictionary.Values.ToArray();
                //根据图片编号列表获取图片信息
                var fileInfoList = _fileInfoRepository.GetAll(m => picIdArray.Contains(m.Id));
                //图片路径赋值
                foreach (var item in result.Entity)
                {
                    if (!pictureIdDictionary.ContainsKey(item.QuestionId)) continue;
                    var fileInfo = fileInfoList.FirstOrDefault(m => m.Id == pictureIdDictionary[item.QuestionId]);
                    if (fileInfo != null)
                    {
                        item.PictureUrl = fileInfo.Path;
                    }
                }

                result.PageIndex = input.PageIndex;
                result.PageSize = input.PageSize;
                result.TotalNums = totalCount;
                result.Message = "获取提问列表成功";

                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "我的回答列表"

        [HttpPost]
        public ActionResult MyReplyList(ExpertMyReplyListInput input)
        {
            using (var result = new ResponseResult<List<ExpertMyReplyListOutput>>())
            {
                long totalCount;
                var list = _expertQuestionReplyService.GetAll<DateTime?>(m => m.UserId == input.ReplyUserId, null,
                    m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount);
                result.Entity = Mapper.Map<List<ExpertMyReplyListOutput>>(list);

                result.PageIndex = input.PageIndex;
                result.PageSize = input.PageSize;
                result.TotalNums = totalCount;
                result.Message = "获取回答列表成功";
                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "对问题点赞"

        [HttpPost]
        // ReSharper disable once InconsistentNaming
        public JsonResult PraiseReply(long ReplyId)
        {
            using (var result = new ResponseResult<object>())
            {
                var model = _expertQuestionReplyService.GetByKey(ReplyId);
                if (model == null)
                {
                    throw new CustomException("被点赞的记录不存在!");
                }

                model.LikeCount += 1;
                result.IsSuccess = _expertQuestionReplyService.Update(model) > 0;
                result.Message = "点赞成功!";
                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "对回复进行采纳（同意）"

        [HttpPost]
        public JsonResult Agree(ExpertQuestionReplyAgreeInput input)
        {
            using (var result = new ResponseResult<object>())
            {
                //判断该条回复是否存在
                var replyModel = _expertQuestionReplyService.GetByKey(input.ReplyId);
                if (replyModel == null)
                    throw new CustomException("回复记录不存在!");

                //判断该条的问题是否存在
                var question = _expertQuestionService.GetByKey(replyModel.QuestionId);
                if (question == null)
                    throw new CustomException("问题不存在!");

                //判断该问题的发起者是否是当前用户
                if (question.UserId != input.UserId)
                    throw new CustomException("您没有操作该问题的权限!");

                ////判断该问题是否已经公开
                //if (question.IsOpen)
                //    throw new CustomException("该问题已公开,不能执行该操作!");

                //判断该条回复是否已经采纳过
                if (replyModel.IsAgree)
                    throw new CustomException("该回复已被提问者采纳,不得重复操作!");

                var hasAgreeReply =
                    _expertQuestionReplyService.Count(m => m.QuestionId == replyModel.QuestionId && m.IsAgree) > 0;
                if (hasAgreeReply)
                    throw new CustomException("该问题已有被采纳的回复,不得重复操作!");

                replyModel.IsAgree = true;
                replyModel.LastModifiedTime = DateTime.Now;
                _expertQuestionReplyService.Update(replyModel);

                //将问题进行公开
                question.IsOpen = true;
                _expertQuestionService.Update(question);

                result.Message = "处理同意操作成功!";
                return new JsonResultEx(result);
            }
        }

        #endregion

        #region "问题列表轮播图接口"

        [HttpPost]
        public JsonResult CarouselPictures(long UserId, int IsOpen)
        {
            using (var result = new ResponseResult<List<ExpertQuestionCarouselOutput>>())
            {
                long totalCount;
                //判断是否有专家身份
                var isExpert = _expertPermissionService.Count(m => m.IsEnabled && m.UserId == UserId) > 0;

                Expression<Func<T_QUESTION, bool>> predicate;
                if (isExpert && IsOpen == 0)
                {
                    predicate = m => !m.IsDeleted && !m.IsOpen && m.IsPutOnCarousel;
                }
                else
                {
                    predicate = m => !m.IsDeleted && m.IsOpen && m.IsPutOnCarousel;
                }

                //获取符合条件的问题列表
                var list = _expertQuestionService.GetAll(predicate, null, m => m.LastModifiedTime, 1, 3, out totalCount, "User");

                var enumerable = list as T_QUESTION[] ?? list.ToArray();
                if (enumerable.Any())
                {
                    result.Entity = Mapper.Map<List<ExpertQuestionCarouselOutput>>(list);

                    //从每个问题条目中提取一个图片编号
                    var pictureIdDictionary = new Dictionary<long, long>();
                    GetOnePictureIdFromPerQuestion(enumerable, pictureIdDictionary);

                    var picIdArray = pictureIdDictionary.Values.ToArray();
                    //根据图片编号列表获取图片信息
                    var fileInfoList = _fileInfoRepository.GetAll(m => picIdArray.Contains(m.Id));

                    //提取每个问题的用户编号
                    var userIdList = enumerable.Select(m => m.UserId).Distinct().ToArray();
                    var userList = _userService.GetAll(m => userIdList.Contains(m.Id));

                    //图片路径赋值
                    foreach (var item in result.Entity)
                    {
                        if (pictureIdDictionary.ContainsKey(item.QuestionId))
                        {
                            var fileInfo = fileInfoList.FirstOrDefault(m => m.Id == pictureIdDictionary[item.QuestionId]);
                            if (fileInfo != null)
                            {
                                item.PictureUrl = fileInfo.Path;
                            }
                        }

                        var user = userList.First(m => m.Id == item.UserId);
                        item.UserName = string.IsNullOrEmpty(user.UserName) ? user.LoginUserName : user.UserName;
                    }
                }
                result.TotalNums = totalCount;
                result.Message = "获取问题列表轮播图成功!";
                return new JsonResultEx(result);
            }
        }

        private static void GetOnePictureIdFromPerQuestion(IEnumerable<T_QUESTION> list,
            IDictionary<long, long> pictureIdDictionary)
        {
            list.Select(m =>
            {
                if (m.PictureIds == null) return m;
                var tempPictureId = m.PictureIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                int pictureId;
                if (int.TryParse(tempPictureId, out pictureId))
                {
                    pictureIdDictionary.Add(m.Id, pictureId);
                }
                return m;
            }).Count();
        }

        private void ExtractQuestionIdWithPictureIdList(IDictionary<long, List<long>> dicPictureList,
            IEnumerable<T_QUESTION> list)
        {
            if (list == null) return;
            foreach (var question in list)
            {
                var pictureIdList = GetPictureIdList(question.PictureIds);
                if (pictureIdList != null)
                {
                    dicPictureList.Add(question.Id, pictureIdList);
                }
            }
        }

        #endregion
    }
}