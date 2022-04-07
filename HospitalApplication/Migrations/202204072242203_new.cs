namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Donations", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Donors", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Donations", new[] { "UserID" });
            DropIndex("dbo.Donors", new[] { "UserID" });
            DropColumn("dbo.Donations", "UserID");
            DropColumn("dbo.Donors", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Donors", "UserID", c => c.String(maxLength: 128));
            AddColumn("dbo.Donations", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Donors", "UserID");
            CreateIndex("dbo.Donations", "UserID");
            AddForeignKey("dbo.Donors", "UserID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Donations", "UserID", "dbo.AspNetUsers", "Id");
        }
    }
}
