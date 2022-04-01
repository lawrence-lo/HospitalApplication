namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pendingmigrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donors", "UserID", c => c.String(maxLength: 128));
            AddColumn("dbo.Donations", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Donations", "UserID");
            CreateIndex("dbo.Donors", "UserID");
            AddForeignKey("dbo.Donations", "UserID", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Donors", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donors", "UserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Donations", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Donors", new[] { "UserID" });
            DropIndex("dbo.Donations", new[] { "UserID" });
            DropColumn("dbo.Donations", "UserID");
            DropColumn("dbo.Donors", "UserID");
        }
    }
}
