namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShiftModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shifts", "User_Id", "dbo.Users");
            DropIndex("dbo.Shifts", new[] { "User_Id" });
            DropTable("dbo.Shifts");
        }
    }
}
