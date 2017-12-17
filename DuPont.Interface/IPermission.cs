using System;

namespace DuPont.Interface
{
    /// <summary>
    /// 页面访问权限检查接口
    /// </summary>
    public interface IPermission
    {
        /// <summary>
        /// 检查是否拥有权限
        /// </summary>
        /// <param name="userId">访问者的用户编号</param>
        /// <param name="url">访问者当前访问的地址：格式：/ControllerName/ActionName</param>
        /// <returns>true:拥有权限  false:没有权限</returns>
        bool HaveAuthority(Int64 userId,string url);
    }
}
