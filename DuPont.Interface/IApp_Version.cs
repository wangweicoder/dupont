using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{

    public interface IApp_Version : IRepository<T_APP_VERSION>
    {
        bool SaveAppFile(T_APP_VERSION entity);
    }
}
