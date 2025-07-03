namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequirement : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceHistories", "ServiceUnit", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.ServiceHistories", "JobTitle", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ServiceHistories", "StartYear", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "StartMonth", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "EndYear", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "EndMonth", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceHistories", "EndMonth", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "EndYear", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "StartMonth", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "StartYear", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "JobTitle", c => c.String(maxLength: 50));
            AlterColumn("dbo.ServiceHistories", "ServiceUnit", c => c.String(maxLength: 100));
        }
    }
}
