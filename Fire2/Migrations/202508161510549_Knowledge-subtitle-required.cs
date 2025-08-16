namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Knowledgesubtitlerequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Knowledges", "SubTitle", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Knowledges", "SubTitle", c => c.String(maxLength: 50));
        }
    }
}
