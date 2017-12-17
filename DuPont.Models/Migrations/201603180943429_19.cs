namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19 : DbMigration
    {
        public override void Up()
        {
            Sql("alter table T_USER_ROLE_RELATION drop constraint DF_T_USER_ROLE_RELATION_Star");
            AddColumn("dbo.T_USER_ROLE_RELATION", "TotalStarCount", c => c.Long());
            AddColumn("dbo.T_USER_ROLE_RELATION", "TotalReplyCount", c => c.Long());
            AlterColumn("dbo.T_USER_ROLE_RELATION", "Star", c => c.Long());
            
        }
        
        public override void Down()
        {
            Sql("alter table T_USER_ROLE_RELATION add constraint DF_T_USER_ROLE_RELATION_Star default(0)");
            AlterColumn("dbo.T_USER_ROLE_RELATION", "Star", c => c.Byte());
            DropColumn("dbo.T_USER_ROLE_RELATION", "TotalReplyCount");
            DropColumn("dbo.T_USER_ROLE_RELATION", "TotalStarCount");
        }
    }
}
