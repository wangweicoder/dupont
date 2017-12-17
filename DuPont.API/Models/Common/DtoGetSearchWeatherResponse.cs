using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Common
{
    public class DtoGetSearchWeatherResponse
    {
        public DtoGetSearchWeatherResponse()
        {
            Daily = new List<WeatherResponse>();
        }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 背景图片
        /// </summary>
        public int BackgroundType { get; set; }
        /// <summary>
        /// 当前天气现象文字
        /// </summary>
        public string Now_Text { get; set; }

        /// <summary>
        /// 当天天气现象代码
        /// </summary>
        public int Now_Code { get; set; }
        /// <summary>
        /// 当前温度
        /// </summary>
        public int Now_Temperature { get; set; }
     
        /// <summary>
        /// 剩下几天天气情况
        /// </summary>
        public List<WeatherResponse> Daily { get; set; }
    }

    public class WeatherResponse
    {
      
        /// <summary>
        /// 日期
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// 白天天气现象文字
        /// </summary>
        public string Day_Text { get; set; }
        /// <summary>
        /// 白天天气现象代码
        /// </summary>
        public int Day_Code { get; set; }
        /// <summary>
        /// 晚间天气现象代码
        /// </summary>
        public string Night_Text { get; set; }
        /// <summary>
        /// 晚间天气现象代码
        /// </summary>
        public int Night_Code { get; set; }
        /// <summary>
        /// 当天最高气温
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 当天最低气温
        /// </summary>
        public int Low { get; set; }
    }
}