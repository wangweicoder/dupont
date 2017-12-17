using DuPont.Utility.Core.MobileNotificationPusher.Core;
using DuPont.Utility.Core.MobileNotificationPusher.DefaultImplement.Apple;
using DuPont.Utility.Core.MobileNotificationPusher.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility
{
    public class NotificationPusher
    {
        private static PushBroker push = null;

        public static void Send(List<string> deviceTokenList, string message, string cerficationPath, string certificatePassword)
        {
            //推送器
            push = push ?? new PushBroker();

            //订阅推送的回调事件
            push.OnNotificationSent += NotificationSent;
            push.OnChannelException += ChannelException;
            push.OnServiceException += ServiceException;
            push.OnNotificationFailed += NotificationFailed;
            push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            push.OnChannelCreated += ChannelCreated;
            push.OnChannelDestroyed += ChannelDestroyed;

            //注册推送要用的证书
            push.RegisterAppleService(new ApplePushChannelSettings(true, File.ReadAllBytes(cerficationPath), certificatePassword),
                "yourAppId_production");

            //生成推送任务并放入到推送队列中
            foreach (var token in deviceTokenList)
            {
                if (token.Length == 64)
                {
                    try
                    {
                        push.QueueNotification(new AppleNotification()
                        .ForDeviceToken(token)
                        .WithAlert(message).WithBadge(1));
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            //等待所有的推送任务完成并停止推送服务
            push.StopAllServices();

        }
        public static void NotificationSent(object sender, INotification notification)
        {
            //消息推送成功后
            Console.WriteLine("Sent: " + sender + " -> " + notification);
        }

        static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
        {
            //推送失败
            Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
        }

        static void ChannelException(object sender, IPushChannel channel, Exception exception)
        {

            Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
        }

        static void ServiceException(object sender, Exception exception)
        {
            //服务异常
            Console.WriteLine("Service Exception: " + sender + " -> " + exception);
        }

        static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
        {
            //设备订阅过期,从数据库里移除过期的deviceToken
            Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
        }

        static void ChannelDestroyed(object sender)
        {
            Console.WriteLine("Channel Destroyed for: " + sender);
        }

        static void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            Console.WriteLine("Channel Created for: " + sender);
        }

        static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //把数据库中的oldSubscriptionId更新为newSubscriptionId
            //这个事件目前只有在 Android GCM  才会被触发
            Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
        }
    }
}
