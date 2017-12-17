using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class QQUserRepository : BaseRepository<QQUser>, IQQUser
    {
        public QQUser GetUserBy(string socialId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.QQUsers.Where(u => u.OpenId == socialId).FirstOrDefault();
            }
        }
    }
}
