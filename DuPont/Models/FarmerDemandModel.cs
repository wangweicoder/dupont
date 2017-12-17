using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Models
{
    public class FarmerDemandModel
    {
        /// <summary>
        /// 需求ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 需求创建人 
        /// </summary>
        public string CreateUser { get; set; }
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
        /// 发布状态
        /// </summary>
        public string PublishState { get; set; }
        /// <summary>
        /// 需求类型
        /// </summary>
        public string DemandType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 农作物编号
        /// </summary>
        public int CropId { get; set; }
        /// <summary>
        /// 亩数编号
        /// </summary>
        public int AcresId { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Brief { get; set; }
        /// <summary>
        /// 期望的日期
        /// </summary>
        public string ExpectedDate { get; set; }
        /// <summary>
        /// 评星数
        /// </summary>
        public int Score { get; set; }
    }
}