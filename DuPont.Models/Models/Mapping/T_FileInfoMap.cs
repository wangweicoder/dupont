using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_FileInfoMap : EntityTypeConfiguration<T_FileInfo>
    {
        public T_FileInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Path)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("T_FileInfo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
