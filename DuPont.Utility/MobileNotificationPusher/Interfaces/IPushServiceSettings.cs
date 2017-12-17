using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.Core.MobileNotificationPusher.Interfaces
{
    public interface IPushServiceSettings
    {
        bool AutoScaleChannels { get; set; }
        int MaxAutoScaleChannels { get; set; }
        long MinAvgTimeToScaleChannels { get; set; }
        int Channels { get; set; }
        int MaxNotificationRequeues { get; set; }
        int NotificationSendTimeout { get; set; }
        TimeSpan IdleTimeout { get; set; }
    }
}
