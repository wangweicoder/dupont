
using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class FileInfoRepository : IFileInfoRepository
    {
        public int Insert(T_FileInfo entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                try
                {
                    dbContext.T_FileInfo.Add(entity);
                    return dbContext.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    var sbMessage = new StringBuilder("【EF错误】:\r\n");
                    if (dbEx.EntityValidationErrors != null)
                    {
                        foreach (var error in dbEx.EntityValidationErrors)
                        {
                            foreach (var errorChild in error.ValidationErrors)
                            {
                                sbMessage.AppendLine(string.Format("{0}:{1}", errorChild.PropertyName, errorChild.ErrorMessage));
                            }
                        }
                    }
                    throw new Exception(sbMessage.ToString());
                }
            }
        }

        public int Insert(IEnumerable<T_FileInfo> entities)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_FileInfo.AddRange(entities);
                return dbContext.SaveChanges();
            }
        }

        public int Delete(object id)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_FileInfo.Find(id);
                if (dbEntry != null)
                {
                    dbContext.T_FileInfo.Remove(dbEntry);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Delete(T_FileInfo entity)
        {
            if (entity != null)
            {
                using (var dbContext = new DuPont_TestContext())
                {
                    dbContext.T_FileInfo.Remove(entity);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Delete(IEnumerable<T_FileInfo> entities)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                dbContext.T_FileInfo.RemoveRange(entities);
                return dbContext.SaveChanges();
            }
        }

        public int Delete(Expression<Func<T_FileInfo, bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_FileInfo.SingleOrDefault(predicate);
                if (dbEntry != null)
                {
                    dbContext.T_FileInfo.Remove(dbEntry);
                    return dbContext.SaveChanges();
                }
            }
            return 0;
        }

        public int Update(T_FileInfo entity)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntry = dbContext.T_FileInfo.Find(entity.Id);
                if (dbEntry != null)
                {
                    ClassValueCopyHelper.Copy(dbEntry, entity);
                    return dbContext.SaveChanges();
                }
            }
            return 0;

        }

        public T_FileInfo GetByKey(object key)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_FileInfo.Find(key);
            }
        }

        public IList<T_FileInfo> GetAll()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_FileInfo.ToList();
            }
        }


        public IList<T_FileInfo> GetAll(System.Linq.Expressions.Expression<Func<T_FileInfo, bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var dbEntryList = dbContext.T_FileInfo.Where(predicate);
                if (dbEntryList != null)
                {
                    return dbEntryList.ToList();
                }
            }
            return null;
        }

    }
}
