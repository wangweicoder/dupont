using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Common
{
    public class DtoGetSearchWeather
    {
        public DtoGetSearchWeather()
        {
            results = new List<DtoGetSearchWeatherdaily>();
        }
        public List<DtoGetSearchWeatherdaily> results { get; set; }

    }
    public class DtoGetSearchWeatherdaily
    {
        public DtoGetSearchWeatherdaily()
        {
            daily = new List<DtoGetSearchWeatherDetailed>();
        }
        public List<DtoGetSearchWeatherDetailed> daily { get; set; }

    }
    public class DtoGetSearchWeatherDetailed
    {
        /// <summary>
        /// 日期
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string text_day { get; set; }
        public int code_day { get; set; }
        public string text_night { get; set; }
        public int code_night { get; set; }
        public int high { get; set; }
        public int low { get; set; }
        public string precip { get; set; }
    }
}