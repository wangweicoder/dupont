using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models.Mapping
{
    public class WeChatUserMap : EntityTypeConfiguration<WeChatUser>
    {
        public WeChatUserMap()
        {
            this.Map(m =>
            {
                var userType = (int)UserTypes.WeChatUser;
                m.Requires("UserType").HasValue(userType);
            });
        }
    }
}
