using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_SYS_ADMINMap : EntityTypeConfiguration<T_SYS_ADMIN>
    {
        public T_SYS_ADMINMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IsSuper)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("T_SYS_ADMIN");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsSuper).HasColumnName("IsSuper");
            this.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedTime).HasColumnName("ModifiedTime");
        }
    }
}
