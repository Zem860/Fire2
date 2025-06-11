namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMembership : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "MembershipType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "MembershipType");
        }
    }
}
