namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationToHoliday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Holidays", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Holidays", "Location");
        }
    }
}
