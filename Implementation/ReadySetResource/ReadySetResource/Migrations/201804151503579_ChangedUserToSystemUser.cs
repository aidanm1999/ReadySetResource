namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserToSystemUser : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "SystemUsers");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SystemUsers", newName: "Users");
        }
    }
}
