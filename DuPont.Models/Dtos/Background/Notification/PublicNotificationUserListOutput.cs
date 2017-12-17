using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Dtos.Background.Notification
{
    public class PublicNotificationUserListOutput
    {
        public long UserId { get; set; }
        public string IosDeviceToken { get; set; }
        public string PhoneNumber { get; set; }
    }
}
