namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_FARMER_VERIFICATION_INFO", "LandAuditState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_FARMER_VERIFICATION_INFO", "LandAuditState");
        }
    }
}
