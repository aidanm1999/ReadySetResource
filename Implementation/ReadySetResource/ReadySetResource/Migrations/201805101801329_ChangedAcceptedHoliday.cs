namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedAcceptedHoliday : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Holidays", "Accepted", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Holidays", "Accepted", c => c.Boolean(nullable: false));
        }
    }
}
