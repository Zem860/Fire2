namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Knowledgesubtitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Knowledges", "SubTitle", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Knowledges", "SubTitle");
        }
    }
}
