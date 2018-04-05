namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHolidayModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Solved = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 100),
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
            DropForeignKey("dbo.Cases", "User_Id", "dbo.Users");
            DropIndex("dbo.Cases", new[] { "User_Id" });
            DropTable("dbo.Cases");
        }
    }
}
