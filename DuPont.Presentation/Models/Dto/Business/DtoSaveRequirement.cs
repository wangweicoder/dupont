using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Business
{
    public class DtoSaveRequirement : BaseModel
    {
        [Required]
        public long id { set; get; }
        [Required]
        public long userid { set; get; }
        [Required]
        public int Type { set; get; }
        [Required]
        [SQLValidate]
        public string Dates { set; get; }
        [Required]
        [SQLValidate]
        public string Address { set; get; }
        [SQLValidate]
        public string DetailAddress { set; get; }
        [Required]
        public int PurchaseWeight { set; get; }
        [Required]
        public int CommenceWeight { set; get; }
        [Required]
        [PhoneNumber]
        [SQLValidate]
        public string PhoneNumber { set; get; }
        [SQLValidate]
        public string Remark { set; get; }
        [Required]
        public int cropId { set; get; }
        public double PurchaseStartPrice { set; get; }
        public double PurchaseEndPrice { set; get; }

    }
}