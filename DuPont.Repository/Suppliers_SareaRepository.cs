

using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class Suppliers_SareaRepository : ISuppliers_Sarea
    {
        public List<Suppliers_AreaModel> GetManageArea(long UserId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var areaList = dbContext.T_SUPPLIERS_AREA.Where(t => t.UserID == UserId).ToList();
                var result = from m in areaList
                             where m.State == true
                             orderby m.AID
                             select new Suppliers_AreaModel
                             {
                                 UserID = m.UserID,
                                 AID = m.AID,
                                 CreateDateTime = m.CreateDateTime,
                                 State = m.State
                             };

                return result.ToList();
            }

        }
        public int Insert(T_SUPPLIERS_AREA entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T_SUPPLIERS_AREA> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        public int Delete(T_SUPPLIERS_AREA entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<T_SUPPLIERS_AREA> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<T_SUPPLIERS_AREA, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(T_SUPPLIERS_AREA entity)
        {
            throw new NotImplementedException();
        }

        public T_SUPPLIERS_AREA GetByKey(object key)
        {
            throw new NotImplementedException();
        }
        public IList<T_SUPPLIERS_AREA> GetAll(System.Linq.Expressions.Expression<Func<T_SUPPLIERS_AREA, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public IList<T_SUPPLIERS_AREA> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
