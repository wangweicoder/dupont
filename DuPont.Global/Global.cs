using DuPont.Models.Enum;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DuPont.Global
{
    public class Global
    {
        public static AdminUserLoginInfo GetAdminLoginInfo()
        {
            return HttpContext.Current.Session[DataKey.UserInfo] as AdminUserLoginInfo;
        }

        public static bool IsInDebug()
        {
            var isDebug = true;
#if !DEBUG
            isDebug = false;
#endif
            return isDebug;
        }


        //学习园地缩略图参数配置
        public static readonly int LeanGardenThumbnailWidth = 89;
        public static readonly int LearGardenThumbnailHeight = 68;
    }
}
