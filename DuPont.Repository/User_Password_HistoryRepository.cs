
// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 曾普
// Created          : 09-28-2015
//
// Last Modified By : 曾普
// Last Modified On : 09-28-2015
// ***********************************************************************
// <copyright file="User_Password_HistoryRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class User_Password_HistoryRepository : IUser_Password_History
    {
        /// <summary>
        /// 根据用户id获得该用户的密码历史记录
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public List<T_USER_PASSWORD_HISTORY> GetPasswordHistoryByUserId(long userId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var historys = from uph in dbContext.T_USER_PASSWORD_HISTORY
                               where uph.UserID == userId
                               orderby uph.CreateTime ascending
                               select uph;

                return historys.ToList();
            }
        }

        /// <summary>
        /// 添加一条用户密码历史记录
        /// </summary>
        /// <param name="entity">记录实体</param>
        /// <returns></returns>
        public int Insert(T_USER_PASSWORD_HISTORY entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_USER_PASSWORD_HISTORY.Add(entity);
                return dbContext.SaveChanges();
            }
        }

        public int Insert(IEnumerable<T_USER_PASSWORD_HISTORY> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除一条密码历史记录
        /// </summary>
        /// <param name="entity">记录实体</param>
        /// <returns></returns>
        public int Delete(T_USER_PASSWORD_HISTORY entity)
        {
            if (entity != null)
            {
                using (var dbContext = new DuPont_TestContext())
                {
                    var entityt = dbContext.T_USER_PASSWORD_HISTORY.Where(a => a.UserID == entity.UserID && a.Password == entity.Password).FirstOrDefault();
                    dbContext.T_USER_PASSWORD_HISTORY.Remove(entityt);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Delete(IEnumerable<T_USER_PASSWORD_HISTORY> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<T_USER_PASSWORD_HISTORY, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(T_USER_PASSWORD_HISTORY entity)
        {
            throw new NotImplementedException();
        }

        public T_USER_PASSWORD_HISTORY GetByKey(object key)
        {
            throw new NotImplementedException();
        }

        public IList<T_USER_PASSWORD_HISTORY> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<T_USER_PASSWORD_HISTORY> GetAll(System.Linq.Expressions.Expression<Func<T_USER_PASSWORD_HISTORY, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
