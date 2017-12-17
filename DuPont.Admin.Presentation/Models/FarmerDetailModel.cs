using DuPont.Models.Dtos.Background.Demand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Admin.Presentation.Models
{
    public class FarmerDetailModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 需求类型
        /// </summary>
        public string DemandType { get; set; }
        /// <summary>
        /// 农作物
        /// </summary>
        public string Crop { get; set; }
        /// <summary>
        /// 品种
        /// </summary>
        public string Variety { get; set; }
        /// <summary>
        /// 发布状态
        /// </summary>
        public string PublishState { get; set; }
        /// <summary>
        /// 田间亩数
        /// </summary>
        public string Acres { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Brief { get; set; }
        /// <summary>
        /// 期望日期
        /// </summary>
        public string ExpectedDate { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        public string TimeSlot { get; set; }
        /// <summary>
        /// 价格下限
        /// </summary>
        public decimal? ExpectedStartPrice { get; set; }
        /// <summary>
        /// 期望价格上限
        /// </summary>
        public decimal? ExpectedEndPrice { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string DetailedAddress { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public List<FarmerDemandReplyItem> ReplyList { get; set; }
    }
}