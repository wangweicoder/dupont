using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface ICommon
    {
        /// <summary>
        ///  公用方法检测typeid是否有效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        bool CheckTypeid<T>(Expression<Func<T, bool>> wherelambda) where T : class;
        /// <summary>
        /// 检测userid是否有效
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        bool CheckUserId(long userid, int key);
        /// <summary>
        /// 公用方法执行修改操作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        int Modify<T>(T entity, Expression<Func<T, bool>> wherelambda) where T : class;
        /// <summary>
        /// 公用方法获取指定model
        /// </summary>
        /// <param name="wherelambda"></param>
        /// <returns></returns>
        T GetById<T>(Expression<Func<T, bool>> wherelambda) where T : class;
        /// <summary>
        /// 公用方法执行添加操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add<T>(T entity) where T : class;

        /// <summary>
        /// 公用方法执行添加操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Add<T>(List<T> entity) where T : class;
        /// <summary>
        /// 公用方法执行删除操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        int Delete<T>(Expression<Func<T, bool>> predicate) where T : class;
        /// <summary>
        /// 获取指定用户坐标
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetCoordinate(long userId);
        /// <summary>
        /// 获取产业商的评价详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        List<CommentDetailModel> GetBusinessCommentDetail(long userid, int pageindex, int pagesize, out long TotalNums);
        /// <summary>
        /// 获取农机手的评价详情
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="TotalNums"></param>
        List<CommentDetailModel> GetOperatorCommentDetail(long userid, int pageindex, int pagesize, out long TotalNums);
        /// <summary>
        /// 获取大农户的评价详情
        /// </summary>GetFarmerCommentDetail
        /// <param name="userid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="TotalNums"></param>
        /// <returns></returns>
        List<CommentDetailModel> GetFarmerCommentDetail(long userid, int pageindex, int pagesize, out long TotalNums);
        /// <summary>
        /// 获取农机手对大农户的评价列表
        /// </summary>GetFarmerCommentList
        /// <param name="userid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="TotalNums"></param>
        /// <author>ww</author>
        /// <returns></returns>
        List<CommentDetailModel> GetFarmerCommentList(long userid, int pageindex, int pagesize, out long TotalNums);
        /// <summary>
        /// 获取人数
        /// </summary>
        /// <param name="roleType">角色类别编号</param>
        /// <param name="userid">发布需求者id</param>
        /// <param name="Province"></param>
        /// <param name="City"></param>
        /// <param name="Region"></param>
        int GetNumber(int roleType, long userid, string Province, string City, string Region);

        /// <summary>
        /// 获取最新版本信息
        /// </summary>
        /// <param name="platform">ios/android</param>
        /// <returns>T_APP_VERSION.</returns>
        T_APP_VERSION GetLastVersion(string platform);

        /// <summary>
        /// 检测数据库的部署（100表示成功）
        /// </summary>
        /// <returns>System.Int32.</returns>
        int CheckDatabaseDeployment();

        /// <summary>
        /// 增加先锋币
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="description">描述</param>
        /// <param name="auditUserId">审核人用户编号</param>
        /// <param name="dpoints">先锋币数.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool AddDuPontPoint(long userId,string description,long auditUserId,int dpoints);

        /// <summary>
        /// 获取附近农机手列表
        /// </summary>
        /// <param name="roleType">角色类别</param>
        /// <param name="Province"></param>
        /// <param name="City"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [Obsolete("please change to use GetOperatorsForFarmerRequirementWithDemandType method!", true)]
        List<OperatorProfile> GetOperatorsForFarmerRequirement(string Province, string City, int pageIndex, int pageSize,out long totalNums);

        /// <summary>
        /// 获取附近农机手列表(带需求类型编号过滤)
        /// </summary>
        /// <param name="roleType">角色类别</param>
        /// <param name="Province"></param>
        /// <param name="City"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<OperatorProfile> GetOperatorsForFarmerRequirementWithDemandType(string Province, string City, int pageIndex, int pageSize,int demandTypeId, out long totalNums);
    }
}
