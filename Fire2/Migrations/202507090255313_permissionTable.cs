namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class permissionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ParentId = c.Int(),
                        Code = c.String(maxLength: 5),
                        Url = c.String(maxLength: 500),
                        Icon = c.String(maxLength: 200),
                        ControllerName = c.String(),
                        ActionName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissions", t => t.ParentId)
                .Index(t => t.ParentId);
            
            AddColumn("dbo.Admins", "Permission", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "ParentId", "dbo.Permissions");
            DropIndex("dbo.Permissions", new[] { "ParentId" });
            DropColumn("dbo.Admins", "Permission");
            DropTable("dbo.Permissions");
        }
    }
}
