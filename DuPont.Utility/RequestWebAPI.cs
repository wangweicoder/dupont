using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility
{
    public class RequestWebAPI
    {
        public static string GetAPIResponse(string url, IDictionary<string,string> parameters,HttpMethod method)
        {
            
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            if (parameters != null)
            {

            }
            string res = string.Empty;

            Encoding encoding = Encoding.GetEncoding("gb2312");

            HttpWebResponse response = method == HttpMethod.GET ? HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null) :
                HttpWebResponseUtility.CreatePostHttpResponse(url, parameters, null, null, encoding, null);

            Stream stream = response.GetResponseStream();
            if (stream != null)
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    res = reader.ReadToEnd();
                }
            }
            return res;
        }
    }
}
