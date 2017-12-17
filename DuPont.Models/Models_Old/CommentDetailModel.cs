using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class CommentDetailModel
    {
        /// <summary>
        /// 评价者的用户编号
        /// </summary>
        public long ExecuteUserId { get; set; }
        /// <summary>
        /// 评价者的角色编号
        /// </summary>
        public int ExecuteRoleId { get; set; }
        /// <summary>
        /// 评价者的角色名称
        /// </summary>
        public string ExecuteRoleName { get; set; }
        public string ExcuteUserName { get; set; }//执行评价的人的姓名
        public string Comments { get; set; }//评价的内容
        public int Score { get; set; }//评价的分数
        public string CommentsTime { get; set; }//评价的时间
    }
}
