using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.User
{
    public class BackgroundUserModel
    {
        public long Id { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string RealName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LoginToken { get; set; }
        public string AvartarUrl { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public string DetailedAddress { get; set; }
        public bool IsLock { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string RoleName { get; set; }
    }
}
