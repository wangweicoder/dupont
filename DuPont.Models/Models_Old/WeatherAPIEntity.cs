using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Models.Models
{
    public class WeatherAPIEntity
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string desc { set; get; }
        public string status { set; get; }      
        public apiBaseWeather data { set; get; }
       
    }
    public class apiBaseWeather 
    {
        public string wendu { set; get; }
        public string city { set; get; }
        public string ganmao { set; get; }

        public List<array> forecast { set; get; }
        public object yesterday { set; get; }
        public string api { set; get; }
       
    }
    public class array
    {
        /// <summary>
        /// 风向
        /// </summary>
        public string fengxiang { set; get; }
        /// <summary>
        /// 风力
        /// </summary>
        public string fengli { set; get; }
        /// <summary>
        /// 高温
        /// </summary>
        public string high { set; get; }
        /// <summary>
        /// 低温
        /// </summary>
        public string low { set; get; }
        /// <summary>
        /// 天气类型
        /// </summary>
        public string type { set; get; }
        /// <summary>
        /// 日期
        /// </summary>
        public string date{ set; get; }
    }
}