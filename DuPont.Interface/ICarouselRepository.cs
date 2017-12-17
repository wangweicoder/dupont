using DuPont.Models.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface ICarouselRepository : IRepositoryBase<T_CAROUSEL>
    {
        /// <summary>
        /// 根据角色获取轮播图列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<CarouselFile> GetCarouselFiles(int roleId);
    }
}
