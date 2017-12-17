namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_VISITOR_RECEIVED_NOTIFICATION",
                c => new
                    {
                        MsgId = c.Long(nullable: false),
                        DeviceToken = c.String(nullable: false, maxLength: 100, unicode: false),
                        OsType = c.String(nullable: false, maxLength: 15, unicode: false),
                        SendTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.MsgId, t.DeviceToken })
                .ForeignKey("dbo.T_NOTIFICATION", t => t.MsgId, cascadeDelete: true)
                .Index(t => t.MsgId);
            
            AddColumn("dbo.T_USER", "IosDeviceToken", c => c.String(maxLength: 100));
            AddColumn("dbo.T_SEND_NOTIFICATION_RESULT", "SendTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.T_SEND_COMMON_NOTIFICATION_PROGRESS", "CreateTaskTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_VISITOR_RECEIVED_NOTIFICATION", "MsgId", "dbo.T_NOTIFICATION");
            DropIndex("dbo.T_VISITOR_RECEIVED_NOTIFICATION", new[] { "MsgId" });
            DropColumn("dbo.T_SEND_COMMON_NOTIFICATION_PROGRESS", "CreateTaskTime");
            DropColumn("dbo.T_SEND_NOTIFICATION_RESULT", "SendTime");
            DropColumn("dbo.T_USER", "IosDeviceToken");
            DropTable("dbo.T_VISITOR_RECEIVED_NOTIFICATION");
        }
    }
}
