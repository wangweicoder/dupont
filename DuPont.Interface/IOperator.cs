using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface IOperator
    {
       
        /// <summary>
        /// 验证userid是否是产业商id
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        //bool CheckUserId(long userid);
        /// <summary>
        /// 产业商响应大农户需求（检测需求id和产业商id的真实性）
        /// </summary>
        /// <param name="id">大农户的需求id/产业商id</param>
        /// <returns></returns>
        //T_FARMER_PUBLISHED_DEMAND GetById(long id);
        /// <summary>
        /// 执行修改操作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        //int Modify<T>(T entity, Expression<Func<T, bool>> wherelambda) where T : class;
        /// <summary>
        /// 执行产业商响应大农户需求操作
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //int Add(T_FARMER_DEMAND_RESPONSE_RELATION entity);

        /// <summary>
        /// 检测当前产业商是否已经响应此需求过
        /// </summary>
        /// <param name="id">需求id</param>
        /// <param name="userId">产业商id</param>
        /// <returns></returns>
        //bool Isexist(long id, long userId);

        List<ReplyModel> GetReplyList(int pageindex, int pagesize, int isclosed, long userid, out long TotalNums);
        BusinessReplyDetailModel GetReplyDetail(long id);

        /// <summary>
        /// 获得农机手的我的应答
        /// </summary>
        /// <param name="ReceiveRoleId">应答人的角色编号</param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="isclosed"></param>
        /// <param name="userid"></param>
        /// <param name="TotalNums"></param>
        /// <author>ww</author>
        /// <returns></returns>
        List<CommonReplyModel> GetOperatorReplayList(int ReceiveRoleId,int pageindex, int pagesize, int isclosed, long userid, out long TotalNums);
    }
}
