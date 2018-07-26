namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccessTypeForUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TypeAppAccesses", "AccessType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TypeAppAccesses", "AccessType");
        }
    }
}
