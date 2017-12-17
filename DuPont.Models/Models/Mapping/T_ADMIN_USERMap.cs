using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_ADMIN_USERMap : EntityTypeConfiguration<T_ADMIN_USER>
    {
        public T_ADMIN_USERMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RealName)
                .HasMaxLength(50);

            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.LoginToken)
                .HasColumnType("varchar")
                .HasMaxLength(200);

            this.Property(t => t.AvartarUrl)
                .HasColumnType("varchar")
                .HasMaxLength(300);

            this.Property(t => t.Province)
                .HasMaxLength(50);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.Region)
                .HasMaxLength(50);

            this.Property(t => t.Township)
                .HasMaxLength(50);

            this.Property(t => t.Village)
                .HasMaxLength(50);

            this.Property(t => t.DetailedAddress)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("T_ADMIN_USER");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.IsSuperAdmin).HasColumnName("IsSuperAdmin");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.LoginToken).HasColumnName("LoginToken");
            this.Property(t => t.AvartarUrl).HasColumnName("AvartarUrl");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.Township).HasColumnName("Township");
            this.Property(t => t.Village).HasColumnName("Village");
            this.Property(t => t.DetailedAddress).HasColumnName("DetailedAddress");
            this.Property(t => t.IsLock).HasColumnName("IsLock");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
