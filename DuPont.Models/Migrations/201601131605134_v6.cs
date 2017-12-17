namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VM_GET_FARMER_TO_BUSINESS_DEMAND_TYPE",
                c => new
                    {
                        Code = c.Int(nullable: false),
                        ParentCode = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Code, t.ParentCode, t.DisplayName, t.Order });
            
            CreateTable(
                "dbo.VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPE",
                c => new
                    {
                        Code = c.Int(nullable: false),
                        ParentCode = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Code, t.ParentCode, t.DisplayName, t.Order });
            
            CreateTable(
                "dbo.VM_GET_LARGE_FARMER_DEMAND_TYPE",
                c => new
                    {
                        Code = c.Int(nullable: false),
                        DisplayName = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Code, t.DisplayName, t.Order });
            
            CreateTable(
                "dbo.VM_GET_PENDING_AUDIT_LIST",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Int(nullable: false),
                        RoleName = c.String(nullable: false, maxLength: 6),
                        CreateTime = c.DateTime(nullable: false),
                        AuditState = c.Int(nullable: false),
                        UserName = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 11),
                        Province = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        Region = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => new { t.Id, t.UserId, t.RoleId, t.RoleName, t.CreateTime, t.AuditState });
            
            CreateTable(
                "dbo.VM_GET_USER_ROLE_INFO_LIST",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 11),
                        CreateTime = c.DateTime(nullable: false),
                        Province = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        Region = c.String(maxLength: 50),
                        Township = c.String(maxLength: 50),
                        Village = c.String(maxLength: 50),
                        UserName = c.String(maxLength: 50),
                        RoleID = c.Int(),
                    })
                .PrimaryKey(t => new { t.Id, t.PhoneNumber, t.CreateTime });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VM_GET_USER_ROLE_INFO_LIST");
            DropTable("dbo.VM_GET_PENDING_AUDIT_LIST");
            DropTable("dbo.VM_GET_LARGE_FARMER_DEMAND_TYPE");
            DropTable("dbo.VM_GET_FARMER_TO_GET_MACHINERY_OPERATOR_DEMAND_TYPE");
            DropTable("dbo.VM_GET_FARMER_TO_BUSINESS_DEMAND_TYPE");
        }
    }
}
