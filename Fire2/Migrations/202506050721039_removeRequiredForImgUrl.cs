namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeRequiredForImgUrl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Experts", "ImgUrl", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Experts", "ImgUrl", c => c.String(nullable: false, maxLength: 300));
        }
    }
}
