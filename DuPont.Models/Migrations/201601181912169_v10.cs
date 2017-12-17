namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.T_USER", "LastLoginTime", c => c.DateTime());
            AddColumn("dbo.T_USER", "LastUpdatePwdTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.T_USER", "LastUpdatePwdTime");
            DropColumn("dbo.T_USER", "LastLoginTime");
        }
    }
}
