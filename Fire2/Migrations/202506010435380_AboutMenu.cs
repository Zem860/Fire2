namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AboutMenu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AboutMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AboutItems = c.String(nullable: false, maxLength: 30),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AboutMenus");
        }
    }
}
