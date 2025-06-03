namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class expertPostcontent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Experts", "PostContent", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Experts", "PostContent");
        }
    }
}
