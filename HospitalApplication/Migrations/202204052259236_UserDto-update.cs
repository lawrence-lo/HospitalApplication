namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDtoupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDtoes", "Department_DeptID", "dbo.Departments");
            DropIndex("dbo.UserDtoes", new[] { "Department_DeptID" });
            DropTable("dbo.UserDtoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserDtoes",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        UserName = c.String(),
                        LastName = c.String(),
                        GivenName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Department = c.Int(nullable: false),
                        Position = c.String(),
                        HireDate = c.DateTime(nullable: false),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Department_DeptID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateIndex("dbo.UserDtoes", "Department_DeptID");
            AddForeignKey("dbo.UserDtoes", "Department_DeptID", "dbo.Departments", "DeptID");
        }
    }
}
