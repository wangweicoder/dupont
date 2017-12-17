// ***********************************************************************
// Assembly         : DuPont.Utility
// Author           : 毛文君
// Created          : 12-04-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-04-2015
// ***********************************************************************
// <copyright file="RestSharpHelper.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using DuPont.Extensions;
using Newtonsoft.Json;


namespace DuPont.Utility
{
    public class RestSharpHelper
    {

        public static TEntity PostWithApplicationJson<TEntity>(string url, IDictionary<string, string> parameters, string certificatePath, string certificatePassword) where TEntity : class,new()
        {
            var client = new RestSharp.RestClient(url);
            var request = new RestSharp.RestRequest(Method.POST);

            request.RequestFormat = DataFormat.Json;
            var jsonBody = new StringBuilder("{");

            if (parameters != null)
            {
                foreach (var para in parameters)
                {
                    jsonBody.AppendFormat("\"{0}\":\"{1}\",", para.Key, HttpUtility.HtmlEncode(para.Value));
                }
            }

            jsonBody.Remove(jsonBody.Length - 1, 1).Append("}");
            request.AddParameter(request.JsonSerializer.ContentType, jsonBody, ParameterType.RequestBody);

#if !DEBUG
            if (client.BaseUrl.Scheme.ToLower() == "https")
            {
                if (string.IsNullOrEmpty(certificatePath))
                    throw new ArgumentNullException("certificatePath");

                client.ClientCertificates = new X509CertificateCollection();
                client.ClientCertificates.Add(new X509Certificate(certificatePath, certificatePassword));
                ServicePointManager.ServerCertificateValidationCallback = (sender1, certificate, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                        return true;

                    return false;
                };
            }
#endif

            var response = client.Execute<TEntity>(request);

            //检测异常
            if (response.ErrorException != null)
            {
                throw new Exception(response.ErrorException.Message + "\r\nUrl:" + url + "\r\n\r\nErrorContent:" + response.Content);//如果有异常就把它抛出来
            }

            return response.Data;
        }

        public static TEntity PostWithStandard<TEntity>(string url, IDictionary<string, string> parameters, string certificatePath, string certificatePassword) where TEntity : class,new()
        {
            var client = new RestSharp.RestClient(url);
            var request = new RestSharp.RestRequest(Method.POST);

            if (parameters != null)
            {
                foreach (var para in parameters)
                {
                    request.AddParameter(para.Key, para.Value);
                }
            }

#if !DEBUG
            if (client.BaseUrl.Scheme.ToLower() == "https")
            {
                if (string.IsNullOrEmpty(certificatePath))
                    throw new ArgumentNullException("certificatePath");

                client.ClientCertificates = new X509CertificateCollection
                {
                    new X509Certificate(certificatePath, certificatePassword)
                };
                ServicePointManager.ServerCertificateValidationCallback = (sender1, certificate, chain, sslPolicyErrors) =>
                {
                    if (sslPolicyErrors == SslPolicyErrors.None)
                        return true;

                    return false;
                };
            }
#endif
            var response = client.Execute<TEntity>(request);

            //检测异常
            if (response.ErrorException != null)
            {
                //throw new Exception(response.ErrorException.Message + "\r\nUrl:" + url + "\r\n\r\nErrorContent:" + response.Content);//如果有异常就把它抛出来//如果有异常就把它抛出来
                //return JsonHelper.FromJsonTo<TEntity>(response.Content);  //这种方式日期反序列化出错了
                
                //重要记录，错误叫，“没有找到默认构造函数”，其实就是json数据里多了一个图片地址，json格式没有问题，所以就反序列化到对象继续返回。
               var s= JsonConvert.DeserializeObject<TEntity>(response.Content);               
                return s;
            }
            else{
                return response.Data;
            }

            
        }
    }
}
