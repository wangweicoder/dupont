using DuPont.Utility.Core.MobileNotificationPusher.Core;
using DuPont.Utility.Core.MobileNotificationPusher.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.Core.MobileNotificationPusher.DefaultImplement.Apple
{
    public static class ApplePushBrokerExtensions
    {
        public static void RegisterAppleService(this PushBroker broker, ApplePushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
        {
            RegisterAppleService(broker, channelSettings, null, serviceSettings);
        }

        public static void RegisterAppleService(this PushBroker broker, ApplePushChannelSettings channelSettings, string applicationId, IPushServiceSettings serviceSettings = null)
        {
            broker.RegisterService<AppleNotification>(new ApplePushService(channelSettings, serviceSettings), applicationId);
        }

        public static AppleNotification AppleNotification(this PushBroker broker)
        {
            return new AppleNotification();
        }
    }
}
