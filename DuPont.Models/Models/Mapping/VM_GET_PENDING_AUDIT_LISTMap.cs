using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class VM_GET_PENDING_AUDIT_LISTMap : EntityTypeConfiguration<VM_GET_PENDING_AUDIT_LIST>
    {
        public VM_GET_PENDING_AUDIT_LISTMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.UserId, t.RoleId, t.RoleName, t.CreateTime, t.AuditState });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.PhoneNumber)
                .HasMaxLength(11);

            this.Property(t => t.Province)
                .HasMaxLength(50);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.Region)
                .HasMaxLength(50);

            this.Property(t => t.AuditState)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VM_GET_PENDING_AUDIT_LIST");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.AuditState).HasColumnName("AuditState");
        }
    }
}
