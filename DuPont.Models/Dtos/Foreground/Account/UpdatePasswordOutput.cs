using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Foreground.Account
{
    public class UpdatePasswordOutput
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string LoginToken { get; set; }
        public string AvartarUrl { get; set; }
        public string UserName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public Nullable<int> DPoint { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        public string DetailedAddress { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<long> ModifiedUserId { get; set; }
        public Nullable<long> DeletedUserId { get; set; }
        public Nullable<System.DateTime> DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
        public string SmsCode { get; set; }
        public string LoginUserName { get; set; }
    }
}
