using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class WeChatUser : T_USER
    {
        public WeChatUser()
        {
            Type = (int)UserTypes.WeChatUser;
        }
        public string UnionId { get; set; }
    }
}
