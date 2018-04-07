namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailVerifToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "EmailConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "EmailConfirmed");
        }
    }
}
