// ***********************************************************************
// Assembly         : DuPont.Utility
// Author           : 毛文君
// Created          : 09-02-2015
//
// Last Modified By : 毛文君
// Last Modified On : 09-02-2015
// ***********************************************************************
// <copyright file="ConfigHelper.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Configuration;
using System.Web;

namespace DuPont.Utility
{
    public class ConfigHelper
    {
        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetConnectionString(string key)
        {
            var conn= ConfigurationManager.ConnectionStrings[key];
            if (conn!=null)
            {
                return conn.ConnectionString;
            }
            return "";
        }

        /// <summary>
        /// 获取AppSetting配置节（带缓存）
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static string GetAppKeyWithCache(string key)
        {
            string result = null;
            if (result == null)
            {
                var cache = HttpContext.Current.Cache;
                result = Convert.ToString(cache[key]);
                if (string.IsNullOrEmpty(result))
                {
                    result = GetAppSetting(key);
                    cache[key] = result;
                }
            }

            return result;
        }
    }
}
