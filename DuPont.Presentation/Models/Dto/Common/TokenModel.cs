﻿
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Account
{
    public partial class TokenModel
    {
        public string token { get; set; }
        public TokenModel()
        {            
            token = string.Empty;
          
        }
    }
}