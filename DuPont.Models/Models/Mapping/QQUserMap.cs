using DuPont.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models.Mapping
{
    public class QQUserMap : EntityTypeConfiguration<QQUser>
    {
        public QQUserMap()
        {
            this.Map(m =>
            {
                var userType = (int)UserTypes.QQUser;
                m.Requires("UserType").HasValue(userType);
            });
        }
    }
}
