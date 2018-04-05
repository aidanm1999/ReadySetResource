namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataOverTimeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataOverTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        MemoryMB = c.Single(nullable: false),
                        BusinessId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.BusinessId, cascadeDelete: true)
                .Index(t => t.BusinessId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DataOverTimes", "BusinessId", "dbo.Businesses");
            DropIndex("dbo.DataOverTimes", new[] { "BusinessId" });
            DropTable("dbo.DataOverTimes");
        }
    }
}
