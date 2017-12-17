using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DuPont.Utility
{
    public class HttpRequestHelper
    {
        public static readonly string CA_PATH = ConfigHelper.GetAppSetting("CertificateUrl");//HTTPS证书路径
        public static readonly string CA_PWD = ConfigHelper.GetAppSetting("CertificatePwd");//HTTPS证书密码

        public static TEntity Post<TEntity>(string uri, IDictionary<string, object> valueTextParameters, IDictionary<string, string> fileParameters) where TEntity : new()
        {
            var httpMethod = Method.POST;
            var client = new RestClient(uri);
            var request = new RestRequest();
            var acceptParameter = client.DefaultParameters.First(p => p.Name == "Accept");
            acceptParameter.Value += ",text/html";

            //添加值、文本类参数
            if (valueTextParameters != null && valueTextParameters.Count > 0)
            {
                foreach (var parameter in valueTextParameters)
                {
                    request.AddParameter(parameter.Key, parameter.Value);
                }
            }

            //添加文件类参数
            if (fileParameters != null && fileParameters.Count > 0)
            {
                foreach (var parameter in fileParameters)
                {
                    request.AddFile(parameter.Key, parameter.Value);
                }
            }

            //证书处理
            if (client.BaseUrl.Scheme.ToLower() == "https" && !string.IsNullOrEmpty(CA_PATH))
            {
                client.ClientCertificates = new X509CertificateCollection();
                client.ClientCertificates.Add(new X509Certificate(CA_PATH, CA_PWD));
                ServicePointManager.ServerCertificateValidationCallback = (sender1, certificate, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                        return true;

                    return false;
                };
            }

            return client.ExecuteAsPost<TEntity>(request, httpMethod.ToString()).Data;
        }

        /// <summary>
        /// Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        public static string sendGet(string url, IDictionary<string, string> parameters)
        {
            //创建请求   
            var client = new RestClient(url + "?" + BuildQuery(parameters, "utf-8"));
            var requestRestclient = new RestRequest(Method.GET);
            requestRestclient.AddHeader("cache-control", "no-cache");
            IRestResponse iresponse = client.Execute(requestRestclient);
            //返回内容
            string retString = iresponse.Content;
            return retString;

        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            if (parameters != null)
            {
                StringBuilder postData = new StringBuilder();
                bool hasParam = false;
                IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
                while (dem.MoveNext())
                {
                    string name = dem.Current.Key;
                    string value = dem.Current.Value;
                    // 忽略参数名或参数值为空的参数
                    if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                    {
                        if (hasParam)
                        {
                            postData.Append("&");
                        }
                        postData.Append(name);
                        postData.Append("=");
                        if (encode == "gb2312")
                        {
                            postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                        }
                        else if (encode == "utf8")
                        {
                            postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                        }
                        else
                        {
                            postData.Append(value);
                        }
                        hasParam = true;
                    }
                }
                return postData.ToString();
            }
            else
            {
                return null;
            }
        }

      
    }
}
