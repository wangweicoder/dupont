
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Admin.Presentation.Models
{
    public class UserModelWithNoPager
    {
        public List<VM_GET_USER_ROLE_INFO_LIST> PendingAuditList { get; set; }
        public WhereModel Wheremodel { get; set; }
        #region 字段
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 用户名字
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区/县
        /// </summary>

        public string Region { get; set; }
        /// <summary>
        /// 乡镇
        /// </summary>
        public string Township { get; set; }
        /// <summary>
        /// 村
        /// </summary>
        public string Village { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>

        public string CreateTime { get; set; }
        #endregion

    }
}