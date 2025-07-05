namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTypeBackToInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "TotalYears", c => c.Int(nullable: false));
            AlterColumn("dbo.Members", "TotalMonths", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "TotalMonths", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "TotalYears", c => c.String(nullable: false));
        }
    }
}
