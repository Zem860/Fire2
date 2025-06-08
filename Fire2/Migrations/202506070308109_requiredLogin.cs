namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NavbarItems", "ShowForLoggedInUsers", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NavbarItems", "ShowForLoggedInUsers");
        }
    }
}
