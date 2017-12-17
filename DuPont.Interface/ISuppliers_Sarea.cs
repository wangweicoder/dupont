
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
    public interface ISuppliers_Sarea : IRepository<T_SUPPLIERS_AREA>
    {
        List<Suppliers_AreaModel> GetManageArea(long UserId);
    }
}
