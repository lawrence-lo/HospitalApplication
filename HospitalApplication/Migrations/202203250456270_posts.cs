namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class posts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Content = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropTable("dbo.Posts");
        }
    }
}
