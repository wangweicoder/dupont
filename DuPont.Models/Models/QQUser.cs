using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public class QQUser : T_USER
    {
        public QQUser()
        {
            Type = (int)UserTypes.QQUser;
        }
        public string OpenId { get; set; }
    }
}
