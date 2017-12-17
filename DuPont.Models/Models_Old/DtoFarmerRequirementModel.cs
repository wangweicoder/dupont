using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Models.Models
{
    public class DtoFarmerRequirementModel
    {

        public DtoFarmerRequirementModel()
        {
            SourceType = 1;
            Machine_sum = "1";
            Chunk = "1";
            Expect_price = "0";
        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 大农户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 大农户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 需求类型编号
        /// </summary>
        public int DemandTypeId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNum { get; set; }        
        /// <summary>
        ///农作物类型
        /// </summary>
        public int CropId { get; set; }        
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 亩数
        /// </summary>
        public string Acreage { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Brief { get; set; }
        /// <summary>
        /// 农机数量
        /// </summary>
        public string Machine_sum { get; set; }
        /// <summary>
        /// 土地块数
        /// </summary>
        public string Chunk { get; set; }
        /// <summary>
        /// 多少钱一亩
        /// </summary>
        public string Expect_price { get; set; }
       /// <summary>
       /// 发布日期
       /// </summary>
        public System.DateTime CreateTime { get; set; }
        /// <summary>
        /// 订单来源（1代表先锋帮给E田的订单）
        /// </summary>
        public int SourceType { get; set; }      
        /// <summary>
        /// 作业开始时间
        /// </summary>
        public string StartDate { get; set; }       
        /// <summary>
        /// 作业结束时间
        /// </summary>
        public string EndDate { get; set; }
    }
}