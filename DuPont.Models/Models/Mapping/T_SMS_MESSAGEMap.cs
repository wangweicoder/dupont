using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_SMS_MESSAGEMap : EntityTypeConfiguration<T_SMS_MESSAGE>
    {
        public T_SMS_MESSAGEMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhoneNumber)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(11);

            this.Property(t => t.Captcha)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("T_SMS_MESSAGE");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Captcha).HasColumnName("Captcha");
            this.Property(t => t.SendTime).HasColumnName("SendTime");
        }
    }
}
