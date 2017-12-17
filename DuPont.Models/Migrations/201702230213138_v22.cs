namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "CommentsFarmer", c => c.String());
            AddColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "ReplyTimeFarmer", c => c.DateTime(nullable: false));
            AddColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "ScoreFarmer", c => c.Int(nullable: false));
            AddColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "SourceType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "SourceType");
            DropColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "ScoreFarmer");
            DropColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "ReplyTimeFarmer");
            DropColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "CommentsFarmer");
        }
    }
}
