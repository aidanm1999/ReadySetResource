namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CleanedModels : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Questions", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions");
            DropForeignKey("dbo.Cases", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DataOverTimes", "Business_Id", "dbo.Businesses");
            DropForeignKey("dbo.Ideas", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Items", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Recipient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Passwords", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Transactions", "Recipient_Id", "dbo.Businesses");
            DropForeignKey("dbo.Transactions", "Sender_Id", "dbo.Businesses");
            DropForeignKey("dbo.Updates", "Business_Id", "dbo.Businesses");
            DropIndex("dbo.Answers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id1" });
            DropIndex("dbo.Questions", new[] { "User_Id" });
            DropIndex("dbo.Cases", new[] { "User_Id" });
            DropIndex("dbo.DataOverTimes", new[] { "Business_Id" });
            DropIndex("dbo.Ideas", new[] { "User_Id" });
            DropIndex("dbo.Items", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "Recipient_Id" });
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropIndex("dbo.Passwords", new[] { "User_Id" });
            DropIndex("dbo.Transactions", new[] { "Recipient_Id" });
            DropIndex("dbo.Transactions", new[] { "Sender_Id" });
            DropIndex("dbo.Updates", new[] { "Business_Id" });
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
            DropTable("dbo.Cases");
            DropTable("dbo.DataOverTimes");
            DropTable("dbo.DataTransferRates");
            DropTable("dbo.Errors");
            DropTable("dbo.Ideas");
            DropTable("dbo.Items");
            DropTable("dbo.Messages");
            DropTable("dbo.Passwords");
            DropTable("dbo.Transactions");
            DropTable("dbo.Updates");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Updates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Business_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Amount = c.Single(nullable: false),
                        VAT = c.Single(nullable: false),
                        Total = c.Single(nullable: false),
                        DateSent = c.DateTime(nullable: false),
                        DateRefunded = c.DateTime(nullable: false),
                        Recipient_Id = c.Int(),
                        Sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Passwords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PassString = c.String(nullable: false, maxLength: 20),
                        Valid = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTimeSend = c.DateTime(nullable: false),
                        DateTimeViewed = c.DateTime(nullable: false),
                        ContentString = c.String(),
                        ContentImagePath = c.String(),
                        Recipient_Id = c.String(maxLength: 128),
                        Sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        PhotoPath = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Errors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 100),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataTransferRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        StartPage = c.String(nullable: false),
                        EndPage = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataOverTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        MemoryMB = c.Single(nullable: false),
                        Business_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Solved = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 100),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Solved = c.Boolean(nullable: false),
                        Title = c.String(nullable: false),
                        Description = c.Int(nullable: false),
                        Views = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Question_Id = c.Int(),
                        User_Id = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Question_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Updates", "Business_Id");
            CreateIndex("dbo.Transactions", "Sender_Id");
            CreateIndex("dbo.Transactions", "Recipient_Id");
            CreateIndex("dbo.Passwords", "User_Id");
            CreateIndex("dbo.Messages", "Sender_Id");
            CreateIndex("dbo.Messages", "Recipient_Id");
            CreateIndex("dbo.Items", "User_Id");
            CreateIndex("dbo.Ideas", "User_Id");
            CreateIndex("dbo.DataOverTimes", "Business_Id");
            CreateIndex("dbo.Cases", "User_Id");
            CreateIndex("dbo.Questions", "User_Id");
            CreateIndex("dbo.Answers", "Question_Id1");
            CreateIndex("dbo.Answers", "ApplicationUser_Id");
            AddForeignKey("dbo.Updates", "Business_Id", "dbo.Businesses", "Id");
            AddForeignKey("dbo.Transactions", "Sender_Id", "dbo.Businesses", "Id");
            AddForeignKey("dbo.Transactions", "Recipient_Id", "dbo.Businesses", "Id");
            AddForeignKey("dbo.Passwords", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "Sender_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "Recipient_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Items", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Ideas", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.DataOverTimes", "Business_Id", "dbo.Businesses", "Id");
            AddForeignKey("dbo.Cases", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions", "Id");
            AddForeignKey("dbo.Questions", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Answers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
