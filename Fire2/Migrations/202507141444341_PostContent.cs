namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostContent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Content");
        }
    }
}
