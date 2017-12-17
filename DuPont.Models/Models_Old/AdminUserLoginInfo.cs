using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class AdminUserLoginInfo
    {
        /// <summary>
        /// 角色组
        /// </summary>
        public List<T_USER_ROLE_RELATION> Roles { get; set; }

        /// <summary>
        /// 是否是超级管理员,管理员
        /// </summary>
        public bool IsSuperOrAdministrator
        {
            get
            {
                if (Roles == null)
                    return false;

                if (User == null)
                    return false;

                if (User.IsSuperAdmin)
                    return true;

                return Roles.Where(m => m.RoleID == (int)RoleType.Admin).Count() > 0;
            }
        }

        /// <summary>
        /// 个人信息
        /// </summary>
        public T_ADMIN_USER User { get; set; }
    }
}
