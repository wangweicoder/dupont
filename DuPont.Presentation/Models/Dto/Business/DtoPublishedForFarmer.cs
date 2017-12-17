using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Business
{
    public class DtoPublishedForFarmer : BaseModel
    {
        [Required]
        public long userId { set; get; }
        [Required]
        public int pageIndex { set; get; }
        [Required]
        public int pageSize { set; get; }
        [Required]
        public int type { set; get; }

        [SQLValidate]
        public string region { set; get; }
        [SQLValidate]
        public string orderfield { set; get; }
    }
}