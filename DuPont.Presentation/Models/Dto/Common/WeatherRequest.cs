using DuPont.Models.Enum;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class WeatherRequest
    {
        public WeatherRequest()
        {
            key = ConfigHelper.GetAppSetting(DataKey.WeatherUrlKey);
            language = "zh-Hans";
            unit = "c";
        }
        /// <summary>
        /// 
        /// </summary>
        public string key { get; set; }

        public string unit { get; set; }
        public string language { get;set;}
    }
}