using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_SUPPLIERS_AREAMap : EntityTypeConfiguration<T_SUPPLIERS_AREA>
    {
        public T_SUPPLIERS_AREAMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserID,t.AID });

            // Properties
            this.Property(t => t.UserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AID)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("T_SUPPLIERS_AREA");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.AID).HasColumnName("AID");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.CreateDateTime).HasColumnName("CreateDateTime");
        }
    }
}
