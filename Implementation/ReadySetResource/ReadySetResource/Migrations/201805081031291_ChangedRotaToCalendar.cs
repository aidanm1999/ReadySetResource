namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedRotaToCalendar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessUserTypes", "Calendar", c => c.String(nullable: false));
            DropColumn("dbo.BusinessUserTypes", "Rota");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BusinessUserTypes", "Rota", c => c.String(nullable: false));
            DropColumn("dbo.BusinessUserTypes", "Calendar");
        }
    }
}
