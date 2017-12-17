using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_BUSINESS_PUBLISHED_DEMANDMap : EntityTypeConfiguration<T_BUSINESS_PUBLISHED_DEMAND>
    {
        public T_BUSINESS_PUBLISHED_DEMANDMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(11);

            this.Property(t => t.ExpectedDate)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.TimeSlot)
                .HasColumnType("varchar")
                .HasMaxLength(3);

            this.Property(t => t.Brief)
                .HasMaxLength(300);

            this.Property(t => t.Province)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Region)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Township)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Village)
                .HasMaxLength(50);

            this.Property(t => t.DetailedAddress)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("T_BUSINESS_PUBLISHED_DEMAND");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DemandTypeId).HasColumnName("DemandTypeId");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.CropId).HasColumnName("CropId");
            this.Property(t => t.PublishStateId).HasColumnName("PublishStateId");
            this.Property(t => t.ExpectedDate).HasColumnName("ExpectedDate");
            this.Property(t => t.TimeSlot).HasColumnName("TimeSlot");
            this.Property(t => t.ExpectedStartPrice).HasColumnName("ExpectedStartPrice");
            this.Property(t => t.ExpectedEndPrice).HasColumnName("ExpectedEndPrice");
            this.Property(t => t.AcquisitionWeightRangeTypeId).HasColumnName("AcquisitionWeightRangeTypeId");
            this.Property(t => t.FirstWeight).HasColumnName("FirstWeight");
            this.Property(t => t.Brief).HasColumnName("Brief");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.Township).HasColumnName("Township");
            this.Property(t => t.Village).HasColumnName("Village");
            this.Property(t => t.DetailedAddress).HasColumnName("DetailedAddress");
            this.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.DeletedUserId).HasColumnName("DeletedUserId");
            this.Property(t => t.DeletedTime).HasColumnName("DeletedTime");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.ModifiedTime).HasColumnName("ModifiedTime");
        }
    }
}
