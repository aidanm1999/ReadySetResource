namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedAppLocation : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Apps", "IconLocation");
            DropColumn("dbo.Apps", "HomeLocation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Apps", "HomeLocation", c => c.String(nullable: false));
            AddColumn("dbo.Apps", "IconLocation", c => c.String(nullable: false));
        }
    }
}
