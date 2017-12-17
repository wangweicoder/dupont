// ***********************************************************************
// Assembly         : DuPont.Repository
// Author           : 毛文君
// Created          : 08-18-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-18-2015
// ***********************************************************************
// <copyright file="AreaRepository.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************


using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DuPont.Repository
{
    public class AreaRepository : IArea
    {
        public int Insert(T_AREA entity)
        {
            throw new NotImplementedException();
        }

        public int Insert(IEnumerable<T_AREA> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(object id)
        {
            throw new NotImplementedException();
        }

        public int Delete(T_AREA entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(IEnumerable<T_AREA> entities)
        {
            throw new NotImplementedException();
        }

        public int Delete(System.Linq.Expressions.Expression<Func<T_AREA, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public int Update(T_AREA entity)
        {
            throw new NotImplementedException();
        }

        public T_AREA GetByKey(object key)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_AREA.Find(key);
            }
        }

        public IList<T_AREA> GetAll()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_AREA.ToList();
            }
        }

        public IList<T_AREA> GetAll(System.Linq.Expressions.Expression<Func<T_AREA, bool>> predicate)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.T_AREA.Where(predicate).ToList();
            }
        }

        public List<AreaViewModel> GetAreaChilds(string parentId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var areaList = dbContext.GET_AREA_CHILDS(parentId);
                var result = from m in areaList
                             orderby m.AID
                             select new AreaViewModel
                             {
                                 AID = m.AID,
                                 DisplayName = m.DisplayName
                             };

                return result.ToList();
            }
        }

        public string GetAreaNamesBy(string areaCodes)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var codes = areaCodes.Split('|');
                string res = string.Empty;

                T_AREA area = null;

                foreach (var code in codes)
                {
                    area = dbContext.T_AREA.FirstOrDefault(a => a.AID == code);
                    res += string.Format("{0}|", area == null ? string.Empty : area.DisplayName);
                }

                res = res.Substring(0, res.Length - 1);

                return res;
            }
        }


        public IEnumerable<VM_GET_PENDING_AUDIT_LIST> vmgetpendingauitlist()
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return this.vmgetpendingauitlist().ToList();
            }
        }

        /// <summary>
        /// 获取管理区域
        /// </summary>
        /// <param name="parentAid">为-1时将获取管理范围的所有级别的地区数据</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<T_AREA> GetManageArea(string parentAid, long userId)
        {
            using (var context = new DuPont_TestContext())
            {
                StringBuilder sb = new StringBuilder();
                var parameters = new List<SqlParameter>(){
                    new SqlParameter("@UserId",userId)
                };

                sb.Append("with cte (AID,ParentAID)");
                sb.Append("as");
                sb.Append("(");
                sb.Append("	select AID,ParentAID from T_AREA where AID in(");
                sb.Append("		SELECT [AID]");
                sb.Append("		FROM [T_SUPPLIERS_AREA]");
                sb.Append("		where UserID=@UserId and State=1");
                sb.Append("	)");
                sb.Append("	union all");
                sb.Append("	select b.AID,b.ParentAID ");
                sb.Append("	from cte A ,T_AREA B ");
                sb.Append("	where a.ParentAID = b.AID");
                sb.Append(")");
                sb.Append("select * from T_AREA where AID in(");
                sb.Append("	select AID");
                sb.Append("	from cte");
                sb.Append(")");
                if (parentAid != "-1")
                {
                    sb.AppendLine(" AND ParentAID=@ParentAid");
                    parameters.Add(new SqlParameter("@ParentAid", parentAid));
                }

                return context.Database.SqlQuery<T_AREA>(sb.ToString(), parameters.ToArray()).ToList();
            }
        }
    }
}
