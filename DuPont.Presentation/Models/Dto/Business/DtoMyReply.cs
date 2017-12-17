﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Business
{
    public class DtoMyReply : BaseModel
    {
        [Required]
        public int pageindex { set; get; }
        [Required]
        public int pagesize { set; get; }
        [Required]
        public int isclosed { set; get; }
        [Required]
        public long userid { set; get; }

    }
}