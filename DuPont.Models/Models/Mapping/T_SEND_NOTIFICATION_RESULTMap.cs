using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models.Mapping
{
    public class T_SEND_NOTIFICATION_RESULTMap : EntityTypeConfiguration<T_SEND_NOTIFICATION_RESULT>
    {
        public T_SEND_NOTIFICATION_RESULTMap()
        {
            //复合主键
            this.HasKey(t => new { t.MsgId, t.UserId });
            this.Property(t => t.SendTime)
                .HasColumnName("SendTime")
                .IsRequired();

            this.HasRequired(m => m.Notification)
                .WithMany(m => m.SendPrivateNotifications)
                .HasForeignKey(m => m.MsgId);

            this.HasRequired(m => m.TargetUser)
                .WithMany(m => m.ReceivedPrivateNotifications)
                .HasForeignKey(m => m.UserId);
        }
    }
}
