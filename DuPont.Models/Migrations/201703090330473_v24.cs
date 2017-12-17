namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_NOTIFICATION", "NotificationSourceId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_NOTIFICATION", "NotificationSourceId");
        }
    }
}
