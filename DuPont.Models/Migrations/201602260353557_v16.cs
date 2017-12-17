namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v16 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.VM_GET_USER_ROLE_INFO_LIST", "Land", c => c.Int());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.VM_GET_USER_ROLE_INFO_LIST", "Land");
        }
    }
}
