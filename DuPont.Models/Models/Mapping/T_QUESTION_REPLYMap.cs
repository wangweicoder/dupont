using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_QUESTION_REPLYMap : EntityTypeConfiguration<T_QUESTION_REPLY>
    {
        public T_QUESTION_REPLYMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Content)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("T_QUESTION_REPLY");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.QuestionId).HasColumnName("QuestionId");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.LikeCount).HasColumnName("LikeCount");
            this.Property(t => t.IsAgree).HasColumnName("IsAgree");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.LastModifiedTime).HasColumnName("LastModifiedTime");
        }
    }
}
