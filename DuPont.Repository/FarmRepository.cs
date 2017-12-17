using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DuPont.Interface;
using DuPont.Models.Models;
using EntityFramework.Extensions;
namespace DuPont.Repository
{
    public class FarmRepository : BaseRepository<T_DEMONSTRATION_FARM>, IFarm
    {
        public int DeleteFarm(params int[] farmIds)
        {
            using (var ctx = GetDbContextInstance())
            {
                return ctx.Set<T_DEMONSTRATION_FARM>()
                      .Where(m => farmIds.Contains(m.Id))
                      .Update(u => new T_DEMONSTRATION_FARM { IsDeleted = true });
            }
        }
    }
}
