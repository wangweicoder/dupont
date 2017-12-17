
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.API.Models.Account
{
    [MetadataType(typeof(LoginModel))]
    public class LoginModelMetaData
    {
        [Required]
        public string Token { get; set; }

        public Int64 UserId { get; set; }

        public List<T_ROLE> Roles { get; set; }

        public string RealName { get; set; }

        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public string DetailAddress { get; set; }
    }

}