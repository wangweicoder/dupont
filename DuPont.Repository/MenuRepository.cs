// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 08-06-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-06-2015
// ***********************************************************************
// <copyright file="MenuRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using DuPont.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using DuPont.Models.Models;
using EntityFramework.Extensions;

namespace DuPont.Repository
{
    public class MenuRepository : IMenu
    {

        public MenuRepository()
        {

        }
        public int Insert(T_MENU entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T_MENU> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        public int Delete(T_MENU entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<T_MENU> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<T_MENU, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(T_MENU entity)
        {
            throw new NotImplementedException();
        }

        public T_MENU GetByKey(object key)
        {
            throw new NotImplementedException();
        }

        public IList<T_MENU> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<T_MENU> GetAll(System.Linq.Expressions.Expression<Func<T_MENU,bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntryList = dbContext.T_MENU.Where(predicate);
                if (dbEntryList != null)
                {
                    return dbEntryList.ToList();
                }
            }
            return null;
        }
        public IList<T_MENU> GetMenuList(System.Linq.Expressions.Expression<Func<T_MENU, bool>> predicate, int pageIndex, int pageSize, out long reocrdCount)
        {

            using (var dbContext = new DuPont_TestContext())
            {
                var listQuery = dbContext.T_MENU.Where(predicate)
                       .OrderBy(p => p.ParentId).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var totalCountQuery = dbContext.T_MENU.Where(predicate).ToList().Count;
                reocrdCount = totalCountQuery;
                return listQuery;
            }
        }
    }
}
