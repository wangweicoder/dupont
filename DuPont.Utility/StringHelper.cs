using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility
{
    public class StringHelper
    {
        /// <summary>
        /// Split the Address string to Dictionnary Collection
        /// </summary>
        /// <param name="address">Address String</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetAddress(string address)
        {

            Dictionary<string, string> addressCollection = new Dictionary<string, string>();
            string[] addresses = address.Split('|');
            addressCollection.Add("Province", addresses[0]);
            addressCollection.Add("City", addresses[1]);
            addressCollection.Add("Region", addresses[2]);
            addressCollection.Add("Township", addresses[3]);
            addressCollection.Add("Village", addresses[4]);

            return addressCollection;

        }
        /// <summary>
        /// 格式化需求中的期望日期，因为ios，android保存数据时传的不同
        /// </summary>
        /// <param name="ExpectedDate"></param>
        /// <returns></returns>
        public static string TransfeDate(string ExpectedDate)
        {
            //干活日期 
            if (!string.IsNullOrWhiteSpace(ExpectedDate))
            {
                string tempdate = ExpectedDate;
                string resultdate = "";
                if (tempdate.Contains(',') || tempdate.Contains('-'))
                {
                    string[] tempdates = tempdate.Split(',');
                    foreach (var item in tempdates)
                    {
                        string idate = Convert.ToDateTime(item).ToString("yyyy年MM月dd日");
                        resultdate += idate + "、";
                    }
                    resultdate = resultdate.TrimEnd('、');
                }
                else if (tempdate.Contains("、"))
                {
                    if (tempdate.EndsWith("、"))
                    {
                        tempdate = tempdate.TrimEnd('、');
                    }
                    resultdate = tempdate;
                }
                else
                {
                    resultdate = tempdate;
                }
                return resultdate;
            }
            else
            {
                return ExpectedDate;
            }
        } 
    
    }
}
