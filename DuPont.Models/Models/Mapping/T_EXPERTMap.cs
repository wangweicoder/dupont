using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_EXPERTMap : EntityTypeConfiguration<T_EXPERT>
    {
        public T_EXPERTMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("T_EXPERT");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IsEnabled).HasColumnName("IsEnabled");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.LastModifiedTime).HasColumnName("LastModifiedTime");
        }
    }
}
