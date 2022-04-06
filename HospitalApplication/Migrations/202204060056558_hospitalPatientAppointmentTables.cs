namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hospitalPatientAppointmentTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HospitalAppointments",
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
                .ForeignKey("dbo.HospitalPatients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.HospitalPatients",
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
            DropForeignKey("dbo.HospitalAppointments", "PatientID", "dbo.HospitalPatients");
            DropForeignKey("dbo.HospitalAppointments", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.HospitalAppointments", new[] { "UserID" });
            DropIndex("dbo.HospitalAppointments", new[] { "PatientID" });
            DropTable("dbo.HospitalPatients");
            DropTable("dbo.HospitalAppointments");
        }
    }
}
