using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Account
{
    public class RoleModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public RoleModel()
        {
            RoleId = 0;
            RoleName = string.Empty;
        }
    }
}