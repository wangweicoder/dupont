// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : 毛文君
// Created          : 12-14-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-29-2015
// ***********************************************************************
// <copyright file="IRepositoryBase.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        int Insert(TEntity entity);
        Task<int> InsertAsync(TEntity entity);

        int Insert(IEnumerable<TEntity> entities);
        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        int Count(Expression<Func<TEntity, bool>> predicate);

        int Count();
        Task<int> CountAsync();


        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll();


        Task<IEnumerable<TEntity>> GetAllAsync();

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll<TKey>(Expression<Func<TEntity, bool>> predicate, Func<TEntity, TKey> orderBy, Func<TEntity, TKey> orderByDecending, int pageIndex, int pageSize, out long totalCount, string includeTableName = null);

        TEntity GetByKey(object key);
        Task<TEntity> GetByKeyAsync(object key);

        int Update(TEntity entity);

        int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);

        Task<int> UpdateAsync(TEntity entity);

        int Update(IEnumerable<TEntity> entities);
        Task<int> UpdateAsync(IEnumerable<TEntity> entities);

        int Delete(string primaryKeyName, int id);
        int Delete(TEntity entity);

        /// <summary>
        /// 按条件进行物理删除(请忽必小心操作)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteAsync(TEntity entity);

        int Delete(IEnumerable<TEntity> entities);
        Task<int> DeleteAsync(IEnumerable<TEntity> entities);
        List<TResult> SqlQuery<TResult>(string sql, params DbParameter[] parameters);
    }
}
