using DuPont.Presentation.DataAnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuPont.Presentation.Models.Dto.Operator
{
    public class DtoCommentRequirement : BaseModel
    {
        [Required]
        public long id { set; get; }
        [Required]
        public string OperatorUserid { set; get; }
        [Required]
        public long FarmerUserId { set; get; }
        [SQLValidate]
        public string CommentString { set; get; }
        [Required]
        public int Score { set; get; }
        /// <summary>
        /// 0 本站，1 E 田
        /// </summary>
        public int SourceType { set; get; }

        //(long id, long executeUserId, long userid, string commentString, int score)
    }
}