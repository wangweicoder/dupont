using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_MENU
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public bool Visible { get; set; }
        public int ParentId { get; set; }
        public string Url { get; set; }
        public Nullable<int> Order { get; set; }
    }
}
