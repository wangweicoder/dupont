using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_BUSINESS_DEMAND_RESPONSE_RELATIONMap : EntityTypeConfiguration<T_BUSINESS_DEMAND_RESPONSE_RELATION>
    {
        public T_BUSINESS_DEMAND_RESPONSE_RELATIONMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Comments)
                .HasColumnType("varchar")
                .HasMaxLength(500);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.PhoneNumber)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(11);

            this.Property(t => t.Brief)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("T_BUSINESS_DEMAND_RESPONSE_RELATION");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.BonusDPoint).HasColumnName("BonusDPoint");
            this.Property(t => t.DemandId).HasColumnName("DemandId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.ReplyTime).HasColumnName("ReplyTime");
            this.Property(t => t.Score).HasColumnName("Score");
            this.Property(t => t.WeightRangeTypeId).HasColumnName("WeightRangeTypeId");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Brief).HasColumnName("Brief");
        }
    }
}
