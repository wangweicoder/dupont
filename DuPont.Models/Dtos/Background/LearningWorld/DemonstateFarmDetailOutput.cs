using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    /// <summary>
    /// 示范农场详情输出类
    /// </summary>
    public class DemonstateFarmDetailOutput
    {
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
    }
}
