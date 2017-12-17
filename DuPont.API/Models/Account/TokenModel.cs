
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Account
{
    public partial class TokenModel
    {
        public string Token { get; set; }
        public TokenModel()
        {            
            Token = string.Empty;
          
        }
    }
}