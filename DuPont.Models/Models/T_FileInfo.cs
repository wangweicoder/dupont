using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_FileInfo
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Path { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
