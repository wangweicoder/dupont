namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_USER_FARMERDEMANDS",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        FarmerDemandId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.FarmerDemandId })
                .ForeignKey("dbo.T_USER", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.T_FARMER_PUBLISHED_DEMAND", t => t.FarmerDemandId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FarmerDemandId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_USER_FARMERDEMANDS", "FarmerDemandId", "dbo.T_FARMER_PUBLISHED_DEMAND");
            DropForeignKey("dbo.T_USER_FARMERDEMANDS", "UserId", "dbo.T_USER");
            DropIndex("dbo.T_USER_FARMERDEMANDS", new[] { "FarmerDemandId" });
            DropIndex("dbo.T_USER_FARMERDEMANDS", new[] { "UserId" });
            DropTable("dbo.T_USER_FARMERDEMANDS");
        }
    }
}
