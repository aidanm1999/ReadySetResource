namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAvitarToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Avitar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Avitar");
        }
    }
}
