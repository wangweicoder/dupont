using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.Core.MobileNotificationPusher.Core
{
    public class MaxSendAttemptsReachedException : Exception
    {
        public MaxSendAttemptsReachedException() : base("The maximum number of Send attempts to send the notification was reached!") { }
    }

    public class DeviceSubscriptonExpiredException : Exception
    {
        public DeviceSubscriptonExpiredException()
            : base("Device Subscription has Expired")
        {
        }
    }
}
