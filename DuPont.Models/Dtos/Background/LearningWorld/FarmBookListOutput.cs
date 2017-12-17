using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class FarmBookListOutput
    {
        /// <summary>
        /// 农场名称
        /// </summary>
        public string FarmName { get; set; }

        public List<FarmBookItem> FarmBookList { get; set; }

    }

    public class FarmBookItem
    {
        /// <summary>
        /// 预约人
        /// </summary>
        public string Visitor { get; set; }

        /// <summary>
        /// 预约人手机号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 参观时间
        /// </summary>
        public DateTime VisitDate { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime BookDate { get; set; }
    }
}
