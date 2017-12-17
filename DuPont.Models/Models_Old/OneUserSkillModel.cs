using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class OneUserSkillModel
    {
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public int Star { get; set; }
        public int SkillCode { get; set; }
    }
}
