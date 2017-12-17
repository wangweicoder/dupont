using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class BusinessReplyDetailModel
    {

        /// <summary>
        /// 需求编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 需求发布日期
        /// </summary>
        public string PublishedDate { get; set; }
        /// <summary>
        /// 需求类别编号
        /// </summary>
        public int TypeId { get; set; }
        /// <summary>
        /// 需求类别描述
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 需求状态编号
        /// </summary>
        public int PublishStateId { get; set; }
        /// <summary>
        /// 需求状态描述
        /// </summary>
        public string PublishState { get; set; }
        /// <summary>
        /// 发布需求者id
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 发布需求者姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 发布需求者级别
        /// </summary>
        public long? UserLevel { get; set; }
        /// <summary>
        /// 发布需求者手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 期望的日期
        /// </summary>
        public string Dates { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailAddress { get; set; }
        /// <summary>
        /// 农作物编号
        /// </summary>
        public int CropId { get; set; }
        /// <summary>
        /// 农作物描述
        /// </summary>
        public string Crop { get; set; }
        /// <summary>
        /// 田地亩数区间编号
        /// </summary>
        public int AcreageId { get; set; }
        /// <summary>
        /// 田地亩数区间描述
        /// </summary>
        public string Acreage { get; set; }
        /// <summary>
        /// 预期出售最低价格
        /// </summary>
        public double ExpectedStartPrice { get; set; }
        /// <summary>
        /// 预期出售最高价格
        /// </summary>
        public double ExpectedEndPrice { get; set; }
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
        /// 发布者的角色编号
        /// </summary>
        public int PublisherRoleId { get; set; }
        /// <summary>
        /// 发布者的角色名称
        /// </summary>
        public string PublisherRoleName { get; set; }  
    }
}
