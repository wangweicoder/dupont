// ***********************************************************************
// Assembly         : DuPont.API
// Author           : 毛文君
// Created          : 12-09-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-10-2015
// ***********************************************************************
// <copyright file="LearningWorldController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using AutoMapper;




using DuPont.Extensions;
using DuPont.Global.ActionResults;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Interface;
using DuPont.Utility;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Linq;
using System.Linq.Expressions;
using DuPont.Global.Exceptions;
using DuPont.Models.Dtos.Foreground.LearningWorld;
using DuPont.Models.Models;
using DuPont.Entity.Enum;

namespace DuPont.API.Controllers
{
    [Validate]
    public class LearningWorldController : Controller
    {
        private readonly IArticle _article;
        private readonly IArticleCategory _articleCategory;
        private ILearningGardenCarousel _learningGardenCarouselService;
        private readonly IFarm _farmService;
        private readonly IFarmArea _farmAreaService;
        private readonly IUser _userService;
        private readonly IFarmBooking _farmBookingService;
        public LearningWorldController(IArticle article, IArticleCategory articleCategory,
            ILearningGardenCarousel learnGardenCarousel, IFarm farmService,
            IFarmArea farmAreaService, IUser userService, IFarmBooking farmBookingService)
        {
            _article = article;
            _articleCategory = articleCategory;
            _learningGardenCarouselService = learnGardenCarousel;
            _farmService = farmService;
            _farmAreaService = farmAreaService;
            _userService = userService;
            _farmBookingService = farmBookingService;
        }

        #region "获取导航菜单列表"
        [HttpPost]
        public JsonResult NavMenu()
        {
            using (var result = new ResponseResult<List<NavigateMenuItemOutput>>())
            {
                var list = _articleCategory.GetAll();
                result.Entity = Mapper.Map<List<NavigateMenuItemOutput>>(list);
                result.TotalNums = result.Entity.Count;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "获取轮播图片列表"
        [HttpPost]
        public JsonResult CarouselPictures(CarouselPictureSearchInput input)
        {
            var regexImg = new Regex(@"(<img[\s\S]+?src="".+?""){1}");
            var regexUrl = new Regex(@"http[\s\S]+?(?="")");
            var regexHtmlTag = new Regex(@"[<][\s\S]+?[>]");
            using (var result = new ResponseResult<List<CarouselPictureOutput>>())
            {
                long totalCount = 3;
                List<T_ARTICLE> list = null;
                if (input.CatId > 1)
                {
                    list = _article.GetAll(m => !m.IsDeleted && m.CatId == input.CatId && m.IsPutOnCarousel, input.OrderBy, 1, 3, true, out totalCount);
                }
                else
                {
                    list = _article.GetAll(m => !m.IsDeleted && m.IsPutOnCarousel, input.OrderBy, 1, 3, true, out totalCount);
                }

                result.Entity = Mapper.Map<List<CarouselPictureOutput>>(list);
                result.TotalNums = result.Entity.Count;
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (!string.IsNullOrEmpty(item.Content))
                    {
                        var img = regexImg.Match(item.Content).Value;
                        result.Entity[i].PictureUrl = regexUrl.Match(img).Value;
                    }
                }
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "获取文章列表"
        [HttpPost]
        public JsonResult ArticleList(ArticleListSearchInput input)
        {
            using (var result = new ResponseResult<List<ArticleListSearchOutput>>())
            {
                List<T_ARTICLE> list = null;
                List<T_ARTICLE> carouselList = null;
                long totalCount;

                if (input.CatId > 1)
                {
                    long carouselTotalCount = 0;
                    carouselList = _article.GetAll(m => !m.IsDeleted && m.CatId == input.CatId && m.IsPutOnCarousel, input.OrderBy, 1, 3, false, out carouselTotalCount);
                    if (carouselTotalCount > 0)
                    {
                        var carouselArticleIdList = carouselList.Select(m => m.Id).ToArray();
                        list = _article.GetAll(m => !m.IsDeleted && m.CatId == input.CatId && !carouselArticleIdList.Contains(m.Id), input.OrderBy, input.PageIndex, input.PageSize, false, out totalCount);
                    }
                    else
                    {
                        list = _article.GetAll(m => !m.IsDeleted && m.CatId == input.CatId, input.OrderBy, input.PageIndex, input.PageSize, false, out totalCount);
                    }
                }
                else
                {
                    long carouselTotalCount = 0;
                    carouselList = _article.GetAll(m => !m.IsDeleted && m.IsPutOnCarousel, input.OrderBy, 1, 3, false, out carouselTotalCount);
                    if (carouselTotalCount > 0)
                    {
                        var carouselArticleIdList = carouselList.Select(m => m.Id).ToArray();
                        list = _article.GetAll(m => !m.IsDeleted && !carouselArticleIdList.Contains(m.Id), input.OrderBy, input.PageIndex, input.PageSize, false, out totalCount);
                    }
                    else
                    {
                        list = _article.GetAll(m => !m.IsDeleted, input.OrderBy, input.PageIndex, input.PageSize, false, out totalCount);
                    }
                }


                result.Entity = Mapper.Map<List<ArticleListSearchOutput>>(list);

                var regexImg = new Regex(@"(<img[\s\S]+?src="".+?""){1}");
                var regexUrl = new Regex(@"http[\s\S]+?(?="")");

                var contentLengthLimit = 28;
                var ellipsisString = "...";
                foreach (var item in result.Entity)
                {
                    var img = regexImg.Match(item.Content).Value;
                    item.PictureUrl = regexUrl.Match(img).Value;
                    var extension = Path.GetExtension(item.PictureUrl);
                    var index = item.PictureUrl.LastIndexOf(extension);
                    item.PictureUrl = item.PictureUrl.Insert(index, string.Format("_{0}x{1}", DuPont.Global.Global.LeanGardenThumbnailWidth, DuPont.Global.Global.LearGardenThumbnailHeight));

                    item.Content = PageValidate.RemoveHtmlTagAndSepecialChar(item.Content);
                    item.Content = item.Content.Length > contentLengthLimit
                        ? item.Content.Substring(0, 28) + ellipsisString
                        : item.Content;
                }
                result.TotalNums = totalCount;
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "文章点击量实时统计"
        [HttpPost]
        public JsonResult Article_Statistics(long ArticleId)
        {
            using (var result = new ResponseResult<object>())
            {
                var article = _article.GetByKey(ArticleId);
                if (article != null)
                {
                    article.Click += 1;
                    _article.Update(article);
                    result.Entity = "ok";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "数据不存在!";
                    result.State.Id = (int)ResponseStatusCode.InvalidArgument;
                    result.State.Description = ResponseStatusCode.InvalidArgument.GetDescription();
                }
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "农场列表"
        [HttpPost]
        public JsonResult FarmList(FarmListInput input)
        {
            using (var result = new ResponseResult<List<FarmListOutput>>())
            {
                var predicate = PredicateBuilder.True<T_DEMONSTRATION_FARM>();
                predicate = predicate.And(m => !m.IsDeleted);
                var areaList = input.Address.Split(new char[] { '|' });
                if (!areaList[0].IsNullOrEmpty())
                {
                    var addrAid = areaList[0];
                    predicate = predicate.And(m => m.ProvinceAid == addrAid);
                }

                if (!areaList[1].IsNullOrEmpty())
                {
                    var addrAid = areaList[1];
                    predicate = predicate.And(m => m.CityAid == addrAid);
                }

                if (!areaList[2].IsNullOrEmpty())
                {
                    var addrAid = areaList[2];
                    predicate = predicate.And(m => m.RegionAid == addrAid);
                }

                var list = _farmService.GetAll(predicate);
                result.Entity = Mapper.Map<List<FarmListOutput>>(list);

                result.Message = "获取农场列表成功!";
                result.TotalNums = list.Count();
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "农场详情"
        [HttpPost]
        public JsonResult FarmDetail(FarmDetailInput input)
        {
            using (var result = new ResponseResult<FarmDetailOutput>())
            {
                var farm = _farmService.GetByKey(input.FarmId);

                var areaList = _farmAreaService.GetAll(m => m.FarmId == input.FarmId);

                var farmDetail = Mapper.Map<FarmDetailOutput>(farm);
                if (areaList != null && areaList.Any())
                {
                    farmDetail.AreaList = Mapper.Map<List<FarmArea>>(areaList);
                }

                result.Entity = farmDetail;
                result.Message = "获取农场详情成功!";
                return new JsonResultEx(result);
            }
        }
        #endregion

        #region "农场预约"
        [HttpPost]
        public JsonResult FarmBooking(FarmBookingInput input)
        {
            using (var result = new ResponseResult<object>())
            {
                //验证用户是否存在
                if (_userService.Count(m => m.Id == input.UserId) == 0)
                    throw new CustomException("用户不存在!");

                //获取农场信息
                var farm = _farmService.GetByKey(input.FarmId);
                if (farm == null || farm.IsDeleted)
                    throw new CustomException("农场不存在!");

                //判断农场是否开放
                if (!farm.IsOpen)
                    throw new CustomException("农场暂未开放!");

                //判断预约时间是否在开放时间内
                if (!(input.VisitDate >= farm.OpenStartDate && input.VisitDate <= farm.OpenEndDate))
                    throw new CustomException("请预约一个有效的日期!");

                //判断是否已经预约过同一个农场
                var existBookingRecord =
                    _farmBookingService.Count(m => m.UserId == input.UserId && m.FarmId == input.FarmId) > 0;
                if (existBookingRecord)
                    throw new CustomException("你已预约过该农场，如需再次预约，请线下联系!");

                if (input.VisitDate != null)
                {
                    var farmBookingRecord = new T_FARM_BOOKING()
                    {
                        FarmId = input.FarmId,
                        UserId = input.UserId,
                        VisitDate = input.VisitDate.Value,
                        CreateTime = DateTime.Now
                    };

                    _farmBookingService.Insert(farmBookingRecord);
                    result.Message = "预约示范农场成功!";
                    return new JsonResultEx(result);
                }

                result.IsSuccess = false;
                result.Message = "预约示范农场失败!";
                return new JsonResultEx(result);
            }
        }
        #endregion
    }
}