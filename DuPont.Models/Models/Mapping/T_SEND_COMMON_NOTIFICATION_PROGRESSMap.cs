using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models.Mapping
{
    public class T_SEND_COMMON_NOTIFICATION_PROGRESSMap : EntityTypeConfiguration<T_SEND_COMMON_NOTIFICATION_PROGRESS>
    {
        public T_SEND_COMMON_NOTIFICATION_PROGRESSMap()
        {
            this.Property(m => m.SendTotalCount).IsRequired();
            this.Property(m => m.CreateTaskTime)
                .HasColumnName("CreateTaskTime")
                .IsRequired();

            this.HasRequired(m => m.Notification)
                .WithOptional(m => m.SendPublicNotificationProgressInfo);

            this.HasKey(m => new { m.MsgId });
        }
    }
}
