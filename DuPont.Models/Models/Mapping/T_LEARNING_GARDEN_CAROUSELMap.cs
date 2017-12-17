using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_LEARNING_GARDEN_CAROUSELMap : EntityTypeConfiguration<T_LEARNING_GARDEN_CAROUSEL>
    {
        public T_LEARNING_GARDEN_CAROUSELMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("T_LEARNING_GARDEN_CAROUSEL");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ArticleId).HasColumnName("ArticleId");
            this.Property(t => t.CatId).HasColumnName("CatId");

            // Relationships
            this.HasRequired(t => t.T_ARTICLE)
                .WithMany(t => t.T_LEARNING_GARDEN_CAROUSEL)
                .HasForeignKey(d => d.ArticleId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.T_ARTICLE_CATEGORY)
                .WithMany(t => t.T_LEARNING_GARDEN_CAROUSEL)
                .HasForeignKey(d => d.CatId)
                .WillCascadeOnDelete(false);

        }
    }
}
