using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Models.Models;

namespace DuPont.Interface
{
    public interface IFarm : IRepositoryBase<T_DEMONSTRATION_FARM>
    {
        int DeleteFarm(params int[] farmIds);
    }
}
