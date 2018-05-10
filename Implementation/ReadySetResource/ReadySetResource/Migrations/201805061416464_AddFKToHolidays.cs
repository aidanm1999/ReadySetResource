namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFKToHolidays : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Holidays", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Holidays", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Holidays", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Holidays", name: "UserId", newName: "User_Id");
        }
    }
}
