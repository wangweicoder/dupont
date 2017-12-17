namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_NOTIFICATION", "IsOpen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_NOTIFICATION", "IsOpen");
        }
    }
}
