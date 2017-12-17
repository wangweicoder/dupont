using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface IOperatorInfoVerifciationRepository : IRepositoryBase<T_MACHINERY_OPERATOR_VERIFICATION_INFO>
    {
        Task<int> UpdateDEMAND(T_FARMER_PUBLISHED_DEMAND fmodel,long id, string OperatorUserId);
       
    }
}
