using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_FARM_BOOKING
    {
        public int Id { get; set; }
        public int FarmId { get; set; }
        public long UserId { get; set; }
        public System.DateTime VisitDate { get; set; }
        public System.DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public virtual T_DEMONSTRATION_FARM T_DEMONSTRATION_FARM { get; set; }

        public virtual T_USER ReservedUser { get; set; }
    }
}
