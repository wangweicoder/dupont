namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_USER", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.T_USER", "PhoneNumber", c => c.String(maxLength: 11, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.T_USER", "PhoneNumber", c => c.String(nullable: false, maxLength: 11, unicode: false));
            DropColumn("dbo.T_USER", "Type");
        }
    }
}
