namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKeyIdToShift2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Shifts", new[] { "User_Id" });
            DropColumn("dbo.Shifts", "UserId");
            RenameColumn(table: "dbo.Shifts", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Shifts", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Shifts", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Shifts", new[] { "UserId" });
            AlterColumn("dbo.Shifts", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Shifts", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Shifts", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Shifts", "User_Id");
        }
    }
}
