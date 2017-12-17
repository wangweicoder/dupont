using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_PIONEERCURRENCYHISTORYMap : EntityTypeConfiguration<T_PIONEERCURRENCYHISTORY>
    {
        public T_PIONEERCURRENCYHISTORYMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("T_PIONEERCURRENCYHISTORY");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.DPoint).HasColumnName("DPoint");
            this.Property(t => t.AuditUserId).HasColumnName("AuditUserId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
