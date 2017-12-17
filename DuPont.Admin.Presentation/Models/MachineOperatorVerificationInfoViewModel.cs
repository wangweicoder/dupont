using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Admin.Presentation.Models
{
    public class MachineOperatorVerificationInfoViewModel:T_MACHINERY_OPERATOR_VERIFICATION_INFO
    {
        public string UserName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public List<string> Pictures { get; set; }
        public List<ProductInfo> MachineList { get; set; }
        public List<T_SYS_DICTIONARY> SkillList { get; set; }
        /// <summary>
        /// APP接口服务的地址
        /// </summary>
        public string RemoteApiUrl { get; set; }
    }
}