// ***********************************************************************
// Assembly         : DuPont.Presentation
// Author           : 毛文君
// Created          : 10-27-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-07-2015
// ***********************************************************************
// <copyright file="BusinessController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using DuPont.Global.Filters.ActionFilters;
using DuPont.Presentation.Models.Dto.Business;
using DuPont.Utility;
using System.Web.Mvc;

namespace DuPont.Presentation.Controllers
{
    [Validate]
    public class BusinessController : BaseController
    {

        /// <summary>
        /// 产业商发布需求
        /// </summary>
        /// <param name="id">0表示执行添加操作</param>
        /// <param name="userid">产业商id</param>
        /// <param name="Type">需求类型编号</param>
        /// <param name="Dates">预期时间</param>
        /// <param name="Address">地址</param>
        /// <param name="DetailAddress">详细地址</param>
        /// <param name="PurchaseWeight">收购区间类型编号</param>
        /// <param name="CommenceWeight">起购重量</param>
        /// <param name="PhoneNumber">手机号</param>
        /// <param name="Remark">摘要</param>
        /// <param name="cropId">农作物Id</param>
        /// <param name="PurchaseStartPrice">预期最低价格</param>
        /// <param name="PurchaseEndPrice">预期最高价格</param>
        /// <param name="?">The ?.</param>
        /// <returns>JsonResult.</returns>
        public string SaveRequirement(DtoSaveRequirement modle)
        {
            SetJsonHeader();
            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        /// <summary>
        /// 删除指定需求信息
        /// </summary>
        /// <param name="id">需求信息id</param>
        /// <returns>JsonResult.</returns>
        public string RemoveRequirement(DtoRemoveRequirement model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        /// <summary>
        /// 产业商评价需求
        /// </summary>
        /// <param name="isOwn">0表示产业商的需求，1表示大农户的需求</param>
        /// <param name="id">需求编号</param>
        /// <param name="userid">响应者id（接受当前需求）</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string CommentRequirement(DtoCommentRequirement model)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        /// <summary>
        /// 产业商响应大农户的需求（向大农户需求响应表添加记录）
        /// </summary>
        /// <param name="id">需求id</param>
        /// <param name="userId">产业商id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string ReplyRequirement(DtoReplyRequirement model)//(long id, long userId)
        {

            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        /// <summary>
        /// 产业商获取我的应答列表
        /// </summary>
        /// <param name="pageindex">页码数</param>
        /// <param name="pagesize">第页要显示的数据条数</param>
        /// <param name="isclosed">需求发布状态（0进行中，1已关闭）</param>
        /// <param name="userid">产业商id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string MyReply(DtoMyReply model)//(int pageindex, int pagesize, int isclosed, long userid)
        {

            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        /// <summary>
        /// 应答详情
        /// </summary>
        /// <param name="requirementid">需求id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string AcceptRequirement(DtoAcceptRequirement model)//(int requirementid)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        /// <summary>
        /// 产业商发布给大农户的需求
        /// </summary>
        /// <param name="userId">大农户id(当前登陆者id)</param>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的数据条数</param>
        /// <param name="type">需求类型编号</param>
        /// <param name="region">区县编号</param>
        /// <param name="orderfield">排序标识</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string PublishedForFarmer(DtoPublishedForFarmer model)//(long userId, int pageIndex, int pageSize, int type, string region, string orderfield)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        /// <summary>
        /// 关闭需求
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <returns>System.Web.Mvc.JsonResult.</returns>
        public string CloseRequirement(DtoRemoveRequirement model)//(long id)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #region 产业商发布给大农户的需求(未登录)
        /// <summary>
        /// 产业商发布给大农户的需求
        /// </summary>        
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的数据条数</param>
        /// <param name="type">需求类型编号</param>        
        /// <param name="orderfield">排序标识</param>
        /// <author>ww</author>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string PublishedForFarmerByTime(int pageIndex, int pageSize, int type, string orderfield)
        {
            SetJsonHeader();

            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion
    }
}