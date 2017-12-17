using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_FARMER_DEMAND_RESPONSE_RELATIONMap : EntityTypeConfiguration<T_FARMER_DEMAND_RESPONSE_RELATION>
    {
        public T_FARMER_DEMAND_RESPONSE_RELATIONMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Comments)
                .HasColumnType("nvarchar")
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("T_FARMER_DEMAND_RESPONSE_RELATION");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BonusDPoint).HasColumnName("BonusDPoint");
            this.Property(t => t.DemandId).HasColumnName("DemandId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.ReplyTime).HasColumnName("ReplyTime");
            this.Property(t => t.Score).HasColumnName("Score");
        }
    }
}
