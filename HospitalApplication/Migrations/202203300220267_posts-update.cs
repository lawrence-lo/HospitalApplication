namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class postsupdate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Posts", new[] { "UserId" });
            CreateIndex("dbo.Posts", "UserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Posts", new[] { "UserID" });
            CreateIndex("dbo.Posts", "UserId");
        }
    }
}
