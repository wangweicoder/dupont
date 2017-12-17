using DuPont.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Interface
{
   public interface IUserToken : IRepositoryBase<T_User_Token>
    {

       T_User_Token GetByToken(string token);
    }
}
