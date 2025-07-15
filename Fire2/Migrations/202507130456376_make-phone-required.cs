namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class makephonerequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Phone", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "Mobile", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Mobile", c => c.String());
            AlterColumn("dbo.Members", "Phone", c => c.String());
        }
    }
}
