using DuPont.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuPont.Infrastructure
{
    public class AdminPermissionProvider:IPermissionProvider
    {
        private IAdminUser adminUserRepository;
        private IPermission permissionRepository;

        public AdminPermissionProvider(IAdminUser adminUserRepository, IPermission permissionRepository)
        {
            this.adminUserRepository = adminUserRepository;
            this.permissionRepository = permissionRepository;
        }

        public bool HaveAuthority(long userId, string url)
        {
            //获取用户信息
            var userInfo = adminUserRepository.GetByKey(userId);
            //是否为超级管理员
            if (userInfo.IsSuperAdmin)
                return true;

            return permissionRepository.HaveAuthority(userId, url);
        }
    }
}