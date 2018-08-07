namespace ReadySetResource.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColourToBusinessUserType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BusinessUserTypes", "Colour", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BusinessUserTypes", "Colour");
        }
    }
}
