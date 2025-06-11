namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeTotalYearMonthsToServiceHistory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ServiceHistories", "TotalYears");
            DropColumn("dbo.ServiceHistories", "TotalMonths");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceHistories", "TotalMonths", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceHistories", "TotalYears", c => c.Int(nullable: false));
        }
    }
}
