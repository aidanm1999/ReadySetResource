namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionModel2 : DbMigration
    {
        public override void Up()
        {
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
                        SenderId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.RecipientId, cascadeDelete: false)
                .ForeignKey("dbo.Businesses", t => t.SenderId, cascadeDelete: false)
                .Index(t => t.SenderId)
                .Index(t => t.RecipientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "SenderId", "dbo.Businesses");
            DropForeignKey("dbo.Transactions", "RecipientId", "dbo.Businesses");
            DropIndex("dbo.Transactions", new[] { "RecipientId" });
            DropIndex("dbo.Transactions", new[] { "SenderId" });
            DropTable("dbo.Transactions");
        }
    }
}
