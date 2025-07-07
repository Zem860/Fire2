namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AboutPageContentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AboutPageContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PageKey = c.String(nullable: false, maxLength: 100),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AboutPageContents");
        }
    }
}
