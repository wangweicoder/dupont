using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.User
{
    public class SearchExpertListOutput
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public bool IsExpert { get; set; }
        public string PhoneNumber { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
