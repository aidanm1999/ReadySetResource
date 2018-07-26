namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedFKsInTypeAppAccess : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TypeAppAccesses", "App_Id", "dbo.Apps");
            DropForeignKey("dbo.TypeAppAccesses", "BusinessUserType_Id", "dbo.BusinessUserTypes");
            DropIndex("dbo.TypeAppAccesses", new[] { "App_Id" });
            DropIndex("dbo.TypeAppAccesses", new[] { "BusinessUserType_Id" });
            DropColumn("dbo.TypeAppAccesses", "AppId");
            DropColumn("dbo.TypeAppAccesses", "BusinessUserTypeId");
            RenameColumn(table: "dbo.TypeAppAccesses", name: "App_Id", newName: "AppId");
            RenameColumn(table: "dbo.TypeAppAccesses", name: "BusinessUserType_Id", newName: "BusinessUserTypeId");
            AlterColumn("dbo.TypeAppAccesses", "AppId", c => c.Int(nullable: false));
            AlterColumn("dbo.TypeAppAccesses", "BusinessUserTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.TypeAppAccesses", "AppId", c => c.Int(nullable: false));
            AlterColumn("dbo.TypeAppAccesses", "BusinessUserTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.TypeAppAccesses", "AppId");
            CreateIndex("dbo.TypeAppAccesses", "BusinessUserTypeId");
            AddForeignKey("dbo.TypeAppAccesses", "AppId", "dbo.Apps", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TypeAppAccesses", "BusinessUserTypeId", "dbo.BusinessUserTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TypeAppAccesses", "BusinessUserTypeId", "dbo.BusinessUserTypes");
            DropForeignKey("dbo.TypeAppAccesses", "AppId", "dbo.Apps");
            DropIndex("dbo.TypeAppAccesses", new[] { "BusinessUserTypeId" });
            DropIndex("dbo.TypeAppAccesses", new[] { "AppId" });
            AlterColumn("dbo.TypeAppAccesses", "BusinessUserTypeId", c => c.Int());
            AlterColumn("dbo.TypeAppAccesses", "AppId", c => c.Int());
            AlterColumn("dbo.TypeAppAccesses", "BusinessUserTypeId", c => c.String());
            AlterColumn("dbo.TypeAppAccesses", "AppId", c => c.String());
            RenameColumn(table: "dbo.TypeAppAccesses", name: "BusinessUserTypeId", newName: "BusinessUserType_Id");
            RenameColumn(table: "dbo.TypeAppAccesses", name: "AppId", newName: "App_Id");
            AddColumn("dbo.TypeAppAccesses", "BusinessUserTypeId", c => c.String());
            AddColumn("dbo.TypeAppAccesses", "AppId", c => c.String());
            CreateIndex("dbo.TypeAppAccesses", "BusinessUserType_Id");
            CreateIndex("dbo.TypeAppAccesses", "App_Id");
            AddForeignKey("dbo.TypeAppAccesses", "BusinessUserType_Id", "dbo.BusinessUserTypes", "Id");
            AddForeignKey("dbo.TypeAppAccesses", "App_Id", "dbo.Apps", "Id");
        }
    }
}
