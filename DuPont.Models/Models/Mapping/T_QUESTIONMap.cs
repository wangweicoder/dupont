using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_QUESTIONMap : EntityTypeConfiguration<T_QUESTION>
    {
        public T_QUESTIONMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.PictureIds)
                .HasMaxLength(100);

            this.Property(t => t.QuestionType)
                .HasColumnType("varchar")
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("T_QUESTION");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.IsOpen).HasColumnName("IsOpen");
            this.Property(t => t.ReplyCount).HasColumnName("ReplyCount");
            this.Property(t => t.PictureIds).HasColumnName("PictureIds");
            this.Property(t => t.QuestionType).HasColumnName("QuestionType");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.LastModifiedTime).HasColumnName("LastModifiedTime");
            this.Property(t => t.IsPutOnCarousel).HasColumnName("IsPutOnCarousel");
            
            
        }
    }
}
