using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class ReplyDetailModel
    {
        /// <summary>
        /// 应答者id
        /// </summary>
        public long ReplyUserId { get; set; }
        /// <summary>
        /// 应答者姓名
        /// </summary>
        public string ReplyUserName { get; set; }
        /// <summary>
        /// 应答者电话
        /// </summary>
        public string ReplyPhoneNumber { get; set; }
        /// <summary>
        /// 应答者地址
        /// </summary>
        public string ReplyDetailedAddress { get; set; }
        /// <summary>
        /// 应答者时间
        /// </summary>
        public string ReplyTime { get; set; }
        /// <summary>
        /// 应答备注
        /// </summary>
        public string ReplyRemark { get; set; }
        /// <summary>
        /// 评价分数
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 应答重量范围编号
        /// </summary>
        public int ReplyWeightId { get; set; }
        /// <summary>
        /// 应答重量范围描述
        /// </summary>
        public string ReplyWeight { get; set; }

        /// <summary>
        /// 应答者的角色编号
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 应答者的角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}
