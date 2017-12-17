using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Models.Models
{
    public class DtoGetWeather
    {
        public DtoGetWeather()
        {
            future = new List<BaseWeather>();
        }
        /// <summary>
        /// 当前温度
        /// </summary>
        public string temperature { set; get; }        
        public List<BaseWeather> future { set; get; }
        public string cityname { set; get; }
        public string weathertype { set; get; }
        public string todayweathertypepic { set; get; }
    }
    public class BaseWeather 
    {
        public BaseWeather() {
            wind_strength = "";
            wind_direction = string.Empty;
            dateweek = string.Empty;
            high_temperature = "";
            low_temperatur = "";
            weathertype = string.Empty;
            weathertypepic = string.Empty;
        }
        /// <summary>
        /// 风向
        /// </summary>
        public string wind_direction { set; get; }
        /// <summary>
        /// 风力
        /// </summary>
        public string wind_strength { set; get; }
        /// <summary>
        /// 高温
        /// </summary>
        public string high_temperature { set; get; }
        /// <summary>
        /// 低温
        /// </summary>
        public string low_temperatur { set; get; }
        /// <summary>
        /// 天气类型
        /// </summary>
        public string weathertype { set; get; }
        /// <summary>
        /// 日期
        /// </summary>
        public string dateweek { set; get; }
        public string weathertypepic { set; get; }
    }
}