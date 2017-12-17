using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Account
{
    public class Logout : BaseModel
    {
        [Required]
        [SQLValidate]
        public string userID { get; set; }

        [SQLValidate]
        public string encrypt { get; set; }
        [SQLValidate]
        public string Token { get; set; }
    }
}