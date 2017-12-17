using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_SYS_DICTIONARYMap : EntityTypeConfiguration<T_SYS_DICTIONARY>
    {
        public T_SYS_DICTIONARYMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Properties
            this.Property(t => t.Code)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DisplayName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("T_SYS_DICTIONARY");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ParentCode).HasColumnName("ParentCode");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.Order).HasColumnName("Order");
        }
    }
}
