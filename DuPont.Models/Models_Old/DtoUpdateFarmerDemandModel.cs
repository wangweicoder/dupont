using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Models.Models
{
    public class DtoUpdateFarmerDemandModel
    {

        public DtoUpdateFarmerDemandModel()
        {

        }
        /// <summary>
        /// 订单编号
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 需求状态编号
        /// </summary>
        public int OrderState { get; set; }
        /// <summary>
        /// 农机手名称
        /// </summary>
        public string OperatoName { get; set; }
        /// <summary>
        ///大农户名称
        /// </summary>
        public string FarmerName { get; set; }
        public string CommentString { set; get; }
        public int Score { set; get; }

    }
}