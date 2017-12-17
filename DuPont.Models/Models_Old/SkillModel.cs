using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class SkillModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<T_SYS_DICTIONARY> Skill { get; set; }
    }
}
