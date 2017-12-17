// ***********************************************************************
// Assembly         : DuPont.Global
// Author           : 毛文君
// Created          : 12-03-2015
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 12-04-2015
// ***********************************************************************
// <copyright file="JsonResultEx.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using DuPont.Global.JsonProvider;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DuPont.Global.ActionResults
{
    public class JsonResultEx : JsonResult
    {
        public JsonResultEx()
        {
            InitialConfig();
        }
        public JsonResultEx(object data, bool allowGet = false)
        {
            this.Data = data;
            if (allowGet)
            {
                this.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            }
            InitialConfig();
        }

        private void InitialConfig()
        {
            this.ContentType = "application/json";
            this.ContentEncoding = Encoding.UTF8;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            try
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if ((this.JsonRequestBehavior == System.Web.Mvc.JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("此请求已被阻止，因为当用在 GET 请求中时，会将敏感信息透漏给第三方网站。");
                }
                HttpResponseBase response = context.HttpContext.Response;
                if (!string.IsNullOrEmpty(this.ContentType))
                {
                    response.ContentType = this.ContentType;
                }
                else
                {
                    response.ContentType = "application/json";
                }
                if (this.ContentEncoding != null)
                {
                    response.ContentEncoding = this.ContentEncoding;
                }
                if (this.Data != null)
                {
                    var settings = new JsonSerializerSettings() { ContractResolver = new NullToEmptyStringResolver() };
                    var jsonSerializer = JsonSerializer.Create(settings);
                    if (this.RecursionLimit.HasValue)
                    {
                        settings.MaxDepth = this.RecursionLimit.Value;
                    }

                    var sw = new StringWriter();
                    jsonSerializer.Serialize(new JsonTextWriter(sw), this.Data);
                    response.Write(sw.GetStringBuilder().ToString());
                }
            }
            catch (Exception ex)
            {
 
            }
        }
    }
}
