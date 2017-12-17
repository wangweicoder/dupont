using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_ARTICLEMap : EntityTypeConfiguration<T_ARTICLE>
    {
        public T_ARTICLEMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(300);

            this.Property(t => t.Content)
                .IsRequired();

            this.Property(t => t.Url)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("T_ARTICLE");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Click).HasColumnName("Click");
            this.Property(t => t.CatId).HasColumnName("CatId");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
            this.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.IsPutOnCarousel).HasColumnName("IsPutOnCarousel");

            // Relationships
            this.HasRequired(t => t.T_ARTICLE_CATEGORY)
                .WithMany(t => t.T_ARTICLE)
                .HasForeignKey(d => d.CatId);

        }
    }
}
