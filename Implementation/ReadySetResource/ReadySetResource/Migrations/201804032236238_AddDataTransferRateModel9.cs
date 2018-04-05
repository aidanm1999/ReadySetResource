namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataTransferRateModel9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DataTransferRates", "StartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.DataTransferRates", "EndTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DataTransferRates", "EndTime", c => c.Int(nullable: false));
            AlterColumn("dbo.DataTransferRates", "StartTime", c => c.Int(nullable: false));
        }
    }
}
