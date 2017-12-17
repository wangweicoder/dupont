namespace DuPont.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v18 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.T_MACHINE_DEMANDTYPE_RELATION",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Int(nullable: false),
                        DemandTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.MachineId, t.DemandTypeId });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.T_MACHINE_DEMANDTYPE_RELATION");
        }
    }
}
