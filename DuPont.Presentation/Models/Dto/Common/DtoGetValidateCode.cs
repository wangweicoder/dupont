using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Common
{
    public class DtoGetValidateCode : BaseModel
    {
        [Required]
        [SQLValidate]
        [PhoneNumber]
        public string phoneNumber { set; get; }

        public override string ToString()
        {
            return string.Format("model field and value:[cur_time]:{0},[phoneNumber]:{1} ", cur_time, phoneNumber);
        }
    }
}