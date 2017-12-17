

using DuPont.Interface;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Presentation.Models.Dto.Operator;
using DuPont.Presentation.Properties;
using DuPont.Utility;
using DuPont.Utility.LogModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Presentation.Controllers
{
    public class OperatorController : BaseController
    {
        
        /// <summary>
        /// 农机手应大农户的需求（向大农户需求响应表添加记录）
        /// </summary>
        /// <param name="id">需求id</param>
        /// <param name="userId">农机手id</param>
        /// <returns>JsonResult.</returns>
        public string ReplyRequirement(DtoReplyRequirement model)//(long id, long userId)
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
            //#region 调用E田接口,更新订单状态
            //ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> farmerrequirement = JsonHelper.FromJsonTo<ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>>(result);
            //if (farmerrequirement.IsSuccess == true && farmerrequirement.Entity != null)
            //{
            //    //接受订单                
            //    Task taskasync = new Task(() => AcceptOrder(farmerrequirement.Entity));
            //    taskasync.Start();
                
            //}
            //#endregion
            
            return result;

        }
        /// <summary>
        /// 接受大农户的需求单
        /// </summary>
        /// <param name="t_FARMER_DEMAND_RESPONSE_RELATION"></param>
        /// <returns></returns>
        private string AcceptOrder(T_FARMER_DEMAND_RESPONSE_RELATION t_FARMER_DEMAND_RESPONSE_RELATION)
        {
            DtoUpdateFarmerDemandModel updatemodel = new DtoUpdateFarmerDemandModel();
            updatemodel.Id = t_FARMER_DEMAND_RESPONSE_RELATION.DemandId;
            updatemodel.OrderState =100502;            
            #region 调用内网服务返回数据
            var content = new Dictionary<string, string>();
            content.Add("Id",  updatemodel.Id.ToString());
            content.Add("UserId", t_FARMER_DEMAND_RESPONSE_RELATION.UserId.ToString());//农机手id
             //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            var resultmodel =PostStandardWithSameControllerAction<DtoUpdateFarmerDemandModel>("Operator", "AcceptOrder", content);//发布后带api的
                 
            if (resultmodel.IsSuccess == true)
            {
                if (resultmodel.Entity != null)
                {
                    updatemodel.OperatoName = resultmodel.Entity.OperatoName;
                    updatemodel.FarmerName = resultmodel.Entity.FarmerName;
                }
            }           
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
            var resultModel = JsonHelper.FromJsonTo<ETResponseResult<object>>(etresult);
            if (resultModel.IsSuccess == true)
            {
                //写文件确认我方调用e田接口完成
                string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "先锋帮的农机手接受订单";
                string parmeters = null;
                foreach (var item in rurlcontent)
                {
                    parmeters += item.Key + ":" + item.Value + "\r\n";
                }
                IOHelper.WriteLogToFile("Operator/ReplyRequirement" + logErrstring + parmeters, RelativePath() + @"\DuPontRequestEtLog");   
            }
            return resultModel.IsSuccess.ToString();
            #endregion
        }
        /// <summary>
        /// 农机手获取我的应答列表
        /// </summary>
        /// <param name="pageindex">页码数</param>
        /// <param name="pagesize">每页要显示的条数</param>
        /// <param name="isclosed">The isclosed.</param>
        /// <param name="userid">用户编号</param>
        /// <returns>JsonResult.</returns>
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
        #region 农机手评价大农户
        /// 农机手评价大农户订单
        /// </summary>
        /// <param name="id">需求编号</param>
        /// <param name="FarmerUserId">大农户id</param>
        /// <param name="OperatorUserid">农机手id</param>
        /// <param name="commentString">评价内容</param>
        /// <param name="score">分数</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string CommentRequirement(DtoCommentRequirement model)//(long id, string OperatorUserid, long FarmerUserId, string commentString, int score,SourceType)
        {

            //验证参数
            var validateRes = ValidateParamModel();

            if (validateRes != null)
            {
                return JsonHelper.ToJsJson(validateRes);
            }

            SetJsonHeader();
            var content = GetPostParameters();
            var result = string.Empty;
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();           
             //先锋帮用户
            //E田调用时，model.SourceType.ToString()=="0" 的值永远为0,可能是请求头格式的问题
            try
            {
                string sourcetype = content["SourceType"].ToString();
                if (sourcetype == "0")
                {
                    result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
                    #region 调用E田接口,更新订单状态
                    //ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION> farmerrequirement = JsonHelper.FromJsonTo<ResponseResult<T_FARMER_DEMAND_RESPONSE_RELATION>>(result);
                    //if (farmerrequirement.IsSuccess == true && farmerrequirement.Entity != null)
                    //{
                    //    //评价大农户订单                
                    //    Task taskasync = new Task(() => CommentOrder(farmerrequirement.Entity));
                    //    taskasync.Start();

                    //}
                    #endregion
                }
                else //E田
                {
                    string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "靠谱作业的农机手评价先锋帮大农户";
                    string parmeters = null;
                    foreach (var item in content)
                    {
                        parmeters += item.Key + ":" + item.Value + "\r\n";
                    }
                    IOHelper.WriteLogToFile(logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
                    //靠谱作业的农机手评价先锋帮大农户  
                    string Apiurl = ConfigHelper.GetAppSetting(DataKey.RemoteApiForRelease);
                    string url = Apiurl + "api" + "/Operator/EtCommentRequirement";
                    result = HttpAsynchronousTool.CustomHttpWebRequestPost(url, content, certification, certificationPwd);

                }
            }
            catch (Exception ex) {
                return ex.Message;
            }

            return result;

        }
        /// <summary>
        /// 农机手评价大农户
        /// </summary>
        /// <param name="t_FARMER_DEMAND_RESPONSE_RELATION"></param>
        /// <returns></returns>
        private string CommentOrder(T_FARMER_DEMAND_RESPONSE_RELATION t_FARMER_DEMAND_RESPONSE_RELATION)
        {
            try
            {
                DtoUpdateFarmerDemandModel updatemodel = new DtoUpdateFarmerDemandModel();
                updatemodel.Id = t_FARMER_DEMAND_RESPONSE_RELATION.DemandId;
                updatemodel.OrderState = 100508;
                updatemodel.CommentString = t_FARMER_DEMAND_RESPONSE_RELATION.CommentsFarmer;
                updatemodel.Score = t_FARMER_DEMAND_RESPONSE_RELATION.ScoreFarmer;
                #region 调用内网服务返回数据
                var content = new Dictionary<string, string>();
                content.Add("Id", updatemodel.Id.ToString());
                content.Add("UserId", t_FARMER_DEMAND_RESPONSE_RELATION.UserId.ToString());//农机手id
                var resultmodel = PostStandardWithSameControllerAction<DtoUpdateFarmerDemandModel>("Operator", "AcceptOrder", content);
                if (resultmodel.IsSuccess == true)
                {
                    if (resultmodel.Entity != null)
                    {
                        updatemodel.OperatoName = resultmodel.Entity.OperatoName;
                        updatemodel.FarmerName = resultmodel.Entity.FarmerName;
                    }
                }               
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
                var resultModel = JsonHelper.FromJsonTo<ETResponseResult<object>>(etresult);
                if(resultmodel.IsSuccess==true)
                {
                    //写文件确认我方调用e田接口已经完成
                    string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "先锋帮的农机手评价大农户";
                    string parmeters = null;
                    foreach (var item in rurlcontent)
                    {
                        parmeters += item.Key + ":" + item.Value + "\r\n";
                    }
                    IOHelper.WriteLogToFile("Operator/CommentRequirement" + logErrstring + parmeters, RelativePath() + @"\DuPontRequestEtLog"); 
                }
                return resultModel.IsSuccess.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                IOHelper.WriteLogToFile("Operator/CommentOrder:" + ex.Message, RelativePath() + @"\DuPontRequestEtLog");
                return "";
            }
        }
        #endregion

        #region 靠谱作业农机手接受订单
        public string ReplyFarmerRequirement(DtoReplyFarmerRequirement model)//(long id, long userId)
        {
            try
            {
                //验证参数
                var validateRes = ValidateParamModel();
                SetJsonHeader();
                var content = GetPostParameters();
                string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "靠谱作业农机手接受订单";
                string parmeters = null;
                foreach (var item in content)
                {
                    parmeters += item.Key + ":" + item.Value + "\r\n";
                }

                IOHelper.WriteLogToFile("Operator/ReplyFarmerRequirement" + logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
                //证书的路径
                var certification = GetCertificationFilePath();
                //证书的密码
                var certificationPwd = GetCertificationPwd();

                var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);

                return result;
            }
            catch(Exception ex)
            {
                IOHelper.WriteLogToFile("Operator/ReplyFarmerRequirement" + ex.Message, RelativePath() + @"\DuPontRequestEtLog");
                return "";
            }

        }
        
        #endregion

        #region 靠谱作业农机手取消订单，更新状态，删除响应记录
        public string CancelFarmerRequirement(DtoCancelFarmerRequirement model)//(long id, long userId)
        {
            try
            {                
                var validateRes = ValidateParamModel();
                SetJsonHeader();
                var content = GetPostParameters();
                string logErrstring = DateTime.Now.ToString("\r\n---------MM/dd/yyyy HH:mm:ss,fff---------\r\n") + "靠谱作业农机手取消订单";
                string parmeters = null;
                foreach (var item in content)
                {
                    parmeters += item.Key + ":" + item.Value + "\r\n";
                }
                IOHelper.WriteLogToFile("Operator/CancelFarmerRequirement" + logErrstring + "\r\n" + parmeters, RelativePath() + @"\DuPontRequestEtLog");
                //证书的路径
                var certification = GetCertificationFilePath();
                //证书的密码
                var certificationPwd = GetCertificationPwd();

                var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);

                return result;
            }
            catch (Exception ex)
            {
                IOHelper.WriteLogToFile("Operator/CancelFarmerRequirement" + ex.Message, RelativePath() + @"\DuPontRequestEtLog");
                return "";
            }

        }

        #endregion
    }
}