﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Expert
{
    public class ExpertMyReplyListInput
    {
        public long ReplyUserId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
