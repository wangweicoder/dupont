using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_USER_PASSWORD_HISTORYMap : EntityTypeConfiguration<T_USER_PASSWORD_HISTORY>
    {
        public T_USER_PASSWORD_HISTORYMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserID,t.Password });

            // Properties
            this.Property(t => t.UserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Password)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("T_USER_PASSWORD_HISTORY");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
