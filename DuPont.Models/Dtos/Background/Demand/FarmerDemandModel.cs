using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webdiyer.WebControls.Mvc;

namespace DuPont.Models.Dtos.Background.Demand
{
    public class FarmerDemandViewModel
    {
        public PagedList<string> Pager { get; set; }
        public List<FarmerDemandModel> PendingAuditList { get; set; }
        public FarmerSeachModel Wheremodel { get; set; }

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
            /// 删除状态
            /// </summary>
            public bool IsDeleted { get; set; }
        }
    }
}
