using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models
{
    public partial class T_User_Token
    {
        public T_User_Token()
        {            
            IsDeleted = false;           
        }
        public long Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }       
        public string Password { get; set; }
        public string Token { get; set; }
        public DateTime? CreateTime { get; set; }      
        public Nullable<System.DateTime> DeletedTime { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
        /// <summary>
        /// 最近一次登录的时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; } 
        /// <summary>
        ///  用户类型
        /// </summary>
        public int? UserType { get; set; }      
       
    }
}
