using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface ILearningGardenCarousel : IRepositoryBase<T_LEARNING_GARDEN_CAROUSEL>
    {
        /// <summary>
        /// 获取推荐文章列表
        /// </summary>
        /// <param name="catId">文章分类编号</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        List<T_LEARNING_GARDEN_CAROUSEL> GetRecommendedList(int catId,string orderBy);
    }
}
