using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_ARTICLE_CATEGORYMap : EntityTypeConfiguration<T_ARTICLE_CATEGORY>
    {
        public T_ARTICLE_CATEGORYMap()
        {
            // Primary Key
            this.HasKey(t => t.CategoryId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("T_ARTICLE_CATEGORY");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
