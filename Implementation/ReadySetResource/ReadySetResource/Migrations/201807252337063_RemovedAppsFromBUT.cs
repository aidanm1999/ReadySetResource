namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedAppsFromBUT : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.BusinessUserTypes", "Administrator");
            DropColumn("dbo.BusinessUserTypes", "Updates");
            DropColumn("dbo.BusinessUserTypes", "Store");
            DropColumn("dbo.BusinessUserTypes", "Messenger");
            DropColumn("dbo.BusinessUserTypes", "Meetings");
            DropColumn("dbo.BusinessUserTypes", "Calendar");
            DropColumn("dbo.BusinessUserTypes", "Holidays");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BusinessUserTypes", "Holidays", c => c.String());
            AddColumn("dbo.BusinessUserTypes", "Calendar", c => c.String());
            AddColumn("dbo.BusinessUserTypes", "Meetings", c => c.String());
            AddColumn("dbo.BusinessUserTypes", "Messenger", c => c.String());
            AddColumn("dbo.BusinessUserTypes", "Store", c => c.String());
            AddColumn("dbo.BusinessUserTypes", "Updates", c => c.String());
            AddColumn("dbo.BusinessUserTypes", "Administrator", c => c.String());
        }
    }
}
