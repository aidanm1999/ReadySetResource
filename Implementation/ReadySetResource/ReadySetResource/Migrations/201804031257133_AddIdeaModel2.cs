namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdeaModel2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ideas", "User_Id", "dbo.Users");
            DropIndex("dbo.Ideas", new[] { "User_Id" });
            DropTable("dbo.Ideas");
        }
    }
}
