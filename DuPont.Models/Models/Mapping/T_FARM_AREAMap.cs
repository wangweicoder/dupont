using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_FARM_AREAMap : EntityTypeConfiguration<T_FARM_AREA>
    {
        public T_FARM_AREAMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Url)
                .HasColumnType("varchar")
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("T_FARM_AREA");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FarmId).HasColumnName("FarmId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.IsFarmMachinery).HasColumnName("IsFarmMachinery");

            // Relationships
            this.HasRequired(t => t.T_DEMONSTRATION_FARM)
                .WithMany(t => t.T_FARM_AREA)
                .HasForeignKey(d => d.FarmId);

        }
    }
}
