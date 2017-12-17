// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 10-27-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-13-2015
// ***********************************************************************
// <copyright file="LearnGardenController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using AutoMapper;



using DuPont.Entity.Enum;
using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DuPont.Extensions;
using System.Linq.Expressions;
using DuPont.Global.Exceptions;
using DuPont.Models.Dtos.Background.LearningWorld;
using DuPont.Models.Models;
using DuPont.Utility;

namespace DuPont.Controllers
{
    [ValidateInput(false)]
    [Validate]
    public class LearnGardenController : BaseController
    {

        private readonly IArticle _articleService;
        private readonly IFarm _farmService;
        private readonly IFarmArea _farmAreaService;
        private readonly IArea _areaService;
        private readonly INotification _notificationService;
        private readonly IFarmBooking _farmBookService;
        public LearnGardenController(IPermissionProvider permissionProvider,
            IAdminUser adminUserRepository, IArticle articleService,
            IFarm farmService, IFarmArea farmAreaService, IArea areaService,
            INotification notificationService,
            IFarmBooking farmBookService
            )
            : base(permissionProvider, adminUserRepository)
        {
            _articleService = articleService;
            _farmService = farmService;
            _farmAreaService = farmAreaService;
            _areaService = areaService;
            _notificationService = notificationService;
            _farmBookService = farmBookService;
        }

        #region 获取文章列表
        [HttpPost]
        public JsonResult ArticleList(ArticleListSearchInput input)
        {
            CheckPermission();
            using (var result = new ResponseResult<List<Article>>())
            {
                result.PageIndex = input.PageIndex;
                result.PageSize = input.PageSize;
                long totalCount;

                var predicate = PredicateBuilder.True<T_ARTICLE>();

                //如果指定了删除状态
                if (input.IsDeleted.HasValue)
                    predicate = predicate.And(m => m.IsDeleted == input.IsDeleted.Value);

                //如果指定了分类
                if (input.CatId > 1)
                    predicate = predicate.And(m => m.CatId == input.CatId);

                //如果指定了关键词
                if (!string.IsNullOrEmpty(input.Keywords))
                    predicate = predicate.And(m => m.Title.Contains(input.Keywords));

                IEnumerable<T_ARTICLE> list;
                if (input.OrderBy.IsNullOrEmpty() || input.OrderBy == "-date")
                {
                    list = _articleService.GetAll(predicate, null, m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount, "T_ARTICLE_CATEGORY");
                }
                else
                {
                    list = _articleService.GetAll(predicate, null, m => m.Click, input.PageIndex, input.PageSize, out totalCount, "T_ARTICLE_CATEGORY");
                }

                var regexHtmlTag = new Regex(@"<[^>]+?>", RegexOptions.Multiline);

                var enumerable = list as T_ARTICLE[] ?? list.ToArray();
                foreach (var article in enumerable)
                {
                    article.Content = HttpUtility.HtmlEncode(regexHtmlTag.Replace(article.Content, ""));
                }
                result.TotalNums = totalCount;
                result.Entity = Mapper.Map<List<Article>>(enumerable.ToList());
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "获取指定文章"
        [HttpPost]
        public JsonResult Article(long id)
        {
            CheckPermission();
            using (var result = new ResponseResult<Article>())
            {
                var article = _articleService.GetById(id);
                result.Entity = Mapper.Map<T_ARTICLE, Article>(article);

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "添加文章"
        [HttpPost]
        public JsonResult AddArticle(ArticleInput input)
        {
            CheckPermission();
            using (var result = new ResponseResult<object>())
            {
                var articleModel = Mapper.Map<T_ARTICLE>(input);
                articleModel.CreateTime = DateTime.Now;
                articleModel.UpdateTime = articleModel.CreateTime;
                articleModel.Content = HttpUtility.HtmlDecode(articleModel.Content);//接收后解码

                //是否同时推送到轮播图
                if (input.AddToCarousel)
                {
                    articleModel.IsPutOnCarousel = true;
                }
                _articleService.Insert(articleModel);
                articleModel.Url = "/Articles/article_" + articleModel.Id + ".html";
                _articleService.Update(articleModel);

                //添加到推送
                if (input.AddToPushNotification)
                {
                    var content = "学习园地中有新东西发布,快去看看吧……";
                    _notificationService.Insert(new T_NOTIFICATION
                    {
                        MsgContent = content.Substring(0, content.Length > 250 ? 250 : content.Length),
                        IsPublic = true,
                        NotificationType = 1,
                        NotificationSource = articleModel.Url,
                        NotificationSourceId = articleModel.Id,
                    });
                }

                result.Entity = articleModel.Id;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "删除指定的文章"
        [HttpPost]
        public JsonResult DeleteArticle(string articleIds)
        {
            CheckPermission();
            using (var result = new ResponseResult<object>())
            {
                if (articleIds != null)
                {
                    var aryStringArticleId = articleIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var aryIntArticleId = new int[aryStringArticleId.Length];
                    for (var i = 0; i < aryStringArticleId.Length; i++)
                    {
                        aryIntArticleId[i] = int.Parse(aryStringArticleId[i]);
                    }
                    result.Entity = _articleService.DeleteArticle(aryIntArticleId);
                }
                else
                {
                    result.IsSuccess = false;
                    result.State.Id = (int)ResponseStatusCode.InvalidArgument;
                    result.State.Description = ResponseStatusCode.InvalidArgument.GetDescription();
                }
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "添加文章到轮播位"
        [HttpPost]
        public JsonResult AddArticleToCarousel(string articleIds)
        {
            CheckPermission();
            using (var result = new ResponseResult<object>())
            {
                if (articleIds != null)
                {
                    var aryStringArticleId = articleIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var aryIntArticleId = new int[aryStringArticleId.Length];
                    for (var i = 0; i < aryStringArticleId.Length; i++)
                    {
                        aryIntArticleId[i] = int.Parse(aryStringArticleId[i]);
                    }
                    result.Entity = _articleService.AddToCarousel(aryIntArticleId);
                }
                else
                {
                    result.IsSuccess = false;
                    result.State.Id = (int)ResponseStatusCode.InvalidArgument;
                    result.State.Description = ResponseStatusCode.InvalidArgument.GetDescription();
                }
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "删除示范农场"
        [HttpPost]
        public JsonResult DeleteFarm(string farmIds)
        {
            CheckPermission();
            using (var result = new ResponseResult<object>())
            {
                if (farmIds != null)
                {
                    var aryStringFarmId = farmIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var aryIntFarmId = new int[aryStringFarmId.Length];
                    for (var i = 0; i < aryStringFarmId.Length; i++)
                    {
                        aryIntFarmId[i] = int.Parse(aryStringFarmId[i]);
                    }
                    result.Entity = _farmService.DeleteFarm(aryIntFarmId);
                }
                else
                {
                    result.IsSuccess = false;
                    result.State.Id = (int)ResponseStatusCode.InvalidArgument;
                    result.State.Description = ResponseStatusCode.InvalidArgument.GetDescription();
                }
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "添加示范农场"
        [HttpPost]
        public JsonResult AddDemonstateFarm(DemonstateFarmDetailViewModel input)
        {
            CheckPermission();
            using (var result = new ResponseResult<object>())
            {
                var isAdmin = false;
                if (!this.UserInfo.IsSuperAdmin)
                {
                    //获取用户角色信息
                    var roles = adminUserRepository.GetRoles(UserId);

                    if (roles.Any(m => m.RoleID == (int)RoleType.Admin))
                        isAdmin = true;

                    if (!isAdmin)
                    {
                        //按经销商处理
                        var areaList = _areaService.GetManageArea("-1", UserId);
                        var provinceIdList = areaList.Where(m => m.ParentAID == "0").Select(m => m.AID).ToArray();
                        var cityIdList = areaList.Where(m => provinceIdList.Contains(m.ParentAID)).Select(m => m.AID).ToArray();
                        var regionIdList = areaList.Where(m => cityIdList.Contains(m.ParentAID)).Select(m => m.AID).ToArray();

                        //判断权限
                        if (!provinceIdList.Contains(input.ProvinceAid)
                            || !cityIdList.Contains(input.CityAid)
                            || !regionIdList.Contains(input.RegionAid)
                            )
                        {
                            throw new CustomException("您没有该地区的添加权限!");
                        }
                    }
                }

                //同地区的同名农场不可重复添加
                var existSameFarm = _farmService.Count(m => m.Name == input.Name
                                                            && m.ProvinceAid == input.ProvinceAid
                                                            && m.CityAid == input.CityAid
                                                            && m.RegionAid == input.RegionAid) > 0;
                if (existSameFarm)
                    throw new CustomException("已存在相同的农场!");

                var farm = new T_DEMONSTRATION_FARM()
                {
                    ProvinceAid = input.ProvinceAid,
                    CityAid = input.CityAid,
                    RegionAid = input.RegionAid,
                    Name = input.Name,
                    IsOpen = input.IsOpen,
                    OpenStartDate = input.OpenStartDate,
                    OpenEndDate = input.OpenEndDate,
                    PlantArea = input.PlantArea,
                    Variety = input.Variety,
                    SowTime = input.SowTime,
                    PlantPoint = input.PlantPoint,
                    CreateTime = DateTime.Now
                };

                _farmService.Insert(farm);
                result.Message = "添加示范农场成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "获取示范农场详情"
        /// <summary>
        /// 编辑示范农场
        /// </summary>
        /// <param name="farmId">示范农场编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DemonstateFarmDetail(int farmId)
        {
            using (var result = new ResponseResult<DemonstateFarmDetailViewModel>())
            {
                var farmInfo = _farmService.GetByKey(farmId);
                if (farmInfo == null)
                    return ResponseErrorWithJson<DemonstateFarmDetailViewModel>(result, "未获取到该农场信息!");

                result.Entity = Mapper.Map<DemonstateFarmDetailViewModel>(farmInfo);

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "更新示范农场"
        /// <summary>
        /// 编辑示范农场
        /// </summary>
        /// <param name="farmId">示范农场编号</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateDemonstateFarmDetail(DemonstateFarmDetailViewModel input)
        {
            using (var result = new ResponseResult<object>())
            {
                _farmService.Update(p => p.Id == input.Id, t => new T_DEMONSTRATION_FARM
                {
                    Name = input.Name,
                    PlantArea = input.PlantArea,
                    SowTime = input.SowTime,
                    Variety = input.Variety,
                    ProvinceAid = input.ProvinceAid,
                    CityAid = input.CityAid,
                    RegionAid = input.RegionAid,
                    PlantPoint = input.PlantPoint,
                    IsOpen = input.IsOpen,
                    OpenStartDate = input.OpenStartDate,
                    OpenEndDate = input.OpenEndDate
                });

                return ResponseSuccessWithJson<object>(result, "修改成功!");
            }
        }
        #endregion

        #region "示范农场列表"
        [HttpPost]
        public JsonResult FarmList(FarmListInput input)
        {
            CheckPermission();
            using (var result = new ResponseResult<List<FarmListOutput>>())
            {
                var predicate = PredicateBuilder.True<T_DEMONSTRATION_FARM>();
                var isAdmin = false;
                if (!this.UserInfo.IsSuperAdmin)
                {
                    //获取用户角色信息
                    var roles = adminUserRepository.GetRoles(UserId);

                    if (roles.Any(m => m.RoleID == (int)RoleType.Admin))
                        isAdmin = true;

                    if (!isAdmin)
                    {
                        //按经销商处理
                        var areaList = _areaService.GetManageArea("-1", UserId);
                        var provinceIdList = areaList.Where(m => m.ParentAID == "0").Select(m => m.AID).ToArray();
                        var cityIdList = areaList.Where(m => provinceIdList.Contains(m.ParentAID)).Select(m => m.AID).ToArray();
                        var regionIdList = areaList.Where(m => cityIdList.Contains(m.ParentAID)).Select(m => m.AID).ToArray();

                        //当地区条件未指定时
                        if (input.ProvinceAid.IsNullOrEmpty()
                            && input.CityAid.IsNullOrEmpty()
                            && input.RegionAid.IsNullOrEmpty()
                            )
                        {
                            predicate = predicate.And(m =>
                                                provinceIdList.Contains(m.ProvinceAid)
                                                && cityIdList.Contains(m.CityAid)
                                                && regionIdList.Contains(m.RegionAid));
                        }
                        else
                        {
                            //省份过滤
                            if (!input.ProvinceAid.IsNullOrEmpty() && provinceIdList.Contains(input.ProvinceAid))
                            {
                                predicate = predicate.And(m => m.ProvinceAid == input.ProvinceAid);
                            }
                            else
                            {
                                predicate = predicate.And(m => provinceIdList.Contains(m.ProvinceAid));
                            }

                            //城市过滤
                            if (!input.CityAid.IsNullOrEmpty() && cityIdList.Contains(input.CityAid))
                            {
                                predicate = predicate.And(m => m.CityAid == input.CityAid);
                            }
                            else
                            {
                                predicate = predicate.And(m => cityIdList.Contains(m.CityAid));
                            }

                            //区县过滤
                            if (!input.RegionAid.IsNullOrEmpty() && regionIdList.Contains(input.RegionAid))
                            {
                                predicate = predicate.And(m => m.RegionAid == input.RegionAid);
                            }
                            else
                            {
                                predicate = predicate.And(m => regionIdList.Contains(m.RegionAid));
                            }
                        }
                    }
                }

                if (input.IsDeleted.HasValue)
                    predicate = predicate.And(m => m.IsDeleted == input.IsDeleted.Value);

                if (input.IsOpen.HasValue)
                    predicate = predicate.And(m => m.IsOpen == input.IsOpen.Value);

                if (!string.IsNullOrEmpty(input.Keywords))
                    predicate = predicate.And(m => m.Name.Contains(input.Keywords));

                if (this.UserInfo.IsSuperAdmin || isAdmin)
                {
                    if (input.OpenStartDate != null)
                        predicate = predicate.And(m => m.OpenStartDate >= input.OpenStartDate.Value);

                    if (input.OpenEndDate != null)
                        predicate = predicate.And(m => m.OpenEndDate <= input.OpenEndDate.Value);

                    if (!input.ProvinceAid.IsNullOrEmpty())
                        predicate = predicate.And(m => m.ProvinceAid == input.ProvinceAid);

                    if (!input.CityAid.IsNullOrEmpty())
                        predicate = predicate.And(m => m.CityAid == input.CityAid);

                    if (!input.RegionAid.IsNullOrEmpty())
                        predicate = predicate.And(m => m.RegionAid == input.RegionAid);
                }

                long totalCount;
                var list = _farmService.GetAll<DateTime>(predicate, null, m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount);
                if (list != null && list.Any())
                {
                    var areaDictionary = new Dictionary<string, string>();
                    list.Select(m =>
                    {
                        if (!string.IsNullOrEmpty(m.ProvinceAid) && !areaDictionary.ContainsKey(m.ProvinceAid))
                            areaDictionary.Add(m.ProvinceAid, string.Empty);

                        if (!string.IsNullOrEmpty(m.CityAid) && !areaDictionary.ContainsKey(m.CityAid))
                            areaDictionary.Add(m.CityAid, string.Empty);

                        if (!string.IsNullOrEmpty(m.RegionAid) && !areaDictionary.ContainsKey(m.RegionAid))
                            areaDictionary.Add(m.RegionAid, string.Empty);

                        return m;
                    }).Count();
                    var areaIdArray = areaDictionary.Keys.ToArray();
                    var areaList = _areaService.GetAll(m => areaIdArray.Contains(m.AID));
                    result.Entity = Mapper.Map<List<FarmListOutput>>(list);
                    foreach (var farm in result.Entity)
                    {
                        if (!string.IsNullOrEmpty(farm.Province) && areaIdArray.Contains(farm.Province))
                        {
                            var areaInfo = areaList.FirstOrDefault(m => m.AID == farm.Province);
                            if (areaInfo != null)
                            {
                                farm.Province = areaInfo.DisplayName;
                            }
                            else
                            {
                                farm.Province = "";
                            }
                        }

                        if (!string.IsNullOrEmpty(farm.City) && areaIdArray.Contains(farm.City))
                        {
                            var areaInfo = areaList.FirstOrDefault(m => m.AID == farm.City);
                            if (areaInfo != null)
                            {
                                farm.City = areaInfo.DisplayName;
                            }
                            else
                            {
                                farm.City = "";
                            }
                        }

                        if (!string.IsNullOrEmpty(farm.Region) && areaIdArray.Contains(farm.Region))
                        {
                            var areaInfo = areaList.FirstOrDefault(m => m.AID == farm.Region);
                            if (areaInfo != null)
                            {
                                farm.Region = areaInfo.DisplayName;
                            }
                            else
                            {
                                farm.Region = "";
                            }
                        }
                    }
                }

                SetJosnResult<List<FarmListOutput>>(result, input.PageIndex, input.PageSize, totalCount, "获取示范农场列表成功!");
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "添加示范农场展区"

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddExhibitionAreaForDemonstateFarm(FarmAreaInput input)
        {
            CheckPermission();
            using (var result = new ResponseResult<object>())
            {
                //判断数据库中是否已有该区
                var existSameArea = _farmAreaService.Count(m => m.FarmId == input.FarmId && m.Name == input.Name) > 0;
                if (existSameArea)
                    throw new CustomException("已有相同的区存在!");

                var farmArea = new T_FARM_AREA
                {
                    FarmId = input.FarmId,
                    Name = input.Name,
                    IsFarmMachinery = input.IsMachineryArea,
                    CreateTime = DateTime.Now
                };

                _farmAreaService.Insert(farmArea);
                farmArea.Url = "/Articles/article_farm_area_" + farmArea.Id + ".html";
                _farmAreaService.Update(farmArea);
                result.Entity = farmArea.Id;
                result.Message = "农场展区添加成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "示范农场报名列表"
        [HttpPost]
        public JsonResult FarmBookList(FarmBookListInput input)
        {
            CheckPermission();
            using (var result = new ResponseResult<FarmBookListOutput>())
            {
                long totalCount;
                var farmBookList = _farmBookService.GetAll<DateTime>(m => m.FarmId == input.FarmId, null, m => m.CreateTime, input.PageIndex, input.PageSize, out totalCount, "ReservedUser,T_DEMONSTRATION_FARM");
                if (farmBookList != null && farmBookList.Any())
                {
                    var farmBookListOutput = new FarmBookListOutput();
                    farmBookListOutput.FarmName = farmBookList.ElementAt(0).T_DEMONSTRATION_FARM.Name;
                    result.Entity = farmBookListOutput;
                    result.Entity.FarmBookList = Mapper.Map<List<FarmBookItem>>(farmBookList);
                }

                SetJosnResult(result, input.PageIndex, input.PageSize, totalCount, "获取农场报名列表成功!");
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "示范农场展区列表"
        [HttpPost]
        public JsonResult ExhibitionAreaList(int farmId)
        {
            using (var result = new ResponseResult<List<ExhibitionAreaListOutput>>())
            {
                var exhibitionAreaList = _farmAreaService.GetAll(m => m.FarmId == farmId);
                if (exhibitionAreaList != null)
                {
                    result.Entity = Mapper.Map<List<ExhibitionAreaListOutput>>(exhibitionAreaList.ToList());
                    SetJosnResult<List<ExhibitionAreaListOutput>>(result, 1, 0, result.Entity.Count, "获取农场展区列表成功!");
                }
                else
                {
                    result.IsSuccess = false;
                    SetJosnResult<List<ExhibitionAreaListOutput>>(result, 1, 0, 0, "获取农场展区列表失败!");
                }

                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "删除示范农场展区"
        [HttpPost]
        public JsonResult DeleteExhibitionArea(int farmId, int areaId)
        {
            using (var result = new ResponseResult<object>())
            {
                var data = _farmAreaService.GetAll(m => m.FarmId == farmId && m.Id == areaId).FirstOrDefault();
                if (data == null)
                {
                    result.IsSuccess = false;
                    result.Message = "指定的展区不存在!";
                }
                else
                {
                    result.IsSuccess = _farmAreaService.Delete(data) > 0;
                }

                return new JsonResultEx(result);
            }
        }
        #endregion
    }
}