using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_USER_ROLE_DEMANDTYPELEVEL_RELATIONMap : EntityTypeConfiguration<T_USER_ROLE_DEMANDTYPELEVEL_RELATION>
    {
        public T_USER_ROLE_DEMANDTYPELEVEL_RELATIONMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.RoleId, t.DemandId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DemandId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("T_USER_ROLE_DEMANDTYPELEVEL_RELATION");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.DemandId).HasColumnName("DemandId");
            this.Property(t => t.Star).HasColumnName("Star");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
