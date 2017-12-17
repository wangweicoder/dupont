using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Operator
{
    public class DtoReplyFarmerRequirement : BaseModel
    {
        [Required]
        public long id { set; get; }
        /// <summary>
        /// 靠谱作业机手id
        /// </summary>
        [Required]
        public string userId { set; get; }
        /// 地址
        /// </summary>
        public string Address { get; set; }
        [Required]
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNum { get; set; }
        /// <summary>
        /// 农机手姓名
        /// </summary>
        public string Name { get; set; }
        public string NickName { get; set; }
        public string OtherMachinery { get; set; }
        public string Credit { get; set; }
        public string SourceType { get; set; }
    }
}