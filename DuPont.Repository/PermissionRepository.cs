
using DuPont.Interface;
using DuPont.Models.Models;
using System.Linq;
namespace DuPont.Repository
{
    public class PermissionRepository : IPermission
    {

        /// <summary>
        /// 检查是否拥有权限
        /// </summary>
        /// <param name="userId">访问者的用户编号</param>
        /// <param name="url">访问者当前访问的地址：格式：/ControllerName/ActionName</param>
        /// <returns>true:拥有权限  false:没有权限</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool HaveAuthority(long userId, string url)
        {
            using (var dbContext = new DuPont_TestContext())
            {

                var query = from mnu in dbContext.T_MENU
                            join mnu_rr in dbContext.T_MENU_ROLE_RELATION on mnu.Id equals mnu_rr.MenuId
                            where (
                                  from u_rr in dbContext.T_USER_ROLE_RELATION
                                  where u_rr.UserID == userId && !u_rr.MemberType
                                  select u_rr.RoleID
                            ).Contains(mnu_rr.RoleId)
                            && mnu.Url == url
                            select mnu.Id;

                bool havePermission = query.FirstOrDefault() != 0;
                return havePermission;
            }
        }
    }
}
