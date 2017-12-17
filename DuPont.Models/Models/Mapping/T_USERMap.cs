using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_USERMap : EntityTypeConfiguration<T_USER>
    {
        public T_USERMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhoneNumber)
                .HasColumnType("varchar")
                .IsOptional()
                .HasMaxLength(11);

            this.Property(t => t.Password)
                .HasColumnType("varchar")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.LoginToken)
                .HasColumnType("varchar")
                .HasMaxLength(200);

            this.Property(t => t.AvartarUrl)
                .HasColumnType("varchar")
                .HasMaxLength(300);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Province)
                .HasMaxLength(50);

            this.Property(t => t.City)
                .HasMaxLength(50);

            this.Property(t => t.Region)
                .HasMaxLength(50);

            this.Property(t => t.Township)
                .HasMaxLength(50);

            this.Property(t => t.Village)
                .HasMaxLength(50);

            this.Property(t => t.DetailedAddress)
                .HasMaxLength(200);

            this.Property(t => t.SmsCode)
                .HasColumnType("varchar")
                .HasMaxLength(200);

            this.Property(t => t.LoginUserName)
                .HasMaxLength(50);

            this.Property(t => t.NickName)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("T_USER");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.LoginToken).HasColumnName("LoginToken");
            this.Property(t => t.AvartarUrl).HasColumnName("AvartarUrl");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.DPoint).HasColumnName("DPoint");
            this.Property(t => t.Region).HasColumnName("Region");
            this.Property(t => t.Township).HasColumnName("Township");
            this.Property(t => t.Village).HasColumnName("Village");
            this.Property(t => t.DetailedAddress).HasColumnName("DetailedAddress");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.DeletedUserId).HasColumnName("DeletedUserId");
            this.Property(t => t.DeletedTime).HasColumnName("DeletedTime");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.ModifiedTime).HasColumnName("ModifiedTime");
            this.Property(t => t.SmsCode).HasColumnName("SmsCode");
            this.Property(t => t.LoginUserName).HasColumnName("LoginUserName");
            this.Property(t => t.IosDeviceToken).HasMaxLength(100).HasColumnName("IosDeviceToken");
            this.Property(t => t.NickName).HasColumnName("NickName");
            //this.Property(t => t.UserType).HasColumnName("UserType");

            this.HasMany(t => t.Questions)
                .WithRequired(t => t.User)
                .HasForeignKey(m => m.UserId);

            //this.HasMany(u => u.FarmerDemands).WithMany(d => d.Operators)
            //    .Map(m => {
            //    m.ToTable("T_USER_FARMERDEMANDS");
            //    m.MapLeftKey("UserId");
            //    m.MapRightKey("FarmerDemandId");
            //});

        }
    }
}
