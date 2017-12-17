using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class ExhibitionAreaListOutput
    {
        /// <summary>
        /// 分区编号
        /// </summary>
        public int ExhibitionAreaId { get; set; }

        /// <summary>
        /// 分区名称
        /// </summary>
        public string ExhibitionAreaName { get; set; }

        /// <summary>
        /// 是否是农机区
        /// </summary>
        public bool IsFarmMachinery { get; set; }
    }
}
