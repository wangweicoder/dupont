using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Models
{
    public class BusinessDemandModel
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
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 农作物编号
        /// </summary>
        public int CropId { get; set; }
        /// <summary>
        /// 起购重量
        /// </summary>
        public int FirstWeight { get; set; }
        /// <summary>
        /// 收购区间
        /// </summary>
        public int AcquisitionWeightRangeTypeId { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Brief { get; set; }
        /// <summary>
        /// 期望的日期
        /// </summary>
        public string ExpectedDate { get; set; }
        /// <summary>
        /// 期望价格上限
        /// </summary>
        public decimal? ExpectedStartPrice { get; set; }
        /// <summary>
        /// 期望价格下限
        /// </summary>
        public decimal? ExpectedEndPrice { get; set; }
    }
}