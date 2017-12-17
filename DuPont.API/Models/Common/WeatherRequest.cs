using DuPont.Models.Enum;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Common
{
    public class WeatherRequest
    {
        public WeatherRequest()
        {
            key = ConfigHelper.GetAppSetting(DataKey.WeatherUrlKey);
            key1 = ConfigHelper.GetAppSetting(DataKey.WeatherUrlKey1);
            language = "zh-Hans";
            unit = "c";
        }
        /// <summary>
        /// 5天天气key
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 实时天气key
        /// </summary>
        public string key1 { get; set; }

        public string unit { get; set; }
        public string language { get;set;}
    }
}