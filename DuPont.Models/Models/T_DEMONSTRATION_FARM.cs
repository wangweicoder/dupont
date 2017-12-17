using System;
using System.Collections.Generic;

namespace DuPont.Models.Models
{
    public partial class T_DEMONSTRATION_FARM
    {
        public T_DEMONSTRATION_FARM()
        {
            this.T_FARM_AREA = new List<T_FARM_AREA>();
            this.T_FARM_BOOKING = new List<T_FARM_BOOKING>();
        }

        public int Id { get; set; }
        public string ProvinceAid { get; set; }
        public string CityAid { get; set; }
        public string RegionAid { get; set; }
        public string Name { get; set; }
        public bool IsOpen { get; set; }
        public Nullable<System.DateTime> OpenStartDate { get; set; }
        public Nullable<System.DateTime> OpenEndDate { get; set; }
        public string PlantArea { get; set; }
        public string Variety { get; set; }
        public string SowTime { get; set; }
        public string PlantPoint { get; set; }
        public System.DateTime CreateTime { get; set; }
        public bool IsDeleted { get; set; }
        public virtual List<T_FARM_AREA> T_FARM_AREA { get; set; }
        public virtual List<T_FARM_BOOKING> T_FARM_BOOKING { get; set; }
    }
}
