namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeyIdToShift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shifts", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shifts", "UserId");
        }
    }
}
