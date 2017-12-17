// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 08-06-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-06-2015
// ***********************************************************************
// <copyright file="Menu_RoleRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using DuPont.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using DuPont.Models.Models;

namespace DuPont.Repository
{
    public class Menu_RoleRepository : IMenu_Role
    {

        public int Insert(T_MENU_ROLE_RELATION entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T_MENU_ROLE_RELATION> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        public int Delete(T_MENU_ROLE_RELATION entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<T_MENU_ROLE_RELATION> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<T_MENU_ROLE_RELATION, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(T_MENU_ROLE_RELATION entity)
        {
            throw new NotImplementedException();
        }

        public T_MENU_ROLE_RELATION GetByKey(object key)
        {
            throw new NotImplementedException();
        }

        public IList<T_MENU_ROLE_RELATION> GetAll()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_MENU_ROLE_RELATION.ToList();
            }
        }



        public IList<T_MENU_ROLE_RELATION> GetAll(System.Linq.Expressions.Expression<Func<T_MENU_ROLE_RELATION, bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntryList = dbContext.T_MENU_ROLE_RELATION.Where(predicate);
                if (dbEntryList!=null)
                {
                    return dbEntryList.ToList();
                }
            }
            return null;
        }
    }
}
