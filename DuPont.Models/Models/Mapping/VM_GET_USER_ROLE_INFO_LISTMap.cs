using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class VM_GET_USER_ROLE_INFO_LISTMap : EntityTypeConfiguration<VM_GET_USER_ROLE_INFO_LIST>
    {
        public VM_GET_USER_ROLE_INFO_LISTMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.PhoneNumber, t.CreateTime });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11);

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

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VM_GET_USER_ROLE_INFO_LIST");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.Township).HasColumnName("Township");
            this.Property(t => t.Village).HasColumnName("Village");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
        }
    }
}
