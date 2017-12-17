using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Models.Models;

namespace DuPont.Interface
{
    public interface IExpertPermission : IRepositoryBase<T_EXPERT>
    {
       int GrantOrCancelExpert(bool grantExpert,params long[] userIds);
    }
}
