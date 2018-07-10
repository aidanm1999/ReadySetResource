namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Answers", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Answers", new[] { "User_Id" });
            AddColumn("dbo.Answers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Answers", "Question_Id1", c => c.Int());
            AlterColumn("dbo.Answers", "Title", c => c.String());
            AlterColumn("dbo.Answers", "User_Id", c => c.String());
            AlterColumn("dbo.BusinessUserTypes", "Name", c => c.String());
            AlterColumn("dbo.BusinessUserTypes", "Administrator", c => c.String());
            AlterColumn("dbo.BusinessUserTypes", "Calendar", c => c.String());
            AlterColumn("dbo.BusinessUserTypes", "Updates", c => c.String());
            AlterColumn("dbo.BusinessUserTypes", "Store", c => c.String());
            AlterColumn("dbo.BusinessUserTypes", "Messenger", c => c.String());
            AlterColumn("dbo.BusinessUserTypes", "Meetings", c => c.String());
            AlterColumn("dbo.BusinessUserTypes", "Holidays", c => c.String());
            AlterColumn("dbo.Businesses", "Name", c => c.String());
            AlterColumn("dbo.Businesses", "Type", c => c.String());
            AlterColumn("dbo.Businesses", "AddressLine1", c => c.String());
            AlterColumn("dbo.Businesses", "AddressLine2", c => c.String());
            AlterColumn("dbo.Businesses", "Postcode", c => c.String());
            AlterColumn("dbo.Businesses", "Town", c => c.String());
            AlterColumn("dbo.Businesses", "Region", c => c.String());
            AlterColumn("dbo.Businesses", "Country", c => c.String());
            AlterColumn("dbo.Businesses", "CardType", c => c.String());
            AlterColumn("dbo.Businesses", "CardNumber", c => c.String());
            AlterColumn("dbo.Businesses", "ExpiryMonth", c => c.String());
            AlterColumn("dbo.Businesses", "ExpiryYear", c => c.String());
            AlterColumn("dbo.Businesses", "SecuriyNumber", c => c.String());
            CreateIndex("dbo.Answers", "ApplicationUser_Id");
            CreateIndex("dbo.Answers", "Question_Id1");
            AddForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions", "Id");
            AddForeignKey("dbo.Answers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id1" });
            DropIndex("dbo.Answers", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Businesses", "SecuriyNumber", c => c.String(maxLength: 3));
            AlterColumn("dbo.Businesses", "ExpiryYear", c => c.String(maxLength: 2));
            AlterColumn("dbo.Businesses", "ExpiryMonth", c => c.String(maxLength: 2));
            AlterColumn("dbo.Businesses", "CardNumber", c => c.String(maxLength: 16));
            AlterColumn("dbo.Businesses", "CardType", c => c.String(maxLength: 10));
            AlterColumn("dbo.Businesses", "Country", c => c.String(maxLength: 40));
            AlterColumn("dbo.Businesses", "Region", c => c.String(maxLength: 40));
            AlterColumn("dbo.Businesses", "Town", c => c.String(maxLength: 40));
            AlterColumn("dbo.Businesses", "Postcode", c => c.String(maxLength: 10));
            AlterColumn("dbo.Businesses", "AddressLine2", c => c.String(maxLength: 255));
            AlterColumn("dbo.Businesses", "AddressLine1", c => c.String(maxLength: 255));
            AlterColumn("dbo.Businesses", "Type", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Businesses", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.BusinessUserTypes", "Holidays", c => c.String(nullable: false));
            AlterColumn("dbo.BusinessUserTypes", "Meetings", c => c.String(nullable: false));
            AlterColumn("dbo.BusinessUserTypes", "Messenger", c => c.String(nullable: false));
            AlterColumn("dbo.BusinessUserTypes", "Store", c => c.String(nullable: false));
            AlterColumn("dbo.BusinessUserTypes", "Updates", c => c.String(nullable: false));
            AlterColumn("dbo.BusinessUserTypes", "Calendar", c => c.String(nullable: false));
            AlterColumn("dbo.BusinessUserTypes", "Administrator", c => c.String(nullable: false));
            AlterColumn("dbo.BusinessUserTypes", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Answers", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Answers", "Title", c => c.String(nullable: false));
            DropColumn("dbo.Answers", "Question_Id1");
            DropColumn("dbo.Answers", "ApplicationUser_Id");
            CreateIndex("dbo.Answers", "User_Id");
            CreateIndex("dbo.Answers", "Question_Id");
            AddForeignKey("dbo.Answers", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Answers", "Question_Id", "dbo.Questions", "Id");
        }
    }
}
