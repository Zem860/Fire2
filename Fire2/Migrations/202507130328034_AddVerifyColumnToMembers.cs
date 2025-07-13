namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVerifyColumnToMembers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "IsVerified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "IsVerified");
        }
    }
}
