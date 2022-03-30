namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Donors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donors",
                c => new
                    {
                        DonorID = c.Int(nullable: false, identity: true),
                        DonorName = c.String(),
                        DonorEmail = c.String(),
                        DonorAddress = c.String(),
                        DonorPhone = c.String(),
                    })
                .PrimaryKey(t => t.DonorID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Donors");
        }
    }
}
