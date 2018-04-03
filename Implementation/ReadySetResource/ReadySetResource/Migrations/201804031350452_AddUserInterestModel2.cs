namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserInterestModel2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserInterests", "Code", c => c.String(nullable: false));
            AlterColumn("dbo.UserInterests", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInterests", "DateTime", c => c.Int(nullable: false));
            AlterColumn("dbo.UserInterests", "Code", c => c.Int(nullable: false));
        }
    }
}
