
// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 08-23-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-23-2015
// ***********************************************************************
// <copyright file="SmsMessageRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using DuPont.Utility;
using DuPont.Models.Models;

namespace DuPont.Repository
{
    public class SmsMessageRepository : ISmsMessage
    {
        public int Insert(T_SMS_MESSAGE entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_SMS_MESSAGE.Add(entity);
                return dbContext.SaveChanges();
            }
        }

        public int Insert(IEnumerable<T_SMS_MESSAGE> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        public int Delete(T_SMS_MESSAGE entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<T_SMS_MESSAGE> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<T_SMS_MESSAGE, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(T_SMS_MESSAGE entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_SMS_MESSAGE.Find(entity.Id);
                if (dbEntry != null)
                {
                    ClassValueCopyHelper.Copy(dbEntry, entity);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public T_SMS_MESSAGE GetByKey(object key)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var keyString = Convert.ToString(key);
                return dbContext.T_SMS_MESSAGE.Where(m => m.PhoneNumber == keyString).FirstOrDefault();
            }
        }

        public IList<T_SMS_MESSAGE> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<T_SMS_MESSAGE> GetAll(System.Linq.Expressions.Expression<Func<T_SMS_MESSAGE, bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntryList = dbContext.T_SMS_MESSAGE.Where(predicate);
                if (dbEntryList != null)
                {
                    return dbEntryList.ToList();
                }
            }
            return null;
        }
    }
}
