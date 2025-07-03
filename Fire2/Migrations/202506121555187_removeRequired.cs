namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceHistories", "ServiceUnit", c => c.String(maxLength: 100));
            AlterColumn("dbo.ServiceHistories", "JobTitle", c => c.String(maxLength: 50));
            AlterColumn("dbo.ServiceHistories", "StartYear", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "EndYear", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "EndMonth", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceHistories", "EndMonth", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "EndYear", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "StartYear", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "JobTitle", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ServiceHistories", "ServiceUnit", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
