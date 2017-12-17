using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class WeChatUserRepository : BaseRepository<WeChatUser>, IWeChatUser
    {
        public WeChatUser GetUserBy(string socialId)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                return dbContext.WeChatUsers.Where(u => u.UnionId != null && u.UnionId == socialId).FirstOrDefault();
            }
        }
    }
}
