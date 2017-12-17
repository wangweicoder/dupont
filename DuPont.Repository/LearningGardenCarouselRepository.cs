
// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 12-09-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-09-2015
// ***********************************************************************
// <copyright file="LearningGardenCarouselRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Interface;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Data.Entity;
using DuPont.Models.Models;

namespace DuPont.Repository
{
    public class LearningGardenCarouselRepository : BaseRepository<T_LEARNING_GARDEN_CAROUSEL>, ILearningGardenCarousel
    {
        private static int TopCount = 3;//最多能获取的总条数

        private IArticle _articleService;
        public LearningGardenCarouselRepository(IArticle articleService)
        {
            _articleService = articleService;
        }

        #region "获取推荐的文章列表"
        public List<T_LEARNING_GARDEN_CAROUSEL> GetRecommendedList(int catId, string orderBy)
        {
            if (catId > 1)
            {
                return this.GetRecommendListByCatId(catId, orderBy);
            }
            else
            {
                return this.GetRecommendList(orderBy);
            }
        }

        /// <summary>
        /// 按分类获取推荐的文章列表
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="orderBy"></param>
        /// <seealso cref="DuPont.Utility.Document\学习园地获取轮播图列表流程图.bmp"/>
        /// <returns></returns>
        private List<T_LEARNING_GARDEN_CAROUSEL> GetRecommendListByCatId(int catId, string orderBy)
        {
            using (var ctx = GetDbContextInstance())
            {
                //按分类查询文章轮播图表数据条目
                var carouselCountByCattory = Count(m => !m.T_ARTICLE.IsDeleted && m.CatId == catId && m.T_ARTICLE.Content.Contains("<img"));

                //如果文章轮播图表数据条目为0条
                if (carouselCountByCattory == 0)
                {
                    var query = ctx.Set<T_ARTICLE>().Where(m => !m.IsDeleted && m.CatId == catId && m.Content.Contains("<img"));
                    if (orderBy == "-click")
                    {
                        return query.OrderByDescending(m => m.Click).Take(TopCount).ToList()
                            .Select(m => new T_LEARNING_GARDEN_CAROUSEL
                        {
                            ArticleId = m.Id,
                            CatId = m.CatId,
                            T_ARTICLE = m,
                            //T_ARTICLE_CATEGORY = new T_ARTICLE_CATEGORY() { CategoryId = m.CatId }

                        }).ToList();
                    }

                    return query.OrderByDescending(m => m.CreateTime).Take(TopCount).ToList()
                    .Select(m => new T_LEARNING_GARDEN_CAROUSEL
                    {
                        ArticleId = m.Id,
                        CatId = m.CatId,
                        T_ARTICLE = m,
                        //T_ARTICLE_CATEGORY = new T_ARTICLE_CATEGORY() { CategoryId = m.CatId }

                    }).ToList();
                }


                //如果够TopCount条,则一次性将这些数据返回
                if (carouselCountByCattory >= TopCount)
                {
                    #region "仅从T_LEARNING_GARDEN_CAROUSEL表获取"
                    var query = from m in ctx.Set<T_LEARNING_GARDEN_CAROUSEL>()
                                join article in ctx.Set<T_ARTICLE>() on m.ArticleId equals article.Id
                                where !article.IsDeleted && article.CatId == catId && m.T_ARTICLE.Content.Contains("<img")
                                select m;

                    if (orderBy == "-click")
                    {
                        query = query.OrderByDescending(m => m.T_ARTICLE.Click);
                    }
                    else
                    {
                        query = query.OrderByDescending(m => m.T_ARTICLE.CreateTime);
                    }

                    return query.Include(m=>m.T_ARTICLE).Take(TopCount).ToList();
                    #endregion
                }

                //如果不够TopCount条,则分别按分类从文章轮播图表和文章表各读取符合TopCount条的数据
                var carouselData1 = new List<T_LEARNING_GARDEN_CAROUSEL>();
                var carouselData2 = new List<T_LEARNING_GARDEN_CAROUSEL>();
                if (orderBy == "-click")
                {
                    //按分类查询文章轮播图表形成数据1
                    carouselData1 = ctx.Set<T_LEARNING_GARDEN_CAROUSEL>()
                         .Include(m => m.T_ARTICLE)
                         .Where(m => !m.T_ARTICLE.IsDeleted && m.CatId == catId && m.T_ARTICLE.Content.Contains("<img"))
                         .OrderByDescending(m => m.T_ARTICLE.Click)

                        //.Include(m => m.T_ARTICLE_CATEGORY)
                         .ToList();

                    var carouselData1IdList = carouselData1.Select(m => m.ArticleId).ToList();
                    //按分类查询文章表形成数据2
                    carouselData2 = ctx.Set<T_ARTICLE>().Where(m => !m.IsDeleted && m.CatId == catId && !carouselData1IdList.Contains(m.Id) && m.Content.Contains("<img"))
                        .OrderByDescending(m => m.Click)
                        .Take(TopCount - carouselData1.Count)
                        //.Include(m => m.T_ARTICLE_CATEGORY)
                        .ToList().Select(m => new T_LEARNING_GARDEN_CAROUSEL
                        {
                            ArticleId = m.Id,
                            CatId = m.CatId,
                            T_ARTICLE = m,
                            //T_ARTICLE_CATEGORY = m.T_ARTICLE_CATEGORY
                        }).ToList();
                }
                else
                {
                    //按分类查询文章轮播图表形成数据1
                    carouselData1 = ctx.Set<T_LEARNING_GARDEN_CAROUSEL>()
                        .Include(m => m.T_ARTICLE)
                        .Where(m => !m.T_ARTICLE.IsDeleted && m.CatId == catId && m.T_ARTICLE.Content.Contains("<img"))
                         .OrderByDescending(m => m.T_ARTICLE.CreateTime)
                         .ToList();

                    var carouselData1IdList = carouselData1.Select(m => m.ArticleId).ToList();
                    //按分类查询文章表形成数据2
                    carouselData2 = ctx.Set<T_ARTICLE>().Where(m => !m.IsDeleted && m.CatId == catId && !carouselData1IdList.Contains(m.Id) && m.Content.Contains("<img"))
                        .OrderByDescending(m => m.CreateTime)
                        .Take(TopCount - carouselData1.Count)
                        //.Include(m => m.T_ARTICLE_CATEGORY)
                        .ToList().Select(m => new T_LEARNING_GARDEN_CAROUSEL
                        {
                            ArticleId = m.Id,
                            CatId = m.CatId,
                            T_ARTICLE = m,
                            //T_ARTICLE_CATEGORY = m.T_ARTICLE_CATEGORY
                        }).ToList();
                }

                if (carouselData2.Count > 0)
                    carouselData1.AddRange(carouselData2);

                return carouselData1;

            }

        }

        /// <summary>
        /// 不按分类获取推荐的文章列表
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="orderBy"></param>
        /// <seealso cref="DuPont.Utility.Document\学习园地获取轮播图列表流程图.bmp"/>
        /// <returns></returns>
        private List<T_LEARNING_GARDEN_CAROUSEL> GetRecommendList(string orderBy)
        {
            using (var ctx = GetDbContextInstance())
            {
                //按分类查询文章轮播图表数据条目
                var carouselCountByCattory = Count(m => !m.T_ARTICLE.IsDeleted && m.T_ARTICLE.Content.Contains("<img"));

                #region "如果文章轮播图表数据条目为0条"
                //如果文章轮播图表数据条目为0条
                if (carouselCountByCattory == 0)
                {
                    var query = ctx.Set<T_ARTICLE>().Where(m => !m.IsDeleted && m.Content.Contains("<img"));
                    if (orderBy == "-click")
                    {
                        return query.OrderByDescending(m => m.Click).Take(TopCount)
                            .Include(m => m.T_ARTICLE_CATEGORY)
                            .ToList()
                            .Select(m => new T_LEARNING_GARDEN_CAROUSEL
                            {
                                ArticleId = m.Id,
                                CatId = m.CatId,
                                T_ARTICLE = m,
                                T_ARTICLE_CATEGORY = m.T_ARTICLE_CATEGORY

                            }).ToList();
                    }

                    return query.OrderByDescending(m => m.CreateTime).Take(TopCount)
                    .Include(m => m.T_ARTICLE_CATEGORY)
                    .ToList()
                    .Select(m => new T_LEARNING_GARDEN_CAROUSEL
                    {
                        ArticleId = m.Id,
                        CatId = m.CatId,
                        T_ARTICLE = m,
                        T_ARTICLE_CATEGORY = m.T_ARTICLE_CATEGORY
                    }).ToList();
                }
                #endregion

                #region "如果够TopCount条,则一次性将这些数据返回"
                //如果够TopCount条,则一次性将这些数据返回
                if (carouselCountByCattory >= TopCount)
                {
                    var query = from m in ctx.Set<T_LEARNING_GARDEN_CAROUSEL>()
                                join article in ctx.Set<T_ARTICLE>() on m.ArticleId equals article.Id
                                where !article.IsDeleted && article.Content.Contains("<img")
                                select m;

                    if (orderBy == "-click")
                    {
                        query = query.OrderByDescending(m => m.T_ARTICLE.Click);
                    }
                    else
                    {
                        query = query.OrderByDescending(m => m.T_ARTICLE.CreateTime);
                    }

                    return query.Include(m => m.T_ARTICLE).Take(TopCount).ToList();
                }
                #endregion

                #region "如果不够TopCount条,则分别按分类从文章轮播图表和文章表各读取符合TopCount条的数据"
                //如果不够TopCount条,则分别按分类从文章轮播图表和文章表各读取符合TopCount条的数据
                var carouselData1 = new List<T_LEARNING_GARDEN_CAROUSEL>();
                var carouselData2 = new List<T_LEARNING_GARDEN_CAROUSEL>();
                if (orderBy == "-click")
                {
                    //按分类查询文章轮播图表形成数据1
                    carouselData1 = ctx.Set<T_LEARNING_GARDEN_CAROUSEL>()
                         .Where(m => !m.T_ARTICLE.IsDeleted && m.T_ARTICLE.Content.Contains("<img"))
                         .OrderByDescending(m => m.T_ARTICLE.Click)
                         .Include(m => m.T_ARTICLE)
                         .ToList();

                    var carouselData1IdList = carouselData1.Select(m => m.ArticleId).ToList();
                    //按分类查询文章表形成数据2
                    carouselData2 = ctx.Set<T_ARTICLE>()
                        .Where(m => !m.IsDeleted && !carouselData1IdList.Contains(m.Id))
                        .OrderByDescending(m => m.Click)
                        .Take(TopCount - carouselData1.Count)
                        //.Include(m => m.T_ARTICLE_CATEGORY)
                        .ToList().Select(m => new T_LEARNING_GARDEN_CAROUSEL
                        {
                            ArticleId = m.Id,
                            CatId = m.CatId,
                            T_ARTICLE = m,
                            //T_ARTICLE_CATEGORY = m.T_ARTICLE_CATEGORY
                        }).ToList();
                }
                else
                {
                    //按分类查询文章轮播图表形成数据1
                    carouselData1 = ctx.Set<T_LEARNING_GARDEN_CAROUSEL>()
                        .Where(m => !m.T_ARTICLE.IsDeleted && m.T_ARTICLE.Content.Contains("<img"))
                         .OrderByDescending(m => m.T_ARTICLE.CreateTime)
                         .Include(m => m.T_ARTICLE)
                         .ToList();

                    var carouselData1IdList = carouselData1.Select(m => m.ArticleId).ToList();
                    //按分类查询文章表形成数据2
                    carouselData2 = ctx.Set<T_ARTICLE>()
                        .Where(m => !m.IsDeleted && !carouselData1IdList.Contains(m.Id))
                        .OrderByDescending(m => m.CreateTime)
                        .Take(TopCount - carouselData1.Count)
                        //.Include(m => m.T_ARTICLE_CATEGORY)
                        .ToList().Select(m => new T_LEARNING_GARDEN_CAROUSEL
                        {
                            ArticleId = m.Id,
                            CatId = m.CatId,
                            T_ARTICLE = m,
                            //T_ARTICLE_CATEGORY = m.T_ARTICLE_CATEGORY
                        }).ToList();
                }

                if (carouselData2.Count > 0)
                    carouselData1.AddRange(carouselData2);

                return carouselData1;
                #endregion
            }
        }
        #endregion
    }
}
