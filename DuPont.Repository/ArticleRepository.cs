
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
// <copyright file="ArticleRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.Entity;
using DuPont.Models.Models;

namespace DuPont.Repository
{
    public class ArticleRepository : BaseRepository<T_ARTICLE>, IArticle
    {

        public System.Collections.Generic.List<T_ARTICLE> GetAll(System.Linq.Expressions.Expression<System.Func<T_ARTICLE, bool>> predicate, string orderBy, int pageIndex, int pageSize, bool IsGetCarouselList, out long totalCount)
        {
            List<T_ARTICLE> list = null;
            using (var ctx = GetDbContextInstance())
            {
                var query = ctx.Set<T_ARTICLE>()
                      .Where(predicate);

                //取出该分类的带轮播的3条文章，并在查询文章的时候忽略掉这些文章
                var carouselIds = query.Where(m => m.IsPutOnCarousel == true)
                    .OrderByDescending(m => m.UpdateTime)
                    .Select(m => m.Id)
                    .Take(3)
                    .ToArray<long>();

                if (!IsGetCarouselList && carouselIds.Length > 0)
                {
                    query = query.Where(m => !carouselIds.Contains(m.Id));
                }

                totalCount = query.Count();
                switch (orderBy)
                {
                    case "-click":
                        list = query.OrderByDescending(m => m.Click)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .Include(m => m.T_ARTICLE_CATEGORY)
                            .ToList();
                        break;
                    case "-date":
                    default:
                        list = query.OrderByDescending(m => m.CreateTime)
                            .Skip((pageIndex - 1) * pageSize)
                            .Take(pageSize)
                            .Include(m => m.T_ARTICLE_CATEGORY)
                            .ToList();
                        break;
                }

                return list;
            }
        }


        public T_ARTICLE GetById(long id)
        {
            using (var ctx = GetDbContextInstance())
            {
                var query = from m in ctx.Set<T_ARTICLE>()
                            join cat in ctx.Set<T_ARTICLE_CATEGORY>() on m.CatId equals cat.CategoryId
                            where m.Id == id
                            select new
                            {
                                Id = m.Id,
                                CatId = m.CatId,
                                Url = m.Url,
                                Click = m.Click,
                                Content = m.Content,
                                CreateTime = m.CreateTime,
                                Title = m.Title,
                                CatName = cat.Name,
                                UpdateTime = m.UpdateTime
                            };
                var result = query.FirstOrDefault();
                if (result == null)
                    return null;

                var article = new T_ARTICLE
                {
                    Id = result.Id,
                    CatId = result.CatId,
                    Url = result.Url,
                    Click = result.Click,
                    Content = result.Content,
                    CreateTime = result.CreateTime,
                    Title = result.Title,
                    UpdateTime = result.UpdateTime,
                    T_ARTICLE_CATEGORY = new T_ARTICLE_CATEGORY
                    {
                        CategoryId = result.CatId,
                        Name = result.CatName
                    }
                };
                return article;
            }
        }
        #region "删除文章(一个或多个)"
        public int DeleteArticle(params int[] articleIds)
        {
            using (var ctx = GetDbContextInstance())
            {
                return ctx.Database.ExecuteSqlCommand("update " + typeof(T_ARTICLE).Name + " set IsDeleted=1 where Id in(" + string.Join(",", articleIds) + ")");
            }
        }
        #endregion


        public int AddToCarousel(params int[] articleIds)
        {
            using (var ctx = GetDbContextInstance())
            {
                return ctx.Database.ExecuteSqlCommand("update " + typeof(T_ARTICLE).Name + " set IsPutOnCarousel=1 where Id in(" + string.Join(",", articleIds) + ")");
            }
        }
    }
}
