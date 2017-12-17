using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Global.Common
{
    public static class MyCommons
    {
        public static Func<T_SYS_DICTIONARY, string> Get_SysDictionary_DisplayName = (dic) =>
        {
            if (dic != null && dic.DisplayName != null)
            {
                return dic.DisplayName;
            }
            return "";
        };

        public static Func<T_AREA, string> Get_Area_DisplayName = (dic) =>
        {
            if (dic != null && dic.DisplayName != null)
            {
                return dic.DisplayName;
            }
            return "";
        };
    }
}
