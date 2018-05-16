namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedIdeaToHolidayInBusinessUserType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessUserTypes", "Holidays", c => c.String(nullable: false));
            DropColumn("dbo.BusinessUserTypes", "Ideas");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BusinessUserTypes", "Ideas", c => c.String(nullable: false));
            DropColumn("dbo.BusinessUserTypes", "Holidays");
        }
    }
}
