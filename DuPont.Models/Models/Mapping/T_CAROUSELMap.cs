using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_CAROUSELMap : EntityTypeConfiguration<T_CAROUSEL>
    {
        public T_CAROUSELMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("T_CAROUSEL");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FileId).HasColumnName("FileId");
            this.Property(t => t.Order).HasColumnName("Order");
            this.Property(t => t.IsDisplay).HasColumnName("IsDisplay");
            this.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.EditUserId).HasColumnName("EditUserId");
            this.Property(t => t.EditTime).HasColumnName("EditTime");
        }
    }
}
