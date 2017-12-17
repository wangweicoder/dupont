using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility
{
    public class GISHelper
    {
        private const double EARTH_RADIUS = 6378.137;//地球半径
        /// <summary>
        /// 获取弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns>返回弧度</returns>
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// 计算两个地理坐标之间的距离
        /// </summary>
        /// <param name="lat1">起始点经度</param>
        /// <param name="lng1">起始点维度</param>
        /// <param name="lat2">终点经度</param>
        /// <param name="lng2">终点维度</param>
        /// <returns>返回距离单位（KM）</returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);

            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10;
            return s;
        }
    
    }
}
