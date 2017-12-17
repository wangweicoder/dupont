using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface IRepository<T> where T: class
    {
        #region public method

        int Insert(T entity);

        int Insert(IEnumerable<T> entities);

        int Delete(object id);

        int Delete(T entity);

        int Delete(IEnumerable<T> entities);

        int Delete(Expression<Func<T, bool>> predicate);

        int Update(T entity);

        T GetByKey(object key);

        IList<T> GetAll();

        IList<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> predicate);


        #endregion
    }
}
