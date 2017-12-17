using DuPont.Models.Enum;
using DuPont.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DuPont.Utility
{
    public static class HttpAsynchronousTool
    {
        public static async Task<JObject> CustomHttpRequestPost(string uri, Dictionary<string, string> content)
        {
            var client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = client.PostAsync(uri, new FormUrlEncodedContent(content)).Result;
            Encoding encoding = Encoding.GetEncoding(response.Content.Headers.ContentType.CharSet ?? "utf-8");
            var contentStream = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(contentStream, encoding))
            {
                var result = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject(result) as JObject;
            }
        }

        /// <summary>
        /// HttpWebRequest版发送http请求方法
        /// </summary>
        /// <param name="uri">请求url</param>
        /// <param name="content">参数</param>
        /// <param name="certificateUrl">证书路径</param>
        /// <returns>System.String.</returns>
        public static string CustomHttpWebRequestPost(string uri, Dictionary<string, string> content, string certificateUrl = null, string certificatePwd = null, Dictionary<string, string> cookie = null)
        {
            string result = "";

            if (certificateUrl.IsNull())
            {
                certificateUrl = ConfigHelper.GetAppSetting(DataKey.CertificateUrl);
            }
            if (certificatePwd.IsNull())
            {
                certificatePwd = ConfigHelper.GetAppSetting(DataKey.CertificatePwd);
            }

            StringBuilder sbPostData = new StringBuilder();

            if (content.Count > 0)
            {
                foreach (var key in content.Keys)
                {
                    sbPostData.Append(key + "=" + HttpUtility.UrlEncode(content[key]) + "&");
                }
            }

            UTF8Encoding encoding = new UTF8Encoding();
            string postData = sbPostData.ToString().Trim('&');
            byte[] data = encoding.GetBytes(postData);

            Uri reqUrl = new Uri(uri);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(reqUrl);

#if (!DEBUG)
            X509Certificate cer = null;
            if (certificatePwd.IsNull())
            {
                cer = new X509Certificate(certificateUrl);
            }
            else
            {
                cer = new X509Certificate(certificateUrl, certificatePwd);
            }
            request.ClientCertificates.Add(cer);
#endif


            request.ContentType = "application/x-www-form-urlencoded";

            request.Method = "POST";
            request.ContentLength = data.Length;
            if (cookie != null)
            {
                //添加cookie
                CookieContainer cookieC = new CookieContainer();
                foreach (var key in cookie.Keys)
                {
                    Cookie tempC = new Cookie(key, cookie[key]);
                    cookieC.Add(tempC);
                }

                request.CookieContainer = cookieC;
            }

            using (Stream myStream = request.GetRequestStream())
            {
                myStream.Write(data, 0, data.Length);
            }

            WebResponse respResult = request.GetResponse();


            using (Stream streamResponse = respResult.GetResponseStream())
            {
                using (StreamReader streamRead = new StreamReader(streamResponse, Encoding.UTF8))
                {
                    result = streamRead.ReadToEnd();
                }
            }

            return result;
        }

        /// <summary>
        /// 发送带文件的HttpRequest post请求(包括https请求)(遗留问题,file类型的ContentType属性传不过去)
        /// </summary>
        /// <param name="url">请求的路径</param>
        /// <param name="fileParams">文件类型的参数</param>
        /// <param name="feildParams">字段类型的参数</param>
        /// <returns></returns>
        public static string CustomHttpPostWithFile(string url, Dictionary<string, HttpPostedFileBase> fileParams, Dictionary<string, string> feildParams, string certificateUrl = null, string certificatePwd = null)
        {

            string result = "";

            #region 准备参数

            string boundary = "----" + DateTime.Now.Ticks.ToString("X");
            Stream memStream = new System.IO.MemoryStream();
            byte[] boundarybytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");

            if (feildParams != null)
            {

                //将字段类型的参数(feildParams)写入请求参数流中
                string feildHeadTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

                foreach (var key in feildParams.Keys)
                {
                    string feild = string.Format(feildHeadTemplate, key, feildParams[key]);
                    byte[] feildbytes = System.Text.Encoding.UTF8.GetBytes(feild);
                    memStream.Write(feildbytes, 0, feildbytes.Length);
                }

            }

            //分隔符
            memStream.Write(boundarybytes, 0, boundarybytes.Length);

            //将文件类型类型的参数(fileParams)写入请求参数流中

            //头模板
            string fileHeaderTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: {2}\r\n\r\n";

            //遍历文件类型参数
            foreach (var key in fileParams.Keys)
            {
                var fileParam = fileParams[key];
                string header = string.Format(fileHeaderTemplate, key, fileParam.FileName, fileParam.ContentType);

                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

                memStream.Write(headerbytes, 0, headerbytes.Length);

                //取文件流内容(适用于小文件形式，以后扩充大文件时分批传送的处理逻辑)
                BinaryReader b = new BinaryReader(fileParam.InputStream);
                byte[] fileData = b.ReadBytes(Convert.ToInt32(fileParam.InputStream.Length));
                //将文件数据写入请求参数流
                memStream.Write(fileData, 0, fileData.Length);
                //分隔符
                memStream.Write(boundarybytes, 0, boundarybytes.Length);
            }

            #endregion

            #region 创建并发送Http请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;

            //Set the Method property to 'POST' to post data to the URI.
            request.Method = "POST";
            //request.KeepAlive = true;
            //request.Credentials =System.Net.CredentialCache.DefaultCredentials;

            request.ContentLength = memStream.Length;

            //给请求添加证书
            if (certificateUrl != null)
            {
                X509Certificate cer = null;
                if (certificatePwd != null)
                    cer = new X509Certificate(certificateUrl, certificatePwd);
                else
                    cer = new X509Certificate(certificateUrl);

                request.ClientCertificates.Add(cer);
            }

            //将准备的参数流memStream写入request的请求流中
            using (Stream requestStream = request.GetRequestStream())
            {
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            }

            try
            {
                Task<WebResponse> respResult = request.GetResponseAsync();

                respResult.Wait();
                using (Stream streamResponse = respResult.Result.GetResponseStream())
                {
                    using (StreamReader streamRead = new StreamReader(streamResponse, Encoding.UTF8))
                    {
                        result = streamRead.ReadToEnd();
                    }
                }
            }
            catch
            {
                //写日志
            }

            #endregion

            return result;
        }
        
    }
}
