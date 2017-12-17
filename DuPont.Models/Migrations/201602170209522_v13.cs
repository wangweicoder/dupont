namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_FARMER_PUBLISHED_DEMAND", "IsOpen", c => c.Boolean(nullable: false, defaultValue:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_FARMER_PUBLISHED_DEMAND", "IsOpen");
        }
    }
}
