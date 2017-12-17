using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Common
{
    public class GetWeatherRealTimeResponse
    {
        /// <summary>
        /// 首页天气信息
        /// </summary>
        public string Name { get; set; }
        public string Now_Text { get; set; }
        public string Now_Code { get; set; }
        public string Now_Temperature { get; set; }
       
    }
}