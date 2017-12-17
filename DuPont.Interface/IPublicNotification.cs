using DuPont.Interface;
using DuPont.Models.Models;
using System.Collections.Generic;
namespace DuPont.Interface
{
    public interface IPublicNotification : IRepositoryBase<T_SEND_COMMON_NOTIFICATION_PROGRESS>
    {
        List<T_USER> GetPublicNotificationValidUser(IEnumerable<T_USER> users, long msgId);
    }
}
