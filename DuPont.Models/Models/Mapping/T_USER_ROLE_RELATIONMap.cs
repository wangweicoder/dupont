using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_USER_ROLE_RELATIONMap : EntityTypeConfiguration<T_USER_ROLE_RELATION>
    {
        public T_USER_ROLE_RELATIONMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserID, t.RoleID, t.MemberType });

            // Properties
            this.Property(t => t.UserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RoleID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("T_USER_ROLE_RELATION");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.MemberType).HasColumnName("MemberType");
            this.Property(t => t.Star).HasColumnName("Star");
            this.Property(t => t.AuditUserId).HasColumnName("AuditUserId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
