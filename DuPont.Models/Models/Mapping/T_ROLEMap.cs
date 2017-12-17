using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_ROLEMap : EntityTypeConfiguration<T_ROLE>
    {
        public T_ROLEMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("T_ROLE");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RoleName).HasColumnName("RoleName");
        }
    }
}
