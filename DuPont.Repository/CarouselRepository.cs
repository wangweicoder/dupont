

using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class CarouselRepository : BaseRepository<T_CAROUSEL>, ICarouselRepository
    {

        public List<CarouselFile> GetCarouselFiles(int roleId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var res = from c in dbContext.T_CAROUSEL
                          join f in dbContext.T_FileInfo on c.FileId equals f.Id
                          where c.RoleId == roleId
                          orderby c.Order
                          select new CarouselFile
                          {
                              Id = c.Id,
                              FilePath = f.Path,
                              Order = c.Order
                          };
                return res == null ? null : res.ToList();
            }
        }
    }
}
