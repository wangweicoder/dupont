using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Demand
{
    public class FarmerSeachModel
    {
        public FarmerSeachModel()
        {
            pageIndex = 1;
            pageSize = 10;
            DemandTypeId = string.Empty;
            PublishStateId = string.Empty;
        }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string DemandTypeId { get; set; }
        public string PublishStateId { get; set; }

        [RegularExpression(@"\d+", ErrorMessage = "省份数据格式错误!")]
        public string ProvinceAid { get; set; }

        [RegularExpression(@"\d+", ErrorMessage = "城市数据格式错误!")]
        public string CityAid { get; set; }

        [RegularExpression(@"\d+", ErrorMessage = "区县数据格式错误!")]
        public string RegionAid { get; set; }

        /// <summary>
        /// 删除状态
        /// </summary>
        public bool? IsDeleted { get; set; }
    }
}
