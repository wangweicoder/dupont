using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models.Mapping
{
    public class T_NOTIFICATIONMap : EntityTypeConfiguration<T_NOTIFICATION>
    {
        public T_NOTIFICATIONMap()
        {
            this.HasKey(m => m.MsgId);
            this.Property(m => m.IsPublic).IsRequired();
            this.Property(m => m.MsgContent).HasMaxLength(250).IsRequired();
            this.Property(m => m.CreateTime).IsRequired();
            this.Property(m => m.IsDeleted).IsRequired();
            this.Property(m => m.IsOnDate).IsRequired();
            this.Property(m => m.SendOnDate).IsOptional();

            this.HasOptional(m => m.TargetUser)
                .WithMany(m => m.Notifications)
                .HasForeignKey(m => m.TargetUserId);

        }
    }
}
