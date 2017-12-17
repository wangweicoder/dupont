namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v7 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_NOTIFICATION",
                c => new
                    {
                        MsgId = c.Long(nullable: false, identity: true),
                        IsPublic = c.Boolean(nullable: false),
                        MsgContent = c.String(nullable: false, maxLength: 250),
                        TargetUserId = c.Long(),
                        CreateTime = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsOnDate = c.Boolean(nullable: false),
                        SendOnDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MsgId)
                .ForeignKey("dbo.T_USER", t => t.TargetUserId)
                .Index(t => t.TargetUserId);
            
            CreateTable(
                "dbo.T_SEND_NOTIFICATION_RESULT",
                c => new
                    {
                        MsgId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.MsgId, t.UserId })
                .ForeignKey("dbo.T_NOTIFICATION", t => t.MsgId, cascadeDelete: true)
                .ForeignKey("dbo.T_USER", t => t.UserId, cascadeDelete: true)
                .Index(t => t.MsgId)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.T_SEND_COMMON_NOTIFICATION_PROGRESS",
                c => new
                    {
                        MsgId = c.Long(nullable: false),
                        LastMaxUserId = c.Long(nullable: false),
                        SendTotalCount = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.MsgId ,t.LastMaxUserId})
                .ForeignKey("dbo.T_NOTIFICATION", t => t.MsgId)
                .Index(t => t.MsgId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.T_NOTIFICATION", "TargetUserId", "dbo.T_USER");
            DropForeignKey("dbo.T_SEND_COMMON_NOTIFICATION_PROGRESS", "MsgId", "dbo.T_NOTIFICATION");
            DropForeignKey("dbo.T_SEND_NOTIFICATION_RESULT", "UserId", "dbo.T_USER");
            DropForeignKey("dbo.T_SEND_NOTIFICATION_RESULT", "MsgId", "dbo.T_NOTIFICATION");
            DropIndex("dbo.T_SEND_COMMON_NOTIFICATION_PROGRESS", new[] { "MsgId" });
            DropIndex("dbo.T_SEND_NOTIFICATION_RESULT", new[] { "UserId" });
            DropIndex("dbo.T_SEND_NOTIFICATION_RESULT", new[] { "MsgId" });
            DropIndex("dbo.T_NOTIFICATION", new[] { "TargetUserId" });
            DropTable("dbo.T_SEND_COMMON_NOTIFICATION_PROGRESS");
            DropTable("dbo.T_SEND_NOTIFICATION_RESULT");
            DropTable("dbo.T_NOTIFICATION");
        }
    }
}
