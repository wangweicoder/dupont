using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_FARM_AREA
    {
        public int Id { get; set; }
        public int FarmId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public System.DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFarmMachinery { get; set; }
        public virtual T_DEMONSTRATION_FARM T_DEMONSTRATION_FARM { get; set; }
    }
}
