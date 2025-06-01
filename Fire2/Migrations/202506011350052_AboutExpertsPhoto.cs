namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AboutExpertsPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experts", "ImgUrl", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Experts", "Title", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Experts", "Title", c => c.String(maxLength: 50));
            DropColumn("dbo.Experts", "ImgUrl");
        }
    }
}
