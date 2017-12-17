namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v28 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "Comments", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_FARMER_DEMAND_RESPONSE_RELATION", "Comments", c => c.String(maxLength: 500, unicode: false));
        }
    }
}
