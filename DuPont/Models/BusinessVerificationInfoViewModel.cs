using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Models
{
    public class BusinessVerificationInfoViewModel:T_BUSINESS_VERIFICATION_INFO
    {
        public string UserName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string TownshipName { get; set; }
        public string VillageName { get; set; }
        public string[] Pictures { get; set; }
        public List<T_SYS_DICTIONARY> SkillList { get; set; }
        /// <summary>
        /// APP接口服务的地址
        /// </summary>
        public string RemoteApiUrl { get; set; }

    }
}