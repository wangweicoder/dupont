using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Models.Models;
using DuPont.Interface;

namespace DuPont.Interface
{
    public interface INotification : IRepositoryBase<T_NOTIFICATION>
    {
        bool ExistsPublicNotification();

        T_NOTIFICATION GetOneValidPublicNotification();

        bool RemoveSameDeviceToken(string deviceToken);
    }
}
