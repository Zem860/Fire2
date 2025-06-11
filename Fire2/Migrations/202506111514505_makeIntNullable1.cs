namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makeIntNullable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ServiceHistories", "StartYear", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "StartMonth", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "EndYear", c => c.Int());
            AlterColumn("dbo.ServiceHistories", "EndMonth", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ServiceHistories", "EndMonth", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "EndYear", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "StartMonth", c => c.Int(nullable: false));
            AlterColumn("dbo.ServiceHistories", "StartYear", c => c.Int(nullable: false));
        }
    }
}
