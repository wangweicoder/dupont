using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Common
{
    public class NearbyOperatorListInput
    {
        [Required(ErrorMessage = "参数UserId不可为空!")]
        public long UserId { get; set; }

        ///// <summary>
        ///// 需求类型编号
        ///// </summary>
        //[Required(ErrorMessage = "需求类型不可为空!")]
        public int? DemandTypeId { get; set; }

        //[Required(ErrorMessage = "参数PageIndex不可为空!")]
        //[Range(1, int.MaxValue)]
        public int? PageIndex { get; set; }

        //[Required(ErrorMessage = "参数PageSize不可为空!")]
        public int? PageSize { get; set; }
    }
}
