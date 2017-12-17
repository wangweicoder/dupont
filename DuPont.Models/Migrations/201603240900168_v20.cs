namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v20 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.VM_GET_USER_ROLE_INFO_LIST", "Type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.VM_GET_USER_ROLE_INFO_LIST", "Type");
        }
    }
}
