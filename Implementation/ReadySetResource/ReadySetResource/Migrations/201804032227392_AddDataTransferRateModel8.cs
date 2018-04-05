namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataTransferRateModel8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataTransferRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.Int(nullable: false),
                        EndTime = c.Int(nullable: false),
                        StartPage = c.String(nullable: false),
                        EndPage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
        }
    }
}
