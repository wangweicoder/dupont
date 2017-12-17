namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_USER", "SourceType", c => c.Int(nullable: false));
            AddColumn("dbo.T_USER", "WeatherCity", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_USER", "WeatherCity");
            DropColumn("dbo.T_USER", "SourceType");
        }
    }
}
