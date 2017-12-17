using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.Core.MobileNotificationPusher.Interfaces
{
    public interface IPushChannel : IDisposable
    {
        void SendNotification(INotification notification, SendNotificationCallbackDelegate callback);
    }
}
