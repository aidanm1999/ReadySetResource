namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Question_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Question_Id)
                .Index(t => t.User_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        BackupEmail = c.String(),
                        AddressLine1 = c.String(maxLength: 255),
                        AddressLine2 = c.String(maxLength: 255),
                        Postcode = c.String(maxLength: 8),
                        Blocked = c.Boolean(nullable: false),
                        TimesLoggedIn = c.Int(nullable: false),
                        NIN = c.String(),
                        EmergencyContact = c.String(),
                        Raise = c.Single(nullable: false),
                        Strikes = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        BusinessUserType_Id = c.Int(),
                        EmployeeType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BusinessUserTypes", t => t.BusinessUserType_Id)
                .ForeignKey("dbo.EmployeeTypes", t => t.EmployeeType_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.BusinessUserType_Id)
                .Index(t => t.EmployeeType_Id);
            
            CreateTable(
                "dbo.BusinessUserTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Business_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.Business_Id)
                .Index(t => t.Business_Id);
            
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Type = c.String(nullable: false, maxLength: 255),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        AddressLine1 = c.String(maxLength: 255),
                        AddressLine2 = c.String(maxLength: 255),
                        Postcode = c.String(maxLength: 10),
                        Town = c.String(maxLength: 40),
                        Region = c.String(maxLength: 40),
                        Country = c.String(maxLength: 40),
                        CardType = c.String(maxLength: 10),
                        CardNumber = c.String(maxLength: 16),
                        ExpiryMonth = c.String(maxLength: 2),
                        ExpiryYear = c.String(maxLength: 2),
                        SecuriyNumber = c.String(maxLength: 3),
                        Plan = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EmployeeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 70),
                        Description = c.String(maxLength: 255),
                        BaseSalary = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.DataOverTimes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(nullable: false),
                        MemoryMB = c.Single(nullable: false),
                        Business_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.Business_Id)
                .Index(t => t.Business_Id);
            
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
                "dbo.Errors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false, maxLength: 100),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Holidays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        Accepted = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Ideas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Recipient_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.Passwords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PassString = c.String(nullable: false, maxLength: 20),
                        Valid = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDateTime = c.DateTime(nullable: false),
                        EndDateTime = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.Recipient_Id)
                .ForeignKey("dbo.Businesses", t => t.Sender_Id)
                .Index(t => t.Recipient_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.Updates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Business_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.Business_Id)
                .Index(t => t.Business_Id);
            
            CreateTable(
                "dbo.UserInterests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInterests", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Updates", "Business_Id", "dbo.Businesses");
            DropForeignKey("dbo.Transactions", "Sender_Id", "dbo.Businesses");
            DropForeignKey("dbo.Transactions", "Recipient_Id", "dbo.Businesses");
            DropForeignKey("dbo.Shifts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Passwords", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Recipient_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Items", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ideas", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Holidays", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DataOverTimes", "Business_Id", "dbo.Businesses");
            DropForeignKey("dbo.Cases", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Questions", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "EmployeeType_Id", "dbo.EmployeeTypes");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "BusinessUserType_Id", "dbo.BusinessUserTypes");
            DropForeignKey("dbo.BusinessUserTypes", "Business_Id", "dbo.Businesses");
            DropIndex("dbo.UserInterests", new[] { "User_Id" });
            DropIndex("dbo.Updates", new[] { "Business_Id" });
            DropIndex("dbo.Transactions", new[] { "Sender_Id" });
            DropIndex("dbo.Transactions", new[] { "Recipient_Id" });
            DropIndex("dbo.Shifts", new[] { "User_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Passwords", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "Sender_Id" });
            DropIndex("dbo.Messages", new[] { "Recipient_Id" });
            DropIndex("dbo.Items", new[] { "User_Id" });
            DropIndex("dbo.Ideas", new[] { "User_Id" });
            DropIndex("dbo.Holidays", new[] { "User_Id" });
            DropIndex("dbo.DataOverTimes", new[] { "Business_Id" });
            DropIndex("dbo.Cases", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.BusinessUserTypes", new[] { "Business_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "EmployeeType_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "BusinessUserType_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Questions", new[] { "User_Id" });
            DropIndex("dbo.Answers", new[] { "User_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropTable("dbo.UserInterests");
            DropTable("dbo.Updates");
            DropTable("dbo.Transactions");
            DropTable("dbo.Shifts");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Passwords");
            DropTable("dbo.Messages");
            DropTable("dbo.Items");
            DropTable("dbo.Ideas");
            DropTable("dbo.Holidays");
            DropTable("dbo.Errors");
            DropTable("dbo.DataTransferRates");
            DropTable("dbo.DataOverTimes");
            DropTable("dbo.Cases");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.EmployeeTypes");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Businesses");
            DropTable("dbo.BusinessUserTypes");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
