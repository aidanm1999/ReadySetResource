namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserForeignIdKeys5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AspNetUsers", new[] { "BusinessUserTypeid" });
            CreateIndex("dbo.AspNetUsers", "BusinessUserTypeId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", new[] { "BusinessUserTypeId" });
            CreateIndex("dbo.AspNetUsers", "BusinessUserTypeid");
        }
    }
}
