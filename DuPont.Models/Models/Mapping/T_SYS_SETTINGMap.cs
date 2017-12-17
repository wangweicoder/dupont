using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_SYS_SETTINGMap : EntityTypeConfiguration<T_SYS_SETTING>
    {
        public T_SYS_SETTINGMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.SETTING_ID)
                .IsRequired()
                .HasMaxLength(3);

            this.Property(t => t.SETTING_VALUE)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.COMMENT)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("T_SYS_SETTING");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SETTING_ID).HasColumnName("SETTING_ID");
            this.Property(t => t.SETTING_VALUE).HasColumnName("SETTING_VALUE");
            this.Property(t => t.COMMENT).HasColumnName("COMMENT");
            this.Property(t => t.CREATE_USER).HasColumnName("CREATE_USER");
            this.Property(t => t.CREATE_DATE).HasColumnName("CREATE_DATE");
            this.Property(t => t.UPDATE_USER).HasColumnName("UPDATE_USER");
            this.Property(t => t.UPDATE_DATE).HasColumnName("UPDATE_DATE");
        }
    }
}
