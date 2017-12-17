using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_SYS_LOGMap : EntityTypeConfiguration<T_SYS_LOG>
    {
        public T_SYS_LOGMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Level)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StackTrace)
                .IsRequired()
                .HasMaxLength(3000);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Url)
                .HasColumnType("varchar")
                .HasMaxLength(500);

            this.Property(t => t.RequestParameter)
                .HasColumnType("varchar")
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("T_SYS_LOG");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.StackTrace).HasColumnName("StackTrace");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.RequestParameter).HasColumnName("RequestParameter");
        }
    }
}
