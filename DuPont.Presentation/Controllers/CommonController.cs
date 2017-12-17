

using DuPont.Global.ActionResults;
using DuPont.Interface;
using DuPont.Models.Dtos.Background.User;
using DuPont.Models.Dtos.Foreground.Common;
using DuPont.Models.Enum;
using DuPont.Models.Models;
using DuPont.Presentation.Models.Dto.Common;
using DuPont.Presentation.Models.Dto.FarmerRequirement;
using DuPont.Presentation.Properties;
using DuPont.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DuPont.Presentation.Controllers
{
    public class CommonController : BaseController
    {
        private static new string Url = ConfigHelper.GetAppSetting(DataKey.RemoteApiForRelease);
        private static string articleBasePath = ConfigHelper.GetAppSetting(DataKey.ArticleStaticPageBasePath);
        private ISms _smsRepository;

        public CommonController(ISms smsRepository)
        {
            _smsRepository = smsRepository;
        }

        #region "获取字典数据"
        /// <summary>
        /// 根据父节点获取子节点字典数据
        /// 可以传入多个Code（用逗号分隔）,可以同时获取多个Code的直接子节点数据
        /// </summary>
        /// <param name="Code">字典编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string GetDictionaryItems(DtoGetDictionaryItems model)//(string Code)
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

        #region "获取地区数据"
        /// <summary>
        /// 获取地区列表
        /// </summary>
        /// <param name="ParentAId">字典编号</param>
        /// <returns>JsonResult.</returns>
        public string GetAreaChild(DtoGetAreaChild model)//(string ParentAId)
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

        #region "上传图片"
        /// <summary>
        /// 上传图片附件
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Pic">图片文件</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult UploadPicture(DtoUploadPicture model)//(Int64 UserId, HttpPostedFileBase Pic)
        {
            using (ResponseResult<Object> result = new ResponseResult<Object>())
            {
                string folder = Server.MapPath("~/") + @"uploadfiles";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string currentFolder = folder + @"\" + GetFolderNameByDate();
                string path = "uploadfiles/" + GetFolderNameByDate();
                if (!Directory.Exists(currentFolder))
                {
                    Directory.CreateDirectory(currentFolder);
                }

                string extensionName = model.Pic.FileName.Split('.').LastOrDefault();

                if (!CheckFileIsImage(extensionName))
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.UploadFileIsImageMessage;

                    return Json(result);
                }

                string guid = Guid.NewGuid().ToString();
                string fileName = guid + "." + extensionName;
                string saveImageBasePath = currentFolder + @"\";
                model.Pic.SaveAs(saveImageBasePath + fileName);


                //插入数据库
                var content = GetPostParameters();
                content.Remove("Pic");
                content.Add("Path", path + "/" + fileName);

                //证书的路径
                var certificationUrl = GetCertificationFilePath();
                //证书的密码
                var certificationPwd = GetCertificationPwd();

                var res = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certificationUrl, certificationPwd);

                var resObj = JsonHelper.FromJsonTo<ResponseResult<Object>>(res);

                if (resObj.IsSuccess)
                {
                    //生成69x69的小图
                    string smallImage = guid + "_69x69." + extensionName;
                    ImageHelper.MakeThumbnail(saveImageBasePath + fileName, saveImageBasePath + smallImage, 69, 69, "Cut");

                    result.IsSuccess = true;
                    result.Entity = resObj.Entity;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = ResponeString.InternalErrorMessage;
                }

                return Json(result);
            }

        }
        #endregion

        #region "私有方法"
        /// <summary>
        /// 根据当前日期获取上传文件夹名称（格式：年份月份）
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetFolderNameByDate()
        {
            string res = string.Format("{0}{1:D2}", DateTime.Now.Year, DateTime.Now.Month);

            return res;
        }

        /// <summary>
        /// 检查文件类型是否是图片
        /// </summary>
        /// <param name="extensionName"></param>
        /// <returns></returns>
        private bool CheckFileIsImage(string extensionName)
        {
            string[] extensions = { "jpg", "jpeg", "bmp", "png" };
            return extensions.Any(e => e == extensionName.ToLower());
        }
        #endregion

        #region "获取评价列表"
        /// <summary>
        /// 评价列表
        /// </summary>
        /// <param name="userid">被评价者的id</param>
        /// <param name="roletype">角色类别</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns>根据roletype的值：3产业商对大农户做出的评价，4返回大农户对农机手做出的评价，5大农户对产业商做出的评价</returns>
        [HttpPost]
        public string CommentDetail(DtoCommentDetail model)//(long userid, int roletype, int pageindex, int pagesize)
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

        #region "获取手机验证码"
        /// <summary>
        /// 获取手机验证码
        /// </summary>
        /// <param name="phoneNumber">手机号码</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string GetValidateCode(DtoGetValidateCode model)//(string phoneNumber)
        {
            SetJsonHeader();

            if (ConfigHelper.GetAppSetting("smsCodeSenderInTestMode") == "1")
            {
                using (var result = new ResponseResult<object>())
                {
                    result.Entity = "9999";
                    return JsonHelper.ToJsJson(result);
                }
            }

            var parameters = ModelHelper.GetPropertyDictionary<DtoGetValidateCode>(model);
            //检查手机验证码的发送状态
            var checkSendStateResultObject = PostStandardWithSameControllerAction<object>("Common", "CheckValidateCodeState", parameters);

            var responseResultObject = new ResponseResult<object>();
            //验证该手机上一次验证码发送状态结果
            switch (checkSendStateResultObject.State.Id)
            {
                case 1:
                    //手机格式不正确
                    responseResultObject.IsSuccess = false;
                    responseResultObject.Message = checkSendStateResultObject.Message;
                    return JsonHelper.ToJsJson(responseResultObject);
                //case 2:
                //    //验证码发送记录不存在

                //    break;
                case 3:
                    //验证码未过期
                    responseResultObject.IsSuccess = true;
                    responseResultObject.Entity = checkSendStateResultObject.Entity;
                    return JsonHelper.ToJsJson(responseResultObject);
                //case 4:
                //    //验证码已过期

                //    break;
                //case 5:
                //    //验证码发送记录保存失败
                //    break;
                //case 100:
                //    //验证码发送记录保存成功

                //    break;
            }

            //需要发送验证的几个状态（验证码发送记录不存在、验证码已过期）
            var needSendValidateCode = checkSendStateResultObject.State.Id == 2
                                     || checkSendStateResultObject.State.Id == 4;
            if (needSendValidateCode)
            {
                //生成随机验证码
                var randomValidateCode = Rand.Number(4, true);
                var sendValidateCodeSuccess = _smsRepository.Send(model.phoneNumber, randomValidateCode);
                if (sendValidateCodeSuccess)
                {
                    //验证码发送成功,保存验证码发送记录到数据库
                    Dictionary<string, string> saveValidateCodeSendRecordParams = new Dictionary<string, string>()
                        {
                            {"phoneNumber",model.phoneNumber},
                            {"validateCode",randomValidateCode}
                        };

                    //保存状态结果，保存的成功和失败这里不做记录
                    var saveStateResult = PostStandardWithSameControllerAction<object>("Common", "SaveValidateCodeSendRecord", saveValidateCodeSendRecordParams);
                    responseResultObject.IsSuccess = true;
                    responseResultObject.Entity = randomValidateCode;
                    return JsonHelper.ToJsJson(responseResultObject);
                }
                else
                {
                    responseResultObject.IsSuccess = false;
                    responseResultObject.Message = "短信获取失败!";
                    return JsonHelper.ToJsJson(responseResultObject);
                }
            }

            return JsonHelper.ToJsJson(responseResultObject);
        }
        #endregion

        #region "获取轮播图片"
        /// <summary>
        /// 获取轮播图片
        /// </summary>
        /// <param name="RoleId">角色编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public JsonResult CarouselPictures(DtoCarouselPictures model)//(int RoleId)
        {
            var result = PostJsonWithSameControllerAction<List<CarouselFile>>(this, new Dictionary<string, string> { { "RoleId", model.RoleId.ToString() } });
            if (result.IsSuccess)
            {
                foreach (var file in result.Entity)
                {
                    file.FilePath = articleBasePath + "/" + file.FilePath;
                }
            }
            return new JsonResultEx(result);
        }
        #endregion

        #region "获取指定地区拥有指定角色的人数"
        /// <summary>
        /// 获取指定地区拥有指定角色的人数
        /// </summary>
        /// <param name="id">地区编号</param>
        /// <param name="roleType">角色编号</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string GetPersonNumber(DtoGetPersonNumber model)//(long id, int roleType)
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

        #region "检查App的版本"
        /// <summary>
        /// 检查App的版本
        /// </summary>
        /// <param name="platform">当前平台(android/ios)</param>
        /// <returns>JsonResult.</returns>
        [HttpPost]
        public string AppVersion(DtoAppVersion model)//(string platform)
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

        #region "获取附近农机手"
        /// <summary>
        /// 获取大农户附近农机手列表
        /// </summary>
        [HttpPost]
        public JsonResult GetOperatorsForFarmerRequire(NearbyOperatorListInput input)
        {
            var parameters = GetPostParameters();
            var result = PostStandardWithSameControllerAction<List<OperatorProfile>>(this, parameters);
            return new JsonResultEx(result);
        }
        #endregion

        #region "检查第三方登录的图标显示状态（仅针对ios终端使用）"
        /// <summary>
        /// 检查第三方登录的图标显示状态（仅针对ios终端使用）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Check_CAS_DisplayState()
        {
            var result = PostStandardWithSameControllerAction<object>(this);
            return new JsonResultEx(result);
        }
        #endregion

        #region "获取天气预报和玉米价格"
        /// <summary>
        /// 获取天气预报数据
        /// </summary>
        /// <author>ww</author>
        [HttpGet]
        public string GetWeatherbyCityName1(string cityname)
        {
            var parameters = GetPostParameters();
            parameters.Add("city", cityname);
            SetJsonHeader();
            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();

            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        [HttpPost]
        public string GetCornPricesold(string everydayurl)
        {
            string result = "{";
            if (!string.IsNullOrWhiteSpace(everydayurl))
            {
                string priceUrl = "www.yumi.com.cn" + everydayurl;

                // Console.WriteLine("原粮价格_玉米价格地址："+ priceUrl);"http://" + "www.yumi.com.cn/html/2017/01/20170119093205203868.html"
                result += "\"IsSuccess\":true ,";
                result += "\"Message\":\"查询成功！\",";
                result += "\"Entity\":{";
                string td1 = "";
                #region table1 第一个表格
                string tempstr = GetTableByHtml("http://" + priceUrl, 1, 21);
                string[] tr = tempstr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo\":[";
                for (int i = 0; i < tr.Length; i++)
                {
                    string[] td = tr[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0 || i == 8 || i == 17 || i == 19)
                    {
                        td1 = tr[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (i == 0 || i == 8 || i == 17 || i == 19)
                    {
                        result += "{\"city\":\"" + td1.ToString() + td[1].ToString() + "\",";
                        result += "\"PurchasingPrice\":\"" + td[2].ToString() + "\",";
                        result += "\"PContrastWithYesterday\":\"" + td[3].ToString() + "\",";
                        result += "\"ExPostPrice\":\"" + td[4].ToString() + "\",";
                        result += "\"EContrastWithYesterday\":\"" + td[5].ToString() + "\"";
                        result += "},";
                    }
                    else
                    {
                        result += "{ \"city\":\"" + td1.ToString() + td[0].ToString() + "\",";
                        result += " \"PurchasingPrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PContrastWithYesterday\":\"" + td[2].ToString() + "\",";
                        result += " \"ExPostPrice\":\"" + td[3].ToString() + "\",";
                        result += " \"EContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                        result += "},";
                    }

                }
                result = result.TrimEnd(new char[] { ',' }) + "],";
                #endregion
                #region table2第二个表格
                string tempstr1 = GetTableByHtml("http://" + priceUrl, 22, 45);
                string[] tr1 = tempstr1.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo1\":[";
                for (int i = 0; i < tr1.Length; i++)
                {
                    string[] td = tr1[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0 || i == 5 || i == 7 || i == 11 || i == 18 || i == 22)
                    {
                        td1 = tr1[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (i == 0 || i == 5 || i == 7 || i == 11 || i == 18 || i == 22)
                    {
                        result += "{ \"City\":\"" + td1.ToString() + td[1].ToString() + "\",";
                        result += " \"PurchasingPrice\":\"" + td[2].ToString() + "\",";
                        result += " \"PContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                    }
                    else
                    {
                        result += "{ \"City\":\"" + td1.ToString() + td[0].ToString() + "\",";
                        result += " \"PurchasingPrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PContrastWithYesterday\":\"" + td[2].ToString() + "\",";
                        result = result.TrimEnd(new char[] { ',' }) + "},";
                    }
                }
                result = result.TrimEnd(new char[] { ',' }) + "],";
                #endregion
                #region table3第三个表格
                string tempstr2 = GetTableByHtml("http://" + priceUrl, 46, 50);
                string[] tr2 = tempstr2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo2\":[";
                for (int i = 0; i < tr2.Length; i++)
                {
                    string[] td = tr2[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0)
                    {
                        td1 = tr2[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (i == 0)
                    {
                        result += "{ \"City\":\"" + td1.ToString() + td[1].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[2].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[3].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                        result += "},";
                    }
                    else if (i == 1)
                    {
                        result += "{ \"City\":\"" + td1.ToString() + td[0].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                    }
                    else if (i == 2 || i == 3)
                    {
                        result += "{ \"City\":\"" + td[0].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                    }
                }
                result = result.TrimEnd(new char[] { ',' }) + "],";
                #endregion
                #region table4第四个表格
                string tempstr3 = GetTableByHtml("http://" + priceUrl, 51, 59);
                string[] tr3 = tempstr3.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo3\":[";
                for (int i = 0; i < tr3.Length; i++)
                {
                    string[] td = tr3[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i == 0 || i == 3 || i == 5 || i == 6)
                    {
                        td1 = tr3[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (i == 0 || i == 3 || i == 5 || i == 6)
                    {
                        result += "{ \"city\":\"" + td1.ToString() + td[1].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[2].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[3].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                        result += "},";
                    }
                    else if (i == 7)
                    {
                        result += "{ \"city\":\"" + td[0].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                    }
                    else
                    {
                        result += "{ \"city\":\"" + td1.ToString() + td[0].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                    }
                }
                result = result.TrimEnd(new char[] { ',' }) + "],";
                #endregion
                #region table5第五个表格
                string tempstr4 = GetTableByHtml("http://" + priceUrl, 60, 69);
                string[] tr4 = tempstr4.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                result += "\"TableInfo4\":[";
                for (int i = 0; i < tr4.Length; i++)
                {
                    string[] td = tr4[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (i != 1 && i != 3)
                    {
                        td1 = tr4[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (i == 1 || i == 3)
                    {
                        result += "{ \"City\":\"" + td1.ToString() + td[0].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[1].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[2].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[3].ToString() + "\"";
                        result += "},";
                    }
                    else
                    {
                        result += "{ \"city\":\"" + td[0].ToString() + td[1].ToString() + "\",";
                        result += " \"AveragePrice\":\"" + td[2].ToString() + "\",";
                        result += " \"PriceType\":\"" + td[3].ToString() + "\",";
                        result += " \"AContrastWithYesterday\":\"" + td[4].ToString() + "\"";
                        result += "},";
                    }


                }
                #endregion
                result = result.TrimEnd(',').Replace("<span", " ") + "]}}";
                return result;
            }
            else
            {
                result += "\"IsSuccess\":true,";
                result += "\"Message\":\"输入参数为空！\",";
                result += "\"Entity\":{}";
                result += "}";
                return result;
            }

        }
        /// <summary>
        /// 数据保存在table中
        /// </summary>
        /// <returns></returns>
        private static string GetTableByHtml(string htmlstr, int start, int end)
        {
            string htmlTablestr = htmlstr.Substring(htmlstr.IndexOf("<table"), htmlstr.LastIndexOf("table") - htmlstr.IndexOf("<table"));
            //行数据 
            string pattern = @"(<tr>[\s\S]*?</tr>)";//"(<p.class=\".*</p>)";                       
            Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            //单元格数据
            string cellpattern = @"(<.*</span>)";
            Regex cellreg = new Regex(cellpattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            string title = "";
            for (int i = start; i < end; i++)
            {
                string row = reg.Matches(htmlTablestr)[i].Value;
                int rowc = cellreg.Matches(row).Count;
                for (int j = 0; j < rowc; j++)
                {
                    string rowvalue = new Regex(">(.*)</span>").Match(cellreg.Matches(row)[j].Value).Value;
                    var r = new Regex("<(\\S*?)[^>]*>");//已<开始的字符，出现0次或者多次，
                    rowvalue = r.Replace(rowvalue, "");
                    title += rowvalue.Replace(">", "") + ",";
                }
                title += ";";
            }
            return title;
        }
        #endregion

        #region 获取玉米价格表
        /// <summary>
        /// 获取玉米价格表
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetModDataList(string dateTime)
        {
            var content = GetPostParameters();
            SetJsonHeader();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            //发送请求
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        #endregion

        #region 获取玉米价格日期列表
        /// <summary>
        /// 获取玉米价格日期列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetModListold()
        {
            string result = "{";
            try
            {
                string dominAddress = "http://www.yumi.com.cn/yumijiage/index.html";
                string loadstr = HttpRequestHelper.sendGet(dominAddress, null);
                string pattern = @"(<ul class=""priceSubChe"">[\s\S]*?</ul>)";
                Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                string cpattern = @"(<p>.*</p>)";
                Regex creg = new Regex(cpattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                result += "\"IsSuccess\":true ,";
                result += "\"Message\":\"查询成功！\",";
                result += "\"State\":{\"Id\": 200,\"Description\": \"请求成功\"},";
                result += "\"Entity\":{";
                result += "\"InfoList\":[";
                foreach (Match item in reg.Matches(loadstr))
                {
                    foreach (Match citem in creg.Matches(item.Value))
                    {
                        string hrefurl = new Regex(@"\d{4}年\d{1,2}月\d{1,2}日").Match(citem.Value).Value;
                        string title = new Regex("\">.*</a>").Match(citem.Value).Value;
                        title = title.Replace("\">", "").Substring(0, title.Replace("\">", "").IndexOf("</"));
                        result += "{\"title\":\"" + title + "\",";
                        hrefurl = Convert.ToDateTime(hrefurl).ToString("yyyyMMdd");
                        result += " \"href\":\"" + hrefurl + "\"},";
                    }
                }
                return result.TrimEnd(',') + "]}}";
            }
            catch (Exception ex)
            {
                result += "\"IsSuccess\":true,";
                result += "\"Message\":\"应用程序异常！\",";
                result += "\"Entity\":{}";
                result += "}";
                return result;
            }
        }
        /// <summary>
        /// 获取玉米价格日期列表
        /// </summary>
        /// <author>ww</author>
        /// <returns>目录中的文件名集合</returns>
        [HttpPost]
        public string GetModList(int pageIndex, int pageSize = 10)
        {
            var content = GetPostParameters();
            SetJsonHeader();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            //发送请求
            var result= HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }

        #endregion

        #region 天气预报
        /// <summary>
        /// 当前城市天气(首页)
        /// </summary>
        /// <param name="location">110000000000|110100000000|||</param>
        /// <author>ww</author>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWeatherbyCityCode(string location)
        {
            using (ResponseResult<DtoGetWeather> result = new ResponseResult<DtoGetWeather>())
            {
                if (!string.IsNullOrWhiteSpace(location))
                {
                    try
                    {
                        #region 获得城市名称
                        //查询名称的api地址
                        string selecturl = ConfigHelper.GetAppSetting(DataKey.BaiduGeocoderUrl);
                        Dictionary<string, string> citydic = new Dictionary<string, string>();
                        //百度的ak
                        string AK = ConfigHelper.GetAppSetting(DataKey.baiduAK);
                        citydic.Add("ak", AK);
                        citydic.Add("location", location);
                        citydic.Add("output", "json");
                        //请求获得名称的地址
                        string resultjson = HttpRequestHelper.sendGet(selecturl, citydic);
                        int cityindex = resultjson.IndexOf("city");
                        int districtindex = resultjson.IndexOf("district");
                        string tempname = resultjson.Substring(cityindex, districtindex - cityindex);
                        //city":"成都市","
                        int shiindex = tempname.IndexOf("市");
                        string CityName = tempname.Substring(7, shiindex - 7);
                        #endregion
                        #region 获得天气信息
                        //天气接口地址未确定
                        string url = ConfigHelper.GetAppSetting(DataKey.WeatherUrl);
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        dic.Add("city", CityName);
                        string apiresult = HttpRequestHelper.sendGet(url, dic);
                        #endregion

                        //反序列化json字符串
                        WeatherAPIEntity apiEntiey = JsonHelper.FromJsonTo<WeatherAPIEntity>(apiresult);
                        if (apiEntiey != null)
                        {
                            if (apiEntiey.status == "1000")
                            {
                                result.IsSuccess = apiEntiey.status == "1000" ? true : false;
                                result.Message = apiEntiey.desc;
                                result.Entity.temperature = apiEntiey.data != null ? apiEntiey.data.wendu : "";
                                result.Entity.cityname = apiEntiey.data != null ? apiEntiey.data.city : "";
                                result.Entity.weathertype = apiEntiey.data.forecast[0].type;
                                for (int i = 0; i < 5; i++)
                                {
                                    result.Entity.future.Add(new BaseWeather());//先初始化天气类和初始化两天的天气集合
                                    result.Entity.future[i].wind_strength = apiEntiey.data.forecast[i].fengli;
                                    result.Entity.future[i].wind_direction = apiEntiey.data.forecast[i].fengxiang;
                                    result.Entity.future[i].high_temperature = apiEntiey.data.forecast[i].high;
                                    result.Entity.future[i].low_temperatur = apiEntiey.data.forecast[i].low;
                                    result.Entity.future[i].weathertype = apiEntiey.data.forecast[i].type;
                                    result.Entity.future[i].dateweek = apiEntiey.data.forecast[i].date;
                                }
                            }
                            else
                            {
                                result.IsSuccess = true;
                                result.Message = "没有获得此位置的天气数据";
                            }
                        }
                        else
                        {
                            result.Message = "应用程序繁忙";
                        }
                        return Json(result);
                    }
                    catch (Exception ex)
                    {
                        result.Message = ex.Message;
                        return Json(result);
                    }
                }
                else
                {
                    return Json(result);
                }

            }

        }
        /// <summary>
        /// 城市天气
        /// </summary>
        /// <param name="CityName"></param>
        /// <author>ww</author>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetWeatherbyCityName(string CityName)
        {
            using (ResponseResult<DtoGetWeather> result = new ResponseResult<DtoGetWeather>())
            {
                if (!string.IsNullOrWhiteSpace(CityName))
                {
                    string url = ConfigHelper.GetAppSetting(DataKey.WeatherUrl);
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("city", CityName);
                    string apiresult = HttpRequestHelper.sendGet(url, dic);
                    //反序列化json字符串
                    WeatherAPIEntity apiEntiey = JsonHelper.FromJsonTo<WeatherAPIEntity>(apiresult);
                    if (apiEntiey != null)
                    {
                        if (apiEntiey.status == "1000")
                        {
                            result.IsSuccess = apiEntiey.status == "1000" ? true : false;
                            result.Message = apiEntiey.desc;
                            result.Entity.temperature = apiEntiey.data != null ? apiEntiey.data.wendu : "";
                            result.Entity.cityname = apiEntiey.data != null ? apiEntiey.data.city : "";
                            result.Entity.weathertype = apiEntiey.data.forecast[0].type;
                            for (int i = 0; i < 5; i++)
                            {
                                result.Entity.future.Add(new BaseWeather());//先初始化天气类和初始化两天的天气集合
                                result.Entity.future[i].wind_strength = apiEntiey.data.forecast[i].fengli;
                                result.Entity.future[i].wind_direction = apiEntiey.data.forecast[i].fengxiang;
                                result.Entity.future[i].high_temperature = apiEntiey.data.forecast[i].high;
                                result.Entity.future[i].low_temperatur = apiEntiey.data.forecast[i].low;
                                result.Entity.future[i].weathertype = apiEntiey.data.forecast[i].type;
                                result.Entity.future[i].dateweek = apiEntiey.data.forecast[i].date;
                            }
                        }
                        else
                        {
                            result.IsSuccess = true;
                            result.Message = "没有获得此位置的天气数据";
                        }
                    }
                    else
                    {
                        result.Message = "应用程序繁忙";
                    }
                    return Json(result);
                }
                else
                {
                    return Json(result);
                }
            }
        }

        #endregion

        #region 新的天气预报

        #region 查询当前天气预报
        /// <summary>
        /// 首页天气预报接口
        /// </summary>
        /// <param name="userId">用户的id</param>
        /// <returns></returns>
        [HttpPost]
        public string GetWeatherRealTime(string userId)
        {
            var content = GetPostParameters();
            SetJsonHeader();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            //发送请求
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;
        }
        /// <summary>
        /// 查询天气接口
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        private static dynamic SelectWeatherinfo(string cityName)
        {
            string url = ConfigHelper.GetAppSetting(DataKey.WeatherUrlNew);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var request = new WeatherRequest();
            dic.Add("key", request.key);//key
            dic.Add("location", cityName);//地址
            dic.Add("language", request.language);
            dic.Add("unit", request.unit);
            string apiresult = HttpRequestHelper.sendGet(url + "/weather/now.json", dic);
            var js = new JavaScriptSerializer();
            var resultJson = js.Deserialize<dynamic>(apiresult);
            return resultJson;
        }
        #endregion
        #region 修改城市接口
        /// <summary>
        ///  修改城市接口
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="name">城市名称</param>
        /// <returns></returns>
        public JsonResult ModifyUserWeatherCity(string userId, string name)
        {
            var result = new ResponseResult<object>();
            if (string.IsNullOrEmpty(userId))
            {
                result.IsSuccess = false;
                result.Message = "用户id为空";
                return Json(result);
            }
            if (string.IsNullOrEmpty(name))
            {
                result.IsSuccess = false;
                result.Message = "城市名称为空";
                return Json(result);
            }
            var content = GetPostParameters();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            //发送请求
            var resPoststr = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            ResponseResult<object> resPost = JsonHelper.FromJsonTo<ResponseResult<object>>(resPoststr);

            resPost.Entity = null;

            return Json(resPost);

        }
        #endregion
        #region  获取天气预报接口
        /// <summary>
        ///   获取天气预报接口
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="name">城市名称</param>
        /// <returns></returns>
        public string GetSearchWeather(string userId, string name)
        {
            var content = GetPostParameters();
            SetJsonHeader();
            //证书的路径
            var certification = GetCertificationFilePath();
            //证书的密码
            var certificationPwd = GetCertificationPwd();
            //发送请求
            var result = HttpAsynchronousTool.CustomHttpWebRequestPost(GetCurrentUrl(this), content, certification, certificationPwd);
            return result;           
        }
        #endregion
        #endregion

        #region "获取评价列表(整合后的)"
        /// <summary>
        /// 评价列表
        /// </summary>
        /// <param name="userid">被评价者的id</param> 
        /// <author>ww</author>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns>根据roletype的值：3产业商对大农户做出的评价，4返回大农户对农机手做出的评价，5大农户对产业商做出的评价</returns>
        [HttpPost]
        public string PublicCommentDetail(DtoPublicDommentDetail model)//(long userid, int roletype, int pageindex, int pagesize)
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

        #region 我的应答（统一的接口）
        /// <summary>
        /// 我的应答列表
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
        #endregion
    }
}