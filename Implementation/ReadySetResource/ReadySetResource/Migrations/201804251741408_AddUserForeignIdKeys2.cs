namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserForeignIdKeys2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "BusinessUserTypeId", "dbo.BusinessUserTypes");
            DropForeignKey("dbo.AspNetUsers", "EmployeeTypeId", "dbo.EmployeeTypes");
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeTypeId" });
            DropIndex("dbo.AspNetUsers", new[] { "BusinessUserTypeId" });
            RenameColumn(table: "dbo.AspNetUsers", name: "BusinessUserTypeId", newName: "BusinessUserType_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "EmployeeTypeId", newName: "EmployeeType_Id");
            AddColumn("dbo.AspNetUsers", "EmpTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "BusTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "EmployeeType_Id", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "BusinessUserType_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "BusinessUserType_Id");
            CreateIndex("dbo.AspNetUsers", "EmployeeType_Id");
            AddForeignKey("dbo.AspNetUsers", "BusinessUserType_Id", "dbo.BusinessUserTypes", "Id");
            AddForeignKey("dbo.AspNetUsers", "EmployeeType_Id", "dbo.EmployeeTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "EmployeeType_Id", "dbo.EmployeeTypes");
            DropForeignKey("dbo.AspNetUsers", "BusinessUserType_Id", "dbo.BusinessUserTypes");
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeType_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "BusinessUserType_Id" });
            AlterColumn("dbo.AspNetUsers", "BusinessUserType_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "EmployeeType_Id", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "BusTypeId");
            DropColumn("dbo.AspNetUsers", "EmpTypeId");
            RenameColumn(table: "dbo.AspNetUsers", name: "EmployeeType_Id", newName: "EmployeeTypeId");
            RenameColumn(table: "dbo.AspNetUsers", name: "BusinessUserType_Id", newName: "BusinessUserTypeId");
            CreateIndex("dbo.AspNetUsers", "BusinessUserTypeId");
            CreateIndex("dbo.AspNetUsers", "EmployeeTypeId");
            AddForeignKey("dbo.AspNetUsers", "EmployeeTypeId", "dbo.EmployeeTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "BusinessUserTypeId", "dbo.BusinessUserTypes", "Id", cascadeDelete: true);
        }
    }
}
