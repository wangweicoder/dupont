using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_FARM_BOOKINGMap : EntityTypeConfiguration<T_FARM_BOOKING>
    {
        public T_FARM_BOOKINGMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("T_FARM_BOOKING");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FarmId).HasColumnName("FarmId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.VisitDate).HasColumnName("VisitDate");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");

            // Relationships
            this.HasRequired(t => t.T_DEMONSTRATION_FARM)
                .WithMany(t => t.T_FARM_BOOKING)
                .HasForeignKey(d => d.FarmId);

            this.HasRequired(t => t.ReservedUser)
                .WithMany(t => t.BookFarmList)
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(false);

        }
    }
}
