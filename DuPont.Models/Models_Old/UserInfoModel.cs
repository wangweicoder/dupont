using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class UserInfoModel
    {
        public UserInfoModel()
        {
            MachineList = new List<ProductInfo>();
            RoleList = new List<RoleModel>();
            OneUserSkillInfo = new List<OneUserSkillModel>();
            SkillInfo = new List<SkillModel>();
            DPoint = 0;
        }

        public List<SkillModel> SkillInfo { get; set; }
        public long Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> DPoint { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Township { get; set; }
        public string Village { get; set; }
        /// <summary>
        /// 亩数
        /// </summary>
        /// <author>ww</author>
        public Nullable<int> Land { get; set; }
        /// <summary>
        /// 亩数审核状态
        /// </summary>
        /// <author>ww</author>
        public int LandAuditState { get; set; }
        public List<OneUserSkillModel> OneUserSkillInfo { get; set; }
        public List<RoleModel> RoleList { get; set; }

        public List<ProductInfo> MachineList { get; set; }
    }
}
