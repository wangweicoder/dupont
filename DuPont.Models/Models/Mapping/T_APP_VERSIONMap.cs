using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DuPont.Models.Models.Mapping
{
    public class T_APP_VERSIONMap : EntityTypeConfiguration<T_APP_VERSION>
    {
        public T_APP_VERSIONMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Version)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DownloadURL)
                .HasMaxLength(1000);

            this.Property(t => t.ChangeLog)
                .HasMaxLength(1000);

            this.Property(t => t.Platform)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("T_APP_VERSION");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.VersionCode).HasColumnName("VersionCode");
            this.Property(t => t.DownloadURL).HasColumnName("DownloadURL");
            this.Property(t => t.ChangeLog).HasColumnName("ChangeLog");
            this.Property(t => t.CREATE_USER).HasColumnName("CREATE_USER");
            this.Property(t => t.CREATE_DATE).HasColumnName("CREATE_DATE");
            this.Property(t => t.UPDATE_USER).HasColumnName("UPDATE_USER");
            this.Property(t => t.UPDATE_DATE).HasColumnName("UPDATE_DATE");
            this.Property(t => t.Platform).HasColumnName("Platform");
            this.Property(t => t.IsOpen).HasColumnName("IsOpen");
        }
    }
}
