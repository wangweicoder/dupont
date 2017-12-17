using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models
{
    public class RegisterModel
    {
        public string Token { get; set; }

        public Int64 UserId { get; set; }

        public RegisterModel()
        {
            Token = string.Empty;
            UserId = 0;
        }
    }
}