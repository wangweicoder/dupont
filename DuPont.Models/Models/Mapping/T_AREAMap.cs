using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_AREAMap : EntityTypeConfiguration<T_AREA>
    {
        public T_AREAMap()
        {
            // Primary Key
            this.HasKey(t => t.AID);

            // Properties
            this.Property(t => t.AID)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.DisplayName)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.ParentAID)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Lng)
                .HasColumnType("varchar")
                .HasMaxLength(200);

            this.Property(t => t.Lat)
                .HasColumnType("varchar")
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("T_AREA");
            this.Property(t => t.AID).HasColumnName("AID");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.ParentAID).HasColumnName("ParentAID");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.Lng).HasColumnName("Lng");
            this.Property(t => t.Lat).HasColumnName("Lat");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
