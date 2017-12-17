using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Models.Models.Mapping
{
   public class T_User_TokenMap:EntityTypeConfiguration<T_User_Token>
    {
       public T_User_TokenMap()
       {
           // Primary Key
           this.HasKey(t => t.Id);

           // Properties   
           this.Property(t => t.Token)
               .HasColumnType("varchar")
               .HasMaxLength(200);

           this.Property(t => t.Password)
               .HasColumnType("varchar")
               .IsRequired()
               .HasMaxLength(100);

           this.Property(t => t.UserName)
               .HasColumnType("nvarchar")
               .HasMaxLength(200);

           // Table & Column Mappings
           this.ToTable("T_User_Token");
           this.Property(t => t.Id).HasColumnName("Id");       
           this.Property(t => t.Token).HasColumnName("Token");        
           this.Property(t => t.UserName).HasColumnName("UserName");
           this.Property(t => t.Password).HasColumnName("Password");
           this.Property(t => t.CreateTime).HasColumnName("CreateTime");          
           this.Property(t => t.DeletedTime).HasColumnName("DeletedTime");
           this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
           this.Property(t => t.ModifiedTime).HasColumnName("ModifiedTime");         
           this.Property(t => t.LastLoginTime).HasColumnName("LastLoginTime");
           this.Property(t => t.UserType).HasColumnName("UserType");
       }

    }
}
