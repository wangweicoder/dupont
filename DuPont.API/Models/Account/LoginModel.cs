
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Account
{
    public partial class LoginModel
    {
        public string Token { get; set; }

        public Int64 UserId { get; set; }

        public List<T_ROLE> Roles { get; set; }

        public string RealName { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string DetailAddress { get; set; }

        public LoginModel()
        {
            UserId = 0;
            Token = string.Empty;
            RealName = string.Empty;
            PhoneNumber = string.Empty;
            DetailAddress = string.Empty;
            Roles = new List<T_ROLE>();
        }
    }
}