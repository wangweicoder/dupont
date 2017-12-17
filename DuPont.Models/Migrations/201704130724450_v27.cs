namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_User_Token",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(maxLength: 200),
                        Password = c.String(nullable: false, maxLength: 100, unicode: false),
                        Token = c.String(maxLength: 200, unicode: false),
                        CreateTime = c.DateTime(),
                        DeletedTime = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        ModifiedTime = c.DateTime(),
                        LastLoginTime = c.DateTime(),
                        UserType = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.T_User_Token");
        }
    }
}
