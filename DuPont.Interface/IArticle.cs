
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace DuPont.Interface
{
    public interface IArticle : IRepositoryBase<T_ARTICLE>
    {
        List<T_ARTICLE> GetAll(Expression<Func<T_ARTICLE, bool>> predicate, string orderBy,int pageIndex,int pageSize,bool IsGetCarouselList,out long totalCount);

        T_ARTICLE GetById(long id);

        int DeleteArticle(params int[] articleIds);

        int AddToCarousel(params int[] articleIds);
    }
}
