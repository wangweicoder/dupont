using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoCarouselPictures : BaseModel
    {
        [Required(ErrorMessage = "角色编号为必填项")]
        [RegularExpression(@"^\+?[1-9][0-9]*$", ErrorMessage = "角色编号必须为数字")]
        public int RoleId { set; get; }
    }
}