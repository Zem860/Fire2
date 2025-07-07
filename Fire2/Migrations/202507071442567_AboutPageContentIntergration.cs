namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AboutPageContentIntergration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AboutPageContents", "Link", c => c.String(maxLength: 30));
            AddColumn("dbo.AboutPageContents", "CreatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AboutPageContents", "CreatedAt");
            DropColumn("dbo.AboutPageContents", "Link");
        }
    }
}
