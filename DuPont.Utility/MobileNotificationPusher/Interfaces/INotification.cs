using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.Core.MobileNotificationPusher.Interfaces
{
    public interface INotification
    {
        object Tag { get; set; }
        int QueuedCount { get; set; }
        bool IsValidDeviceRegistrationId();
        DateTime EnqueuedTimestamp { get; set; }
    }
}
