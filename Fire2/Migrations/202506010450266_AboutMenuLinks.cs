namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AboutMenuLinks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AboutMenus", "Link", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AboutMenus", "Link");
        }
    }
}
