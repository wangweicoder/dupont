using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Business
{
    public class DtoCommentRequirement : BaseModel
    {
        [Required]
        public int isOwn { set; get; }
        [Required]
        public long id { set; get; }
        [Required]
        public long userid { set; get; }
        [SQLValidate]
        public string commentString { set; get; }
        [Required]
        public int score { set; get; }
    }
}