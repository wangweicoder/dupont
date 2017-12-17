using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class VM_GET_USER_ROLE_INFO_LIST
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string UserName { get; set; }
        public long UserId { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<int> Land { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int Type { get; set; }
    }
}
