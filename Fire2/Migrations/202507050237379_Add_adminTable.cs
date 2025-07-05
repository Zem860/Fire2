namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_adminTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account = c.String(nullable: false, maxLength: 50),
                        PasswordHash = c.String(nullable: false, maxLength: 100),
                        Salt = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 100),
                        Gender = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Admins");
        }
    }
}
