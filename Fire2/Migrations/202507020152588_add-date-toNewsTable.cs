namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adddatetoNewsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.News", "UpdatedAt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "UpdatedAt");
            DropColumn("dbo.News", "CreatedAt");
        }
    }
}
