using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.FarmerRequirement
{
    public class DtoSaveRequirement : BaseModel
    {
        [Required]
        public long id { set; get; }
        [Required]
        public long userId { set; get; }
        [Required]
        public int type { set; get; }
        [Required]
        public int cropId { set; get; }
        [Required]
        public int acreage { set; get; }
        [SQLValidate]
        public string description { set; get; }
        [SQLValidate]
        [Required]
        public string date { set; get; }
        [SQLValidate]
        [Required]
        public string address { set; get; }
        [SQLValidate]
        public string detailAddress { set; get; }
        [SQLValidate]
        [Required]
        public string PhoneNumber { set; get; }
        
        public double ExpectedStartPrice { set; get; }
        
        public double ExpectedEndPrice { set; get; }

        //(long id, long userId, int type, int cropId, int acreage, string description, string date,
        //string address, string detailAddress, string PhoneNumber, double ExpectedStartPrice = 0, double ExpectedEndPrice = 0)
    }
}