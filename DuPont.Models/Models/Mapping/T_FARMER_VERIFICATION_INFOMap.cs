using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_FARMER_VERIFICATION_INFOMap : EntityTypeConfiguration<T_FARMER_VERIFICATION_INFO>
    {
        public T_FARMER_VERIFICATION_INFOMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RealName)
                .HasMaxLength(50);

            this.Property(t => t.DupontOrderNumbers)
                .HasColumnType("varchar")
                .HasMaxLength(2000);

            this.Property(t => t.PurchasedProducts)
                .HasColumnType("varchar")
                .HasMaxLength(1000);

            this.Property(t => t.RejectReason)
                .HasMaxLength(500);

            this.Property(t => t.Province)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            this.Property(t => t.City)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            this.Property(t => t.Region)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            this.Property(t => t.Township)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            this.Property(t => t.Village)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            this.Property(t => t.DetailAddress)
                .HasMaxLength(200);

            this.Property(t => t.PhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(11);

            // Table & Column Mappings
            this.ToTable("T_FARMER_VERIFICATION_INFO");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.DupontOrderNumbers).HasColumnName("DupontOrderNumbers");
            this.Property(t => t.PurchasedProducts).HasColumnName("PurchasedProducts");
            this.Property(t => t.Land).HasColumnName("Land");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.AuditUserId).HasColumnName("AuditUserId");
            this.Property(t => t.AuditTime).HasColumnName("AuditTime");
            this.Property(t => t.AuditState).HasColumnName("AuditState");
            this.Property(t => t.RejectReason).HasColumnName("RejectReason");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.Township).HasColumnName("Township");
            this.Property(t => t.Village).HasColumnName("Village");
            this.Property(t => t.DetailAddress).HasColumnName("DetailAddress");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
        }
    }
}
