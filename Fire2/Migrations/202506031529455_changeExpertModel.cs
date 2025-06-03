namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeExpertModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experts", "Education", c => c.String());
            AddColumn("dbo.Experts", "Introduction", c => c.String());
            AddColumn("dbo.Experts", "Others", c => c.String());
            DropColumn("dbo.Experts", "PostContent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Experts", "PostContent", c => c.String());
            DropColumn("dbo.Experts", "Others");
            DropColumn("dbo.Experts", "Introduction");
            DropColumn("dbo.Experts", "Education");
        }
    }
}
