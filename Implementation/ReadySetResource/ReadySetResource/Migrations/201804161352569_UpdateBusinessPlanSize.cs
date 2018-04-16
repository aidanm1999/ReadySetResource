namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBusinessPlanSize : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Businesses", "Plan", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Businesses", "Plan", c => c.String(maxLength: 10));
        }
    }
}
