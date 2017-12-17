using DuPont.Models.Models;

// ***********************************************************************
// Assembly         : DuPont.Interface
// Author           : Ning Wang
// Created          : 08-11-2015
//
// Last Modified By : Ning Wang
// Last Modified On : 08-11-2015
// ***********************************************************************
// <copyright file="IRequirement.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DuPont.Models.Dtos.Background.Demand;
using DuPont.Models;

namespace DuPont.Interface
{
    public interface IFarmerRequirement : IRepository<T_FARMER_PUBLISHED_DEMAND>
    {
        /// <summary>
        /// 获取大农户需求详情
        /// </summary>
        /// <param name="id">需求id</param>
        /// <returns></returns>
        FarmerPublishedDetailsModel GetFarmerDetail(long id);
        /// <summary>
        /// 获取产业商需求详情
        /// </summary>
        /// <param name="id">需求id</param>
        /// <returns></returns>
        BusinessPublishedDetailsModel GetBusinessDetail(long id);

        /// <summary>
        /// 获取大农户发给产业商（农机手）的信息
        /// </summary>
        /// <param name="Lat">登陆者的纬度值</param>
        /// <param name="Lng">登陆者的经度值</param>
        /// <param name="type">需求类型编号</param>
        /// <param name="pronvices">省份</param>
        /// <param name="city">城市</param>
        /// <param name="region">地区</param>
        /// <param name="orderfield">排序字段</param>
        /// <param name="PageIndex">每几页</param>
        /// <param name="PageSize">每页显示数据条数</param>
        /// <param name="TotalNums">符合条件的总记录数</param>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        List<PublishedModel> GetRequirementList(double Lat, double Lng, int type, string pronvices, string city, string region, string orderfield, int PageIndex, int PageSize, out long TotalNums, long userId = 0);
        /// <summary>
        /// 大农户我的应答
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="isclosed">发布状态标识（0进行中，1已关闭）</param>
        /// <param name="userid">大农户id</param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        List<ReplyModel> GetReplyList(int pageindex, int pagesize, int isclosed, long userid, out long TotalNums);
        /// <summary>
        /// 大农户我的应答
        /// </summary>
        /// <param name="ReceiveRoleId">应答人的角色编号</param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="isclosed">发布状态标识（0进行中，1已关闭）</param>
        /// <param name="userid">大农户id</param>
        /// <author>ww</author>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        List<CommonReplyModel> GetFarmerReplyList(int RelceiveRoleId,int pageindex, int pagesize, int isclosed, long userid, out long TotalNums);
        /// <summary>
        /// 大农户应答详情
        /// </summary>
        /// <param name="id">需求id</param>
        /// <returns></returns>
        FarmerReplyDetailModel GetReplyDetail(long id);
        /// <summary>
        /// 根据编号获取区域名称
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        string GetAreaName(string code);
        /// <summary>
        /// 分页获取大农户数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IList<T_FARMER_PUBLISHED_DEMAND> GetAll(FarmerSeachModel model, out int totalCount);

        /// <summary>
        /// 获取需求应答列表（后台用）
        /// </summary>
        /// <param name="demandList"></param>
        /// <returns></returns>
        List<FarmerDemandReplyItem> GetDemandReplyList(long demandId);

        /// <summary>
        /// 分配需要给指定的农机手列表
        /// </summary>
        /// <param name="operatorDemands"></param>
        /// <returns></returns>
        int AssignOperators(List<T_USER_FARMERDEMANDS> operatorDemands);
        /// <summary>
        /// 查询指定农机手的数据
        /// </summary>
        /// <param name="operatorDemands"></param>
        /// <returns></returns>
        int SelectOperators(long FarmerDemandid);
        #region 大农户发布给产业商和农机手的需求列表
        /// <summary>
        /// 获取大农户发给产业商（农机手）的信息
        /// </summary>
        /// <author>ww</author>
        /// <param name="type">需求类别编号</param>        
        /// <param name="orderfield">排序标识（可选）</param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        List<PublishedModel> GetRequirementListForOperatorAndBusiness(int PageIndex, int PageSize, int type, string orderfield, out long TotalNums);
        #endregion
    }
}
