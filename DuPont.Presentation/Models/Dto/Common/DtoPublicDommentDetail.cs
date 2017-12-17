using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoPublicDommentDetail:BaseModel
    {
        [Required(ErrorMessage = "用户编号为必填项")]
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "用户编号必须为数字")]
        public long userid { set; get; }        
        [Required(ErrorMessage = "页码为必填项")]
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "页码必须为数字")]
        public int pageindex { set; get; }
        [Required(ErrorMessage = "页容量为必填项")]
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "页容量必须为数字")]
        public int pagesize { set; get; }
    }
}