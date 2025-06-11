namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTotalYearMonthsToServiceHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceHistories", "TotalYears", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceHistories", "TotalMonths", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceHistories", "TotalMonths");
            DropColumn("dbo.ServiceHistories", "TotalYears");
        }
    }
}
