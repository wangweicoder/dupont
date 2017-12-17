using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoUploadPicture : BaseModel
    {
        [Required]
        public Int64 UserId { set; get; }
        [Required]
        public HttpPostedFileBase Pic { set; get; }

    }
}