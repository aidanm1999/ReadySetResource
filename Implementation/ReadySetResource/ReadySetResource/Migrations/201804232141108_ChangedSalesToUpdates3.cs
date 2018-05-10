namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedSalesToUpdates3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessUserTypes", "Administrator", c => c.String(nullable: false));
            AddColumn("dbo.BusinessUserTypes", "Calendar", c => c.String(nullable: false));
            AddColumn("dbo.BusinessUserTypes", "Updates", c => c.String(nullable: false));
            AddColumn("dbo.BusinessUserTypes", "Store", c => c.String(nullable: false));
            AddColumn("dbo.BusinessUserTypes", "Messenger", c => c.String(nullable: false));
            AddColumn("dbo.BusinessUserTypes", "Meetings", c => c.String(nullable: false));
            AddColumn("dbo.BusinessUserTypes", "Ideas", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessUserTypes", "Ideas");
            DropColumn("dbo.BusinessUserTypes", "Meetings");
            DropColumn("dbo.BusinessUserTypes", "Messenger");
            DropColumn("dbo.BusinessUserTypes", "Store");
            DropColumn("dbo.BusinessUserTypes", "Updates");
            DropColumn("dbo.BusinessUserTypes", "Calendar");
            DropColumn("dbo.BusinessUserTypes", "Administrator");
        }
    }
}
