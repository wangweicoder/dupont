using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Operator
{
    public class DtoCancelFarmerRequirement : BaseModel
    {        
        [Required]
        public long id { set; get; }
        [Required]
        public string OperatorUserId { set; get; }
        [Required]
        public string FarmerUserId { set; get; }      
        public int OrderState { get; set; }
       
    }
}