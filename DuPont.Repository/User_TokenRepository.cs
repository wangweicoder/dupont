
using DuPont.Interface;
using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class User_TokenRepository : BaseRepository<T_User_Token>, IUserToken
    {
        /// <summary>
        /// 根据用户token获得用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>

        public T_User_Token GetByToken(string token)
        {
            using (var dbContext = new DuPont_TestContext())
            {
                var model = dbContext.UserToken.SingleOrDefault(u => u.Token == token);
                return model;
            }
        }
    }
}
