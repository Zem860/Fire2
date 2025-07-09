namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cancel_required_news_content : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Content", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "Content", c => c.String(nullable: false));
        }
    }
}
