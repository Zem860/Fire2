namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removephoneRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Phone", c => c.String());
            AlterColumn("dbo.Members", "Mobile", c => c.String());
            DropColumn("dbo.Members", "Captcha");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "Captcha", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "Mobile", c => c.String(nullable: false));
            AlterColumn("dbo.Members", "Phone", c => c.String(nullable: false));
        }
    }
}
