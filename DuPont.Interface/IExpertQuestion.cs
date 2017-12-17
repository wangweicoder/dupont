using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Models.Models;
using System.Linq.Expressions;

namespace DuPont.Interface
{
    public interface IExpertQuestion : IRepositoryBase<T_QUESTION>
    {
        List<T_QUESTION> Search(Expression<Func<T_QUESTION, bool>> predicate, int pageIndex, int pageSize, string orderBy, out long totalCount);
    }
}
