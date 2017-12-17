namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v9 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.T_FARM_BOOKING", "UserId");
            AddForeignKey("dbo.T_FARM_BOOKING", "UserId", "dbo.T_USER", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_FARM_BOOKING", "UserId", "dbo.T_USER");
            DropIndex("dbo.T_FARM_BOOKING", new[] { "UserId" });
        }
    }
}
