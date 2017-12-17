

using DuPont.Interface;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Presentation.Models.Dto.FarmerRequirement;
using DuPont.Presentation.Properties;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Presentation.Controllers
{
    public class FarmerRequirementController : BaseController
    {

        public FarmerRequirementController()
        {

        }

        /// <summary>
        /// 发布需求
        /// </summary>
        /// <param name="id">0表示执行添加操作,非0执行修改操作</param>
        /// <param name="userId">发布者id</param>
        /// <param name="type">需求类型</param>
        /// <param name="cropId">农作物类别</param>
        /// <param name="acreage">亩数</param>
        /// <param name="description">摘要</param>
        /// <param name="date">The date.</param>
        /// <param name="address">基本地址</param>
        /// <param name="detailAddress">详细地址</param>
        /// <param name="PhoneNumber">发布需求者的手机号</param>
        /// <param name="ExpectedStartPrice">期望的最低价格</param>
        /// <param name="ExpectedEndPrice">期望的最高价格</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string SaveRequirement(DtoSaveRequirement model)//(long id, long userId, int type, int cropId, int acreage, string description, string date,
        //string address, string detailAddress, string PhoneNumber, double ExpectedStartPrice = 0, double ExpectedEndPrice = 0)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

            SetJsonHeader();
            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            //try
            //{
            //    #region 调用E田接口,传订单数据
            //    ResponseResult<T_FARMER_PUBLISHED_DEMAND> farmerrequirement = JsonHelper.FromJsonTo<ResponseResult<T_FARMER_PUBLISHED_DEMAND>>(result);
            //    if (farmerrequirement.IsSuccess == true && farmerrequirement.Entity != null)
            //    {
            //        //干活需求单
            //        if (farmerrequirement.Entity.DemandTypeId != (int)DuPont.Entity.Enum.FarmerDemandType.SellGrain && farmerrequirement.Entity.DemandTypeId != (int)DuPont.Entity.Enum.FarmerDemandType.SellSilage)
            //        {
            //            Task taskasync = new Task(() => ReturnOrderModel(farmerrequirement.Entity));
            //            taskasync.Start();
            //        }
            //    }
            //    #endregion
            //}
            //catch
            //{
            //    return result;
            //}
            return result;

        }
        /// <summary>
        ///  重新整理订单数据发送给E田
        /// </summary>
        /// <param name="farmerdemand">大农户需求单</param>
        /// <returns></returns>
        private string ReturnOrderModel(T_FARMER_PUBLISHED_DEMAND farmerdemand)
        {
            try
            {
                var content = new Dictionary<string, string>();
                content.Add("AcresId", farmerdemand.AcresId.ToString());
                if (!string.IsNullOrWhiteSpace(farmerdemand.Township))
                {
                    content.Add("Address", farmerdemand.Township);
                }
                else if (!string.IsNullOrWhiteSpace(farmerdemand.Region))
                {
                    content.Add("Address", farmerdemand.Region);
                }
                else if (!string.IsNullOrWhiteSpace(farmerdemand.City))
                {
                    content.Add("Address", farmerdemand.City);
                }
                else if (!string.IsNullOrWhiteSpace(farmerdemand.Province))
                {
                    content.Add("Address", farmerdemand.Province);
                }
                content.Add("UserId", farmerdemand.CreateUserId.ToString());
                //证书的路径
                var certification = GetCertificationFilePath();
                //证书的密码
                var certificationPwd = GetCertificationPwd();
                string baseurl = ConfigHelper.GetAppSetting(DataKey.RemoteApiForRelease);
                //这里发布了需要替换，因为多一层api目录
                var result = PostStandardWithSameControllerAction<DtoFarmerRequirementModel>("FarmerRequirement", "ReturnOrderModel", content);
                DtoFarmerRequirementModel resultmodel = new DtoFarmerRequirementModel();
                if (result.IsSuccess)
                {
                    resultmodel = result.Entity;
                }
                resultmodel.OrderId = farmerdemand.Id;
                resultmodel.UserId = farmerdemand.CreateUserId;
                resultmodel.CropId = farmerdemand.CropId;
                resultmodel.DemandTypeId = farmerdemand.DemandTypeId;
                resultmodel.Brief = farmerdemand.Brief;
                resultmodel.PhoneNum = farmerdemand.PhoneNumber;
                resultmodel.CreateTime = DateTime.Now;
                //干活日期
                string tempdate = farmerdemand.ExpectedDate;
                if (tempdate.Contains(',')||tempdate.Contains('-'))
                {
                    string[] tempdates = tempdate.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
                    if (tempdates.Length > 1)
                    {
                        resultmodel.StartDate = tempdates[0].ToString();
                        resultmodel.EndDate = tempdates[tempdates.Length - 1].ToString();
                    }
                    else
                    {
                        resultmodel.StartDate = tempdates[0].ToString();
                        resultmodel.EndDate = tempdates[0].ToString();
                    }

                }
                else if (tempdate.Contains('、'))
                {
                    if (tempdate.EndsWith("、"))
                    {
                        tempdate = tempdate.TrimEnd('、');
                    }
                    string[] tempdates = tempdate.Split(new char[]{'、'},StringSplitOptions.RemoveEmptyEntries);
                    if (tempdates.Length > 1)
                    {
                        resultmodel.StartDate = Convert.ToDateTime(tempdates[0].ToString()).ToString("yyyy-MM-dd");
                        resultmodel.EndDate = Convert.ToDateTime(tempdates[tempdates.Length - 1].ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        resultmodel.StartDate = Convert.ToDateTime(tempdates[0].ToString()).ToString("yyyy-MM-dd");
                        resultmodel.EndDate = Convert.ToDateTime(tempdates[0].ToString()).ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    resultmodel.StartDate = Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd");
                    resultmodel.EndDate = Convert.ToDateTime(tempdate).ToString("yyyy-MM-dd");
                }
                #region 调用E田接口
                var etcontent = new Dictionary<string, string>();
                etcontent = ModelHelper.GetPropertyDictionary<DtoFarmerRequirementModel>(resultmodel);
                //加密要传递的参数              
                Dictionary<string, string> rurlcontent = EncryptDictionary(etcontent);
                //e田接口地址
                string Apiurl = ConfigHelper.GetAppSetting("EtApiUrl");
                string Eturl = Apiurl + "/wei/work/dupont/farmer_order.jsp";
                //证书的路径
                var etcertification = GetCertificationFilePath();
                //证书的密码
                var etcertificationPwd = GetCertificationPwd();
                var etresult = HttpAsynchronousTool.CustomHttpWebRequestPost(Eturl, rurlcontent, etcertification, etcertificationPwd);
                var resultModel = JsonConvert.DeserializeObject<ETResponseResult<object>>(etresult);
                if (resultModel.IsSuccess == true)
                {  //写文件确认我方调用e田接口完成
                    string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "大农户的订单";
                    string parmeters = null;
                    foreach (var item in rurlcontent)
                    {
                        parmeters += item.Key + ":" + item.Value + "\r\n";
                    }

                    IOHelper.WriteLogToFile("FarmerRequirement/SaveRequirement" + logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
                }
                return resultModel.IsSuccess.ToString();
            }
            catch (Exception ex)
            {
                IOHelper.WriteLogToFile("FarmerRequirement/SaveRequirement" + "\r\n错误：" + ex.Message, RelativePath() + @"\DuPontRequestEtLog");
            }
            return "";
                #endregion
        }

        /// <summary>            
        /// 将对象属性转换为key-value对  
        /// </summary>  
        /// <param name="o"></param>  
        /// <returns></returns>  
        public static Dictionary<String, Object> ToMap(Object o)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();
            Type t = o.GetType();
            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                }
            }
            return map;
        }

        [HttpPost]
        public string SaveOperatorRequirement(DtoSaveOperatorRequirement model)
        {
            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

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
        /// 获取需求详情(公共接口适用于大农户和产业商)
        /// </summary>
        /// <param name="id">需求id</param>
        /// <param name="roletype">角色编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string RequirementDetail(DtoRequirementDetail model)//(long id, int roletype)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

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
        [HttpPost]
        public string RemoveRequirement(DtoRemoveRequirement model)//(long id)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

            SetJsonHeader();
            var content = GetPostParameters();

            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();


            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            #region 调用E田接口,更新订单状态
            //ResponseResult<T_FARMER_PUBLISHED_DEMAND> farmerrequirement = JsonHelper.FromJsonTo<ResponseResult<T_FARMER_PUBLISHED_DEMAND>>(result);
            //if (farmerrequirement.IsSuccess == true && farmerrequirement.Entity != null)
            //{
            //    //取消订单
            //    if (farmerrequirement.Entity.DemandTypeId != (int)DuPont.Entity.Enum.FarmerDemandType.SellGrain && farmerrequirement.Entity.DemandTypeId != (int)DuPont.Entity.Enum.FarmerDemandType.SellSilage)
            //    {
            //        Task taskasync = new Task(() => UpdateCancleOrder(farmerrequirement.Entity));
            //        taskasync.Start();
            //    }
            //}
            #endregion
            return result;


        }
        /// <summary>
        /// 先锋帮大农户取消需求订单
        /// </summary>
        /// <param name="t_FARMER_PUBLISHED_DEMAND"></param>
        /// <returns></returns>
        private string UpdateCancleOrder(T_FARMER_PUBLISHED_DEMAND t_FARMER_PUBLISHED_DEMAND)
        {
            DtoUpdateFarmerDemandModel updatemodel = new DtoUpdateFarmerDemandModel();
            updatemodel.Id = t_FARMER_PUBLISHED_DEMAND.Id;
            updatemodel.OrderState = t_FARMER_PUBLISHED_DEMAND.PublishStateId;
            updatemodel.FarmerName = t_FARMER_PUBLISHED_DEMAND.CreateUserId.ToString();
            #region 调用内网服务返回数据
            //var content = new Dictionary<string, string>();
            //content.Add("Id", updatemodel.Id.ToString());
            ////农机手id
            //var resultmodel = PostStandardWithSameControllerAction<DtoUpdateFarmerDemandModel>("Operator", "AcceptOrder", content);
            //if (resultmodel.IsSuccess == true)
            //{
            //    if (resultmodel.Entity != null)
            //    {
            //        updatemodel.OperatoName = resultmodel.Entity.OperatoName;
            //        updatemodel.FarmerName = resultmodel.Entity.FarmerName;
            //    }
            //}           
            #endregion
            #region 调用E田接口
            var etcontent = new Dictionary<string, string>();
            etcontent = ModelHelper.GetPropertyDictionary<DtoUpdateFarmerDemandModel>(updatemodel);
            //加密要传递的参数
            Dictionary<string, string> rurlcontent = EncryptDictionary(etcontent);
            //e田接口地址
            string Apiurl = ConfigHelper.GetAppSetting("EtApiUrl");
            string Eturl = Apiurl + "/wei/work/dupont/order_status.jsp";
            //证书的路径
            var etcertification = GetCertificationFilePath();
            //证书的密码
            var etcertificationPwd = GetCertificationPwd();
            var etresult = HttpAsynchronousTool.CustomHttpWebRequestPost(Eturl, rurlcontent, etcertification, etcertificationPwd);
            var resultModel = JsonConvert.DeserializeObject<ETResponseResult<object>>(etresult);
            if (resultModel.IsSuccess == true)
            {
                //写文件确认我方调用e田接口完成
                string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "先锋帮大农户取消需求订单";
                string parmeters = null;
                foreach (var item in rurlcontent)
                {
                    parmeters += item.Key + ":" + item.Value + "\r\n";
                }
                IOHelper.WriteLogToFile(logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
            }
            return resultModel.IsSuccess.ToString();
            #endregion
        }
        /// <summary>
        /// 获取对产业商发布的需求
        /// </summary>
        /// <param name="userId">产业商id</param>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的条数</param>
        /// <param name="type">需求类型编号</param>
        /// <param name="region">区县（可选）</param>
        /// <param name="orderfield">排序标识（可选）</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string PublishedForBusiness(DtoPublishedForBusiness model)//(long userId, int pageIndex, int pageSize, int type, string region, string orderfield)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }


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
        /// 获取对农机手发布的需求
        /// </summary>
        /// <param name="userId">农机手id</param>
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的条数</param>
        /// <param name="type">需求类型编号</param>
        /// <param name="region">区县编号（可选）</param>
        /// <param name="orderfield">排序标识（可选）</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string PublishedForOperator(DtoPublishedForOperator model)//(long userId, int pageIndex, int pageSize, int type, string region, string orderfield)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }


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
        /// 大农户进行订单评价
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="executeUserId">大农户id</param>
        /// <param name="userid">产业商id/农机手id</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string CommentRequirement(DtoCommentRequirement model)//(long id, long executeUserId, long userid, string commentString, int score)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

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
        /// 大农户接受产业商任务（向产业商需求响应表中添加记录）
        /// </summary>
        /// <param name="id">需求Id</param>
        /// <param name="userId">接收者id</param>
        /// <param name="weightrangetypeid">起购重量或者亩数编号</param>
        /// <param name="address">地址</param>
        /// <param name="phonenumber">手机号</param>
        /// <param name="brief">备注信息</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string AcceptTask(DtoAcceptTask model)//(long id, long userId, int weightrangetypeid, string address, string phonenumber, string brief)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

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
        /// 大农户获取我的应答列表
        /// </summary>
        /// <param name="pageindex">页码数</param>
        /// <param name="pagesize">每页要显示的条数</param>
        /// <param name="isclosed">需求状态</param>
        /// <param name="userid">大农户id</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string MyReply(DtoMyReply model)//(int pageindex, int pagesize, int isclosed, long userid)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

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

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

            SetJsonHeader();
            var content = GetPostParameters();

            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();


            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;

        }

        #region 大农户发布给产业商和农机手的需求列表(未登录)
        /// <summary>
        /// 获取大农户发布给产业商和农机手的需求列表
        /// </summary>        
        /// <param name="pageIndex">页码数</param>
        /// <param name="pageSize">每页要显示的条数</param>
        /// <param name="type">需求类型编号</param>        
        /// <param name="orderfield">排序标识（可选）</param> 
        /// <author>ww</author>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string PublishedForOperatorAndBusiness(int pageIndex, int pageSize, int type, string orderfield)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

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

        #region "需求评价(加需求状态100507，100508后的接口)"
        /// <summary>
        /// 大农户进行订单评价
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="executeUserId">大农户id</param>
        /// <param name="userid">产业商id/农机手id</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string CommentRequirementForOperator(DtoCommentRequirement model)//(long id, long executeUserId, long userid, string commentString, int score)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

            SetJsonHeader();
            var content = GetPostParameters();

            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);

            #region 调用E田接口,评价靠谱作业的农机手
            //ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> farmerrequirement = JsonHelper.FromJsonTo<ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>>(result);
            //if (farmerrequirement.IsSuccess == true && farmerrequirement.Entity != null)
            //{
            //    //靠谱作业农机手接的单
            //    if (farmerrequirement.Entity.SourceType == (int)DuPont.Entity.Enum.SourceType.JeRei)
            //    {
            //        //大农户评价靠谱作业农机手                
            //        Task taskasync = new Task(() => CommentOrderForOperator(farmerrequirement.Entity));
            //        taskasync.Start();
            //    }

            //}
            #endregion
            return result;

        }
        /// <summary>
        /// 调用E田接口，评价靠谱作业农机手  
        /// </summary>
        /// <param name="t_FARMER_DEMAND_RESPONSE_RELATION"></param>
        /// <returns></returns>
        private object CommentOrderForOperator(T_FARMER_DEMAND_RESPONSE_RELATION t_FARMER_DEMAND_RESPONSE_RELATION)
        {
            try
            {
                DtoCommentFarmerDemandModel updatemodel = new DtoCommentFarmerDemandModel();
                updatemodel.Id = t_FARMER_DEMAND_RESPONSE_RELATION.DemandId;
                updatemodel.CommentString = t_FARMER_DEMAND_RESPONSE_RELATION.Comments;
                updatemodel.Score = t_FARMER_DEMAND_RESPONSE_RELATION.Score;
                #region 调用内网服务返回数据
                var content = new Dictionary<string, string>();
                content.Add("Id", updatemodel.Id.ToString());
                content.Add("UserId", t_FARMER_DEMAND_RESPONSE_RELATION.UserId.ToString());//农机手id
                var resultmodel = PostStandardWithSameControllerAction<DtoCommentFarmerDemandModel>("FarmerRequirement", "CommentOrderForOperator", content);
                if (resultmodel.IsSuccess == true)
                {
                    if (resultmodel.Entity != null)
                    {
                        updatemodel.OperatorUserId = resultmodel.Entity.OperatorUserId;
                        updatemodel.FarmerUserId = resultmodel.Entity.FarmerUserId;
                    }
                }
                #endregion
                #region 调用E田接口
                var etcontent = new Dictionary<string, string>();
                etcontent = ModelHelper.GetPropertyDictionary<DtoCommentFarmerDemandModel>(updatemodel);
                //加密要传递的参数
                Dictionary<string, string> rurlcontent = EncryptDictionary(etcontent);

                //e田接口地址
                string Apiurl = ConfigHelper.GetAppSetting("EtApiUrl");
                string Eturl = Apiurl + "/wei/work/dupont/evaluate_driver.jsp";
                //证书的路径
                var etcertification = GetCertificationFilePath();
                //证书的密码
                var etcertificationPwd = GetCertificationPwd();
                var etresult = HttpAsynchronousTool.CustomHttpWebRequestPost(Eturl, rurlcontent, etcertification, etcertificationPwd);
                var resultModel = JsonHelper.FromJsonTo<ETResponseResult<object>>(etresult);
                if (resultModel.IsSuccess == true)
                {
                    //写文件确认我方调用e田接口完成
                    string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "先锋帮大农户评价靠谱作业的农机手";
                    string parmeters = null;
                    foreach (var item in rurlcontent)
                    {
                        parmeters += item.Key + ":" + item.Value + "\r\n";
                    }
                    IOHelper.WriteLogToFile("FarmerRequirement/CommentRequirementForOperator" + logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
                }

                return resultModel.IsSuccess.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                IOHelper.WriteLogToFile("FarmerRequirement/CommentOrderForOperator:" + ex.Message, RelativePath() + @"\DuPontRequestEtLog");
                return "";
            }
        }
        #endregion
    }
}