namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletedUserInterests : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserInterests", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserInterests", new[] { "User_Id" });
            DropTable("dbo.UserInterests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserInterests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UserInterests", "User_Id");
            AddForeignKey("dbo.UserInterests", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
