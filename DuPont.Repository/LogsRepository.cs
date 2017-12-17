

using DuPont.Interface;
using DuPont.Models.Models;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class LogsRepository : ILog
    {
        public List<LogModel> GetLogListModel(int pageIndex, int pageSize, out long reocrdCount)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                //var areaList = dbContext.T_SYS_LOG.Where(t => 1 == 1).ToList();
                var result = (from m in dbContext.T_SYS_LOG
                              //where m.State == true
                              orderby m.Id descending
                              select new
                              {
                                  Id = m.Id,
                                  Level = m.Level,
                                  StackTrace = m.StackTrace,
                                  Message = m.Message,
                                  CreateTime = m.CreateTime
                              }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                reocrdCount = dbContext.T_SYS_LOG.Count();

                if (result != null)
                {
                    var resultList = new List<LogModel>();
                    foreach (var model in result)
                    {
                        resultList.Add(new LogModel
                        {
                            Id = model.Id,
                            Level = model.Level,
                            StackTrace = model.StackTrace,
                            Message = model.Message,
                            CreateTime = TimeHelper.GetChinaLocalTime(model.CreateTime)
                        });
                    }
                    return resultList;
                }
                return null;
            }

        }
        public int Insert(T_SYS_LOG entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T_SYS_LOG> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        public int Delete(T_SYS_LOG entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<T_SYS_LOG> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<T_SYS_LOG, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(T_SYS_LOG entity)
        {
            throw new NotImplementedException();
        }

        public T_SYS_LOG GetByKey(object key)
        {
            throw new NotImplementedException();
        }
        public IList<T_SYS_LOG> GetAll(System.Linq.Expressions.Expression<Func<T_SYS_LOG, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public IList<T_SYS_LOG> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
