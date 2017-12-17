namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v14 : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.T_USER_FARMERDEMANDS");
            //AddPrimaryKey("dbo.T_USER_FARMERDEMANDS", new[] { "FarmerDemandId", "UserId" });
        }
        
        public override void Down()
        {
            //DropPrimaryKey("dbo.T_USER_FARMERDEMANDS");
            //AddPrimaryKey("dbo.T_USER_FARMERDEMANDS", new[] { "UserId", "FarmerDemandId" });
        }
    }
}
