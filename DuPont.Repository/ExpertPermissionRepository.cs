using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DuPont.Interface;
using DuPont.Models.Models;

namespace DuPont.Repository
{
    public class ExpertPermissionRepository : BaseRepository<T_EXPERT>, IExpertPermission
    {

        public int GrantOrCancelExpert(bool grantExpert, params long[] userIds)
        {
            using (var ctx = GetDbContextInstance())
            {
                return ctx.Database.ExecuteSqlCommand("update " + typeof(T_EXPERT).Name + " set IsEnabled="+(grantExpert?1:0)+" where UserId in(" + string.Join(",", userIds) + ")");
            }
        }
    }
}
