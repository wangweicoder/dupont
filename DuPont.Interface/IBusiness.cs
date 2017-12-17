using DuPont.Models.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DuPont.Models.Dtos.Background.Demand;

namespace DuPont.Interface
{
    public interface IBusiness:IRepository<T_BUSINESS_PUBLISHED_DEMAND>
    {
        /// <summary>
        /// 产业商获取我的应答列表
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="isclosed"></param>
        /// <param name="userid">产业商id</param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        List<ReplyModel> GetReplyList(int pageindex, int pagesize, int isclosed, long userid, out long TotalNums);
        /// <summary>
        /// 分页获取产业商需求
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IList<T_BUSINESS_PUBLISHED_DEMAND> GetAll(BusinessSeachModel model, out int totalCount);
        /// <summary>
        /// 应答详情
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <returns></returns>
        BusinessReplyDetailModel GetReplyDetail(long id);
        /// <summary>
        /// 产业商发布给大农户的所有需求
        /// </summary>
        /// <param name="farmerLat">登陆者的纬度值</param>
        /// <param name="farmerLng">登陆者的经度值</param>
        /// <param name="type">需求类别编号</param>
        /// <param name="region">区县编号（可选）</param>
        /// <param name="orderfield">排序标识（可选）</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        List<PublishedModel> GetRequirementList(long userId,double farmerLat, double farmerLng, int type, string pronvices, string city, string region, string orderfield, int PageIndex, int PageSize, out long TotalNums);
        /// <summary>
        /// 用于产业商关闭需求
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool GetScore(long id);

        /// <summary>
        /// 获取需求应答列表（后台用）
        /// </summary>
        /// <param name="demandId"></param>
        /// <returns></returns>
        List<DemandReplyItem> GetDemandReplyList(long demandId);
        /// <summary>
        /// 产业商发布给大农户的所有需求（未登录）
        /// </summary>
        /// <author>ww</author>
        /// <param name="type">需求类别编号</param>        
        /// <param name="orderfield">排序标识（可选）</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        List<PublishedModel> GetRequirementListByTime( int type, string orderfield, int PageIndex, int PageSize, out long TotalNums);
        /// <summary>
        /// 产业商获取我的应答列表
        /// </summary>
        /// <param name="ReceiveRoleId">应答人的角色编号</param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="isclosed"></param>
        /// <param name="userid">产业商id</param>
        /// <param name="TotalNums"></param>
        /// <author>ww</author>
        /// <returns></returns>
        List<CommonReplyModel> GetBusinessReplyList(int ReceiveRoleId,int pageindex, int pagesize, int isclosed, long userid, out long TotalNums);
    }
}
