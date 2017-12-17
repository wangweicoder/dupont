using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models.Mapping
{
    public class T_VISITOR_RECEIVED_NOTIFICATIONMap : EntityTypeConfiguration<T_VISITOR_RECEIVED_NOTIFICATION>
    {
        public T_VISITOR_RECEIVED_NOTIFICATIONMap()
        {
            this.HasKey(t => new { t.MsgId, t.DeviceToken });
            this.Property(t => t.DeviceToken)
                .HasMaxLength(100)
                .HasColumnType("varchar")
                .HasColumnName("DeviceToken")
                .IsRequired();

            this.Property(t => t.OsType)
                .HasMaxLength(15)
                .HasColumnType("varchar")
                .HasColumnName("OsType")
                .IsRequired();

            this.Property(t => t.SendTime)
                .HasColumnName("SendTime")
                .IsRequired();

            //关系
            this.HasRequired(m => m.Notification)
                .WithMany(m => m.SendVisitorNotifications)
                .HasForeignKey(m => m.MsgId);
        }
    }
}
