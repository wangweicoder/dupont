using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class WhereModel
    {
        /// <summary>
        /// 角色类别
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        //[RegularExpression(@"^[1]+[3,5,7,8]+\d{9}")]
        [Required(ErrorMessage = "手机号")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 省编号
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 市编号
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县编号
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 权限范围
        /// </summary>

        public List<string> SuppliersWhere { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "开始时间")]


        public System.DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Required(ErrorMessage = "结束时间")]

        public System.DateTime EndTime { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public int? UserTypeId { get; set; }
    }
}
