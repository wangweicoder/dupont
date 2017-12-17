
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
    public class APP_VersionRepository : IApp_Version
    {
        public int Insert(T_APP_VERSION entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T_APP_VERSION> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        public int Delete(T_APP_VERSION entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<T_APP_VERSION> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<T_APP_VERSION, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(T_APP_VERSION entity)
        {
            throw new NotImplementedException();
        }

        public T_APP_VERSION GetByKey(object key)
        {
            throw new NotImplementedException();
        }

        public IList<T_APP_VERSION> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<T_APP_VERSION> GetAll(System.Linq.Expressions.Expression<Func<T_APP_VERSION, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 保存上传的app信息
        /// </summary>
        /// <param name="entity">app信息实体</param>
        /// <returns></returns>
        public bool SaveAppFile(T_APP_VERSION entity)
        {
            bool result = true;
            using (var dbContext = new DuPont_TestContext())
            {
                var existEntity = dbContext.T_APP_VERSION.Where(a => a.Platform == entity.Platform).FirstOrDefault();
                int saveRes = 0;
                if (existEntity != null)
                {
                    entity.CREATE_DATE = existEntity.CREATE_DATE;
                    entity.Id = existEntity.Id;
                    ClassValueCopyHelper.Copy(existEntity, entity);
                    saveRes = dbContext.SaveChanges();
                }
                else
                {
                    entity.UPDATE_DATE = null;
                    dbContext.T_APP_VERSION.Add(entity);
                    saveRes = dbContext.SaveChanges();
                }
            }

            return result;
        }
    }
}
