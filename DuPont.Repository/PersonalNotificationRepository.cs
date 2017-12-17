using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuPont.Interface;
using DuPont.Repository;
using DuPont.Models.Models;
namespace DuPont.Repository
{
    public class PersonalNotificationRepository : BaseRepository<T_SEND_NOTIFICATION_RESULT>, IPersonalNotification
    {
    }
}
