using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_USER_ROLE_RELATION
    {
        public T_USER_ROLE_RELATION()
        {
            Star = 0;
            TotalReplyCount = 0;
            TotalStarCount = 0;
        }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 是否为前台注册会员
        /// </summary>
        public bool MemberType { get; set; }
        /// <summary>
        /// 角色等级
        /// </summary>
        public Nullable<long> Star { get; set; }
        /// <summary>
        /// 累计总评星数
        /// </summary>
        public long? TotalStarCount { get; set; }
        /// <summary>
        /// 累计总评论数
        /// </summary>
        public long? TotalReplyCount { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public long AuditUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreateTime { get; set; }
    }
}
