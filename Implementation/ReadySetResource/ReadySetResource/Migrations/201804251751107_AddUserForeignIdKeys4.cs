namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserForeignIdKeys4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "BusinessUserType_Id", "dbo.BusinessUserTypes");
            DropForeignKey("dbo.AspNetUsers", "EmployeeType_Id", "dbo.EmployeeTypes");
            DropIndex("dbo.AspNetUsers", new[] { "BusinessUserType_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeType_Id" });
            RenameColumn(table: "dbo.AspNetUsers", name: "BusinessUserType_Id", newName: "BusinessUserTypeid");
            RenameColumn(table: "dbo.AspNetUsers", name: "EmployeeType_Id", newName: "EmployeeTypeId");
            AlterColumn("dbo.AspNetUsers", "BusinessUserTypeid", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "EmployeeTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "EmployeeTypeId");
            CreateIndex("dbo.AspNetUsers", "BusinessUserTypeid");
            AddForeignKey("dbo.AspNetUsers", "BusinessUserTypeid", "dbo.BusinessUserTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "EmployeeTypeId", "dbo.EmployeeTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "EmployeeTypeId", "dbo.EmployeeTypes");
            DropForeignKey("dbo.AspNetUsers", "BusinessUserTypeid", "dbo.BusinessUserTypes");
            DropIndex("dbo.AspNetUsers", new[] { "BusinessUserTypeid" });
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeTypeId" });
            AlterColumn("dbo.AspNetUsers", "EmployeeTypeId", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "BusinessUserTypeid", c => c.Int());
            RenameColumn(table: "dbo.AspNetUsers", name: "EmployeeTypeId", newName: "EmployeeType_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "BusinessUserTypeid", newName: "BusinessUserType_Id");
            CreateIndex("dbo.AspNetUsers", "EmployeeType_Id");
            CreateIndex("dbo.AspNetUsers", "BusinessUserType_Id");
            AddForeignKey("dbo.AspNetUsers", "EmployeeType_Id", "dbo.EmployeeTypes", "Id");
            AddForeignKey("dbo.AspNetUsers", "BusinessUserType_Id", "dbo.BusinessUserTypes", "Id");
        }
    }
}
