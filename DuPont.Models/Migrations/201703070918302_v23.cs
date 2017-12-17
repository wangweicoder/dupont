namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_NOTIFICATION", "NotificationType", c => c.Int());
            AddColumn("dbo.T_NOTIFICATION", "NotificationSource", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_NOTIFICATION", "NotificationSource");
            DropColumn("dbo.T_NOTIFICATION", "NotificationType");
        }
    }
}
