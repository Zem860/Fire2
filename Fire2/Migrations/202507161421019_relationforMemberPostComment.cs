namespace Fire2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class relationforMemberPostComment : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Comments", "MemberId");
            CreateIndex("dbo.Comments", "PostId");
            AddForeignKey("dbo.Comments", "MemberId", "dbo.Members", "Id");
            AddForeignKey("dbo.Comments", "PostId", "dbo.Posts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Comments", "MemberId", "dbo.Members");
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropIndex("dbo.Comments", new[] { "MemberId" });
        }
    }
}
