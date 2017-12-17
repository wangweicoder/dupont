using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Config
{
    public class SystemSettingViewModel
    {
        public int ID { get; set; }
        public string SETTING_ID { get; set; }
        public string SETTING_VALUE { get; set; }
        public string COMMENT { get; set; }
    }
}
