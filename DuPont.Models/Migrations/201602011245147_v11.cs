namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("T_USER", "UserType", c => c.Int());
            AddColumn("T_USER", "OpenId", c => c.String(maxLength: 500));
            AddColumn("T_USER", "UnionId", c => c.String(maxLength: 500));
            AddColumn("T_USER", "NickName", c => c.String(maxLength:500));
            //CreateTable(
            //    "dbo.T_USER",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            PhoneNumber = c.String(nullable: false, maxLength: 11, unicode: false),
            //            Password = c.String(nullable: false, maxLength: 100, unicode: false),
            //            LoginToken = c.String(maxLength: 200, unicode: false),
            //            AvartarUrl = c.String(maxLength: 300, unicode: false),
            //            UserName = c.String(maxLength: 50),
            //            Province = c.String(maxLength: 50),
            //            City = c.String(maxLength: 50),
            //            DPoint = c.Int(),
            //            Region = c.String(maxLength: 50),
            //            Township = c.String(maxLength: 50),
            //            Village = c.String(maxLength: 50),
            //            DetailedAddress = c.String(maxLength: 200),
            //            CreateTime = c.DateTime(nullable: false),
            //            ModifiedUserId = c.Long(),
            //            DeletedUserId = c.Long(),
            //            DeletedTime = c.DateTime(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            ModifiedTime = c.DateTime(),
            //            SmsCode = c.String(maxLength: 200, unicode: false),
            //            LoginUserName = c.String(maxLength: 50),
            //            LastLoginTime = c.DateTime(),
            //            LastUpdatePwdTime = c.DateTime(),
            //            IosDeviceToken = c.String(maxLength: 100),
            //            UserType = c.Int(),
            //            NickName = c.String(maxLength: 500),
            //            OpenId = c.Long(),
            //            UnionId = c.Long(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //DropTable("dbo.T_USER");
        }
        
        public override void Down()
        {
            DropColumn("T_USER", "UserType");
            DropColumn("T_USER", "OpenId");
            DropColumn("T_USER", "UnionId");
            DropColumn("T_USER", "NickName");
            //CreateTable(
            //    "dbo.T_USER",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            PhoneNumber = c.String(nullable: false, maxLength: 11, unicode: false),
            //            Password = c.String(nullable: false, maxLength: 100, unicode: false),
            //            LoginToken = c.String(maxLength: 200, unicode: false),
            //            AvartarUrl = c.String(maxLength: 300, unicode: false),
            //            UserName = c.String(maxLength: 50),
            //            Province = c.String(maxLength: 50),
            //            City = c.String(maxLength: 50),
            //            DPoint = c.Int(),
            //            Region = c.String(maxLength: 50),
            //            Township = c.String(maxLength: 50),
            //            Village = c.String(maxLength: 50),
            //            DetailedAddress = c.String(maxLength: 200),
            //            CreateTime = c.DateTime(nullable: false),
            //            ModifiedUserId = c.Long(),
            //            DeletedUserId = c.Long(),
            //            DeletedTime = c.DateTime(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            ModifiedTime = c.DateTime(),
            //            SmsCode = c.String(maxLength: 200, unicode: false),
            //            LoginUserName = c.String(maxLength: 50),
            //            LastLoginTime = c.DateTime(),
            //            LastUpdatePwdTime = c.DateTime(),
            //            IosDeviceToken = c.String(maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //DropTable("dbo.T_USER");
        }
    }
}
