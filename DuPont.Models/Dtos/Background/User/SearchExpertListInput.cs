using DuPont.Models.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.User
{
    public class SearchExpertListInput
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int RoleId { get; set; }
        public long Province { get; set; }
        public long City { get; set; }
        public long Region { get; set; }


        [PhoneNumber]
        [DisplayName("手机号")]
        public string PhoneNumber { get; set; }

        [DateTime]
        [DisplayName("开始时间")]
        public string StartTime { get; set; }

        [DateTime]
        [DisplayName("结束时间")]
        public string EndTime { get; set; }
    }
}
