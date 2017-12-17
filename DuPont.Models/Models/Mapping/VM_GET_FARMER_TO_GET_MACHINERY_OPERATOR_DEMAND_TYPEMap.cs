using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPEMap : EntityTypeConfiguration<VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPE>
    {
        public VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPEMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Code, t.ParentCode, t.DisplayName, t.Order });

            // Properties
            this.Property(t => t.Code)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ParentCode)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DisplayName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Order)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPE");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ParentCode).HasColumnName("ParentCode");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.Order).HasColumnName("Order");
        }
    }
}
