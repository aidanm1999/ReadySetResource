namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRequiredFromAcceptedAttributeInHoliday : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Holidays", "Accepted", c => c.String(nullable: true));
        }
        
        public override void Down()
        {

        }
    }
}
