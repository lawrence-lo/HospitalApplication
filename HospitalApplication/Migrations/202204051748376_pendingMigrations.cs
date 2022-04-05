namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pendingMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        DoctorName = c.String(),
                        Description = c.String(),
                        PatientID = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DOB = c.DateTime(nullable: false),
                        PhoneNo = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PatientID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Appointments", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Appointments", new[] { "UserID" });
            DropIndex("dbo.Appointments", new[] { "PatientID" });
            DropTable("dbo.Patients");
            DropTable("dbo.Appointments");
        }
    }
}
