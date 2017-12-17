using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_MENU_ROLE_RELATIONMap : EntityTypeConfiguration<T_MENU_ROLE_RELATION>
    {
        public T_MENU_ROLE_RELATIONMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MenuId, t.RoleId });

            // Properties
            this.Property(t => t.MenuId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("T_MENU_ROLE_RELATION");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.AuditUserId).HasColumnName("AuditUserId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
