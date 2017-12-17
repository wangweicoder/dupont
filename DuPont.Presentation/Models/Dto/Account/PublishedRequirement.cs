using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class PublishedRequirement : BaseModel
    {
        [Required(ErrorMessage="用户编号为必填项")]
        public long userId { get; set; }
        [Required(ErrorMessage = "发布状态为必填项")]
        public int isClosed { get; set; }
        [Required(ErrorMessage = "角色类别必填项")]
        public int roleType { get; set; }
        [Required(ErrorMessage = "页码为必填项")]
        public int pageIndex { get; set; }
        [Required(ErrorMessage = "页容量为必填项")]
        public int pageSize { get; set; }
    }
}