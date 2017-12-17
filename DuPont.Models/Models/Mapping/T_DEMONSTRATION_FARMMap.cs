using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_DEMONSTRATION_FARMMap : EntityTypeConfiguration<T_DEMONSTRATION_FARM>
    {
        public T_DEMONSTRATION_FARMMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ProvinceAid)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CityAid)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.RegionAid)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PlantArea)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Variety)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SowTime)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PlantPoint)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("T_DEMONSTRATION_FARM");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProvinceAid).HasColumnName("ProvinceAid");
            this.Property(t => t.CityAid).HasColumnName("CityAid");
            this.Property(t => t.RegionAid).HasColumnName("RegionAid");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.IsOpen).HasColumnName("IsOpen");
            this.Property(t => t.OpenStartDate).HasColumnName("OpenStartDate");
            this.Property(t => t.OpenEndDate).HasColumnName("OpenEndDate");
            this.Property(t => t.PlantArea).HasColumnName("PlantArea");
            this.Property(t => t.Variety).HasColumnName("Variety");
            this.Property(t => t.SowTime).HasColumnName("SowTime");
            this.Property(t => t.PlantPoint).HasColumnName("PlantPoint");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
        }
    }
}
