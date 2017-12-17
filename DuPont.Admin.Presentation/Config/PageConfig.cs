
using DuPont.Models.Enum;
using DuPont.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Admin.Presentation.Config
{
    public class PageConfig
    {
        public static string GetRemoteApiUrl()
        {
            return ConfigHelper.GetAppKeyWithCache(DataKey.RemoteApiForRelease);
        }
    }
}