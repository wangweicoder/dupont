using DuPont.Utility.Core.MobileNotificationPusher.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.Core.MobileNotificationPusher.Interfaces
{
    public delegate void SendNotificationCallbackDelegate(object sender, SendNotificationResult result);

    public delegate void PushChannelExceptionDelegate(object sender, Exception ex);

    public interface IPushService : IDisposable
    {
        event ChannelCreatedDelegate OnChannelCreated;
        event ChannelDestroyedDelegate OnChannelDestroyed;
        event NotificationSentDelegate OnNotificationSent;
        event NotificationFailedDelegate OnNotificationFailed;
        event NotificationRequeueDelegate OnNotificationRequeue;
        event ChannelExceptionDelegate OnChannelException;
        event ServiceExceptionDelegate OnServiceException;
        event DeviceSubscriptionExpiredDelegate OnDeviceSubscriptionExpired;
        event DeviceSubscriptionChangedDelegate OnDeviceSubscriptionChanged;

        IPushChannelFactory PushChannelFactory { get; }
        IPushServiceSettings ServiceSettings { get; }
        IPushChannelSettings ChannelSettings { get; }
        bool IsStopping { get; }
        void QueueNotification(INotification notification);
        void Stop(bool waitForQueueToFinish = true);
    }
}
