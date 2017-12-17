using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Admin.Presentation.Models
{
    public class BusinessVerificationInfoViewModel
    {

        public long Id { get; set; }
        public long UserId { get; set; }
        public string PurchaseType { get; set; }
        public string Introduction { get; set; }
        public string PictureIds { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> AuditUserId { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public int AuditState { get; set; }
        public string RejectReason { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public string RealName { get; set; }
        public string PhoneNumber { get; set; }
        public string DetailAddress { get; set; }
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