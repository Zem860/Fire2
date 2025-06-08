namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NavbarItems", "Position", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NavbarItems", "Position");
        }
    }
}
