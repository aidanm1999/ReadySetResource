namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBusinessUserTypesForeignIdKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BusinessUserTypes", "Business_Id", "dbo.Businesses");
            DropIndex("dbo.BusinessUserTypes", new[] { "Business_Id" });
            RenameColumn(table: "dbo.BusinessUserTypes", name: "Business_Id", newName: "BusinessId");
            AlterColumn("dbo.BusinessUserTypes", "BusinessId", c => c.Int(nullable: false));
            CreateIndex("dbo.BusinessUserTypes", "BusinessId");
            AddForeignKey("dbo.BusinessUserTypes", "BusinessId", "dbo.Businesses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BusinessUserTypes", "BusinessId", "dbo.Businesses");
            DropIndex("dbo.BusinessUserTypes", new[] { "BusinessId" });
            AlterColumn("dbo.BusinessUserTypes", "BusinessId", c => c.Int());
            RenameColumn(table: "dbo.BusinessUserTypes", name: "BusinessId", newName: "Business_Id");
            CreateIndex("dbo.BusinessUserTypes", "Business_Id");
            AddForeignKey("dbo.BusinessUserTypes", "Business_Id", "dbo.Businesses", "Id");
        }
    }
}
