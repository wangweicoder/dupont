using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_MENUMap : EntityTypeConfiguration<T_MENU>
    {
        public T_MENUMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MenuName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Url)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("T_MENU");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuName).HasColumnName("MenuName");
            this.Property(t => t.Visible).HasColumnName("Visible");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Order).HasColumnName("Order");
        }
    }
}
