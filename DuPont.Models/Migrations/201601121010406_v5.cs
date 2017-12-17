namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.T_USER_PASSWORD_HISTORY");
            AddPrimaryKey("dbo.T_USER_PASSWORD_HISTORY", new[] { "UserID", "Password" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.T_USER_PASSWORD_HISTORY");
            AddPrimaryKey("dbo.T_USER_PASSWORD_HISTORY", "UserID");
        }
    }
}
