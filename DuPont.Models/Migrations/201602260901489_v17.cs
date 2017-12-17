namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v17 : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.VM_GET_USER_ROLE_INFO_LIST");
            //AddColumn("dbo.VM_GET_USER_ROLE_INFO_LIST", "UserId", c => c.Long(nullable: false));
            //AlterColumn("dbo.VM_GET_USER_ROLE_INFO_LIST", "Id", c => c.Guid(nullable: false));
            //AddPrimaryKey("dbo.VM_GET_USER_ROLE_INFO_LIST", new[] { "Id", "PhoneNumber", "CreateTime" });
        }
        
        public override void Down()
        {
            //DropPrimaryKey("dbo.VM_GET_USER_ROLE_INFO_LIST");
            //AlterColumn("dbo.VM_GET_USER_ROLE_INFO_LIST", "Id", c => c.Long(nullable: false));
            //DropColumn("dbo.VM_GET_USER_ROLE_INFO_LIST", "UserId");
            //AddPrimaryKey("dbo.VM_GET_USER_ROLE_INFO_LIST", new[] { "Id", "PhoneNumber", "CreateTime" });
        }
    }
}
