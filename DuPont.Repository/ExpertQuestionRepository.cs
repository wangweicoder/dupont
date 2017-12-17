using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DuPont.Interface;
using DuPont.Models.Models;
namespace DuPont.Repository
{
    public class ExpertQuestionRepository : BaseRepository<T_QUESTION>, IExpertQuestion
    {

        public List<T_QUESTION> Search(System.Linq.Expressions.Expression<Func<T_QUESTION, bool>> predicate, int pageIndex, int pageSize, string orderBy, out long totalCount)
        {
            //目前不理会OrderBy参数，默认按日期降序排序
            using (var ctx = GetDbContextInstance())
            {
                var query = ctx.Set<T_QUESTION>().Include("User").Where(predicate);

                totalCount = query.Count();

                query = query.OrderByDescending(m => m.CreateTime)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);

                return query.ToList();
            }
        }
    }
}
