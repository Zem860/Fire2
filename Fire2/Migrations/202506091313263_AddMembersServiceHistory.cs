namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMembersServiceHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NavbarItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NavbarItem = c.String(nullable: false, maxLength: 30),
                        Link = c.String(nullable: false, maxLength: 70),
                        ParentId = c.Int(),
                        DisplayOrder = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        ShowForLoggedInUsers = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NavbarItems", t => t.ParentId)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NavbarItems", "ParentId", "dbo.NavbarItems");
            DropIndex("dbo.NavbarItems", new[] { "ParentId" });
            DropTable("dbo.NavbarItems");
        }
    }
}
