namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Donations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        DonationID = c.Int(nullable: false, identity: true),
                        DonationAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonationDate = c.DateTime(nullable: false),
                        DonationDescription = c.String(),
                        DonorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonationID)
                .ForeignKey("dbo.Donors", t => t.DonorID, cascadeDelete: true)
                .Index(t => t.DonorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "DonorID", "dbo.Donors");
            DropIndex("dbo.Donations", new[] { "DonorID" });
            DropTable("dbo.Donations");
        }
    }
}
