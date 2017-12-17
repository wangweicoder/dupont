using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.FarmerRequirement
{
    public class DtoAcceptTask : BaseModel
    {
        [Required]
        public long id { set; get; }
        [Required]
        public long userId { set; get; }
        [Required]
        public int weightrangetypeid { set; get; }
        [SQLValidate]
        public string address { set; get; }
        [Required]
        [SQLValidate]
        public string phonenumber { set; get; }
        [SQLValidate]
        public string brief { set; get; }
        //(long id, long userId, int weightrangetypeid, string address, string phonenumber, string brief)
    }
}