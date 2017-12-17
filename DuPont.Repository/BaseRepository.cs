// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 10-27-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-02-2015
// ***********************************************************************
// <copyright file="BaseRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace DuPont.Repository
{
    public abstract class BaseRepository<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class
    {

        protected DbContext GetDbContextInstance()
        {
            return new DuPont_TestContext();
        }

        protected DuPont_TestContext GetDoPontEntityInstance()
        {
            return new DuPont_TestContext();
        }

        protected void Batch(DbContext dbContext, EntityState state, params TEntity[] entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                var dbEntry = dbContext.Entry<TEntity>(entity);
                dbEntry.State = state;
            }
        }

        protected void Batch<OtherTEntity>(DbContext dbContext, EntityState state, params OtherTEntity[] entities) where OtherTEntity : class
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                var dbEntry = dbContext.Entry<OtherTEntity>(entity);
                dbEntry.State = state;
            }
        }

        public int Insert(TEntity entity)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Added, entity);
                return dbContext.SaveChanges();
            }
        }

        public async Task<int> InsertAsync(TEntity entity)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Added, entity);
                return await dbContext.SaveChangesAsync();
            }
        }

        public int Insert(IEnumerable<TEntity> entities)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Added, entities.ToArray());
                return dbContext.SaveChanges();
            }

        }

        public Task<int> InsertAsync(IEnumerable<TEntity> entities)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Added, entities.ToArray());
                return dbContext.SaveChangesAsync();
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.Set<TEntity>().ToList();
            }
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using (var dbContext = GetDbContextInstance())
            {
                var list = dbContext.Set<TEntity>().ToListAsync();
                if (list == null)
                    return null;

                return Task.FromResult(list.Result.AsEnumerable());
            }
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Set<TEntity>().Where(predicate).ToList();
            }
        }

        public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var dbContext = GetDbContextInstance())
            {
                var list = dbContext.Set<TEntity>().Where(predicate).ToListAsync();
                if (list == null)
                    return null;

                return Task.FromResult(list.Result.AsEnumerable());
            }
        }

        public IEnumerable<TEntity> GetAll<TKey>(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, Func<TEntity, TKey> orderBy, Func<TEntity, TKey> orderByDecending, int pageIndex, int pageSize, out long totalCount, string includeTableName = null)
        {
            using (var ctx = GetDbContextInstance())
            {
                var query = ctx.Set<TEntity>().Where(predicate);
                totalCount = query.Count();
                if (!string.IsNullOrEmpty(includeTableName))
                {
                    foreach (var navigatePropertyName in includeTableName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(navigatePropertyName);
                    }
                }

                if (orderBy != null)
                {
                    return query.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }

                return query.OrderByDescending(orderByDecending).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }


        public TEntity GetByKey(object key)
        {
            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Set<TEntity>().Find(key);
            }
        }

        public Task<TEntity> GetByKeyAsync(object key)
        {
            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Set<TEntity>().FindAsync(key);
            }
        }

        public int Update(TEntity entity)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Modified, entity);
                return dbContext.SaveChanges();
            }
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Modified, entity);
                return dbContext.SaveChangesAsync();
            }
        }

        public int Update(IEnumerable<TEntity> entities)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Modified, entities.ToArray());
                return dbContext.SaveChanges();
            }
        }

        public Task<int> UpdateAsync(IEnumerable<TEntity> entities)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Modified, entities.ToArray());
                return dbContext.SaveChangesAsync();
            }
        }

        public int Delete(TEntity entity)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Deleted, entity);
                return dbContext.SaveChanges();
            }
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Deleted, entity);
                return dbContext.SaveChangesAsync();
            }
        }

        public int Delete(IEnumerable<TEntity> entities)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Deleted, entities.ToArray());

                return dbContext.SaveChanges();
            }
        }

        public Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            using (var dbContext = GetDbContextInstance())
            {
                Batch(dbContext, EntityState.Deleted, entities.ToArray());
                return dbContext.SaveChangesAsync();
            }
        }


        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Set<TEntity>().Where(predicate).Count();
            }
        }

        Task<int> IRepositoryBase<TEntity>.CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Set<TEntity>().Where(predicate).CountAsync();
            }
        }


        public int Count()
        {
            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Set<TEntity>().Count();
            }
        }

        public Task<int> CountAsync()
        {
            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Set<TEntity>().CountAsync();
            }
        }


        public int Delete(string primaryKeyName, int id)
        {
            if (primaryKeyName == null || !Regex.IsMatch(primaryKeyName, "[a-zA-Z]+"))
                throw new ArgumentException("primaryKeyName");

            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Database.ExecuteSqlCommand("delete from dbo." + typeof(TEntity).Name + " where " + primaryKeyName + " =" + id);
            }
        }

        public List<TResult> SqlQuery<TResult>(string sql, params DbParameter[] parameters)
        {
            using (var dbContext = GetDbContextInstance())
            {
                if (parameters == null)
                    return dbContext.Database.SqlQuery<TResult>(sql).ToList();

                return dbContext.Database.SqlQuery<TResult>(sql, parameters).ToList();
            }
        }


        /// <summary>
        /// Deletes the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>System.Int32.</returns>
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            using (var dbContext = GetDbContextInstance())
            {
                return dbContext.Set<TEntity>().Where(predicate).Delete();
            }
        }


        public int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            using (var dbContext = GetDbContextInstance())
            {
                var query = dbContext.Set<TEntity>();
                return query.Where(predicate).Update(updateExpression);
            }
        }
    }
}
