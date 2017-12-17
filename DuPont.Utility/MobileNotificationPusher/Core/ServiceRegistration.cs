using DuPont.Utility.Core.MobileNotificationPusher.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Utility.Core.MobileNotificationPusher.Core
{
    public class ServiceRegistration
    {
        public static ServiceRegistration Create<TNotification>(IPushService service, string applicationId = null)
        {
            return new ServiceRegistration()
            {
                ApplicationId = applicationId,
                Service = service,
                NotificationType = typeof(TNotification)
            };
        }

        public IPushService Service { get; set; }
        public string ApplicationId { get; set; }
        public Type NotificationType { get; set; }
    }
}
