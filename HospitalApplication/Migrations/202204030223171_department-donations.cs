namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departmentdonations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donations", "Department_DeptID", c => c.Int());
            CreateIndex("dbo.Donations", "Department_DeptID");
            AddForeignKey("dbo.Donations", "Department_DeptID", "dbo.Departments", "DeptID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Donations", "Department_DeptID", "dbo.Departments");
            DropIndex("dbo.Donations", new[] { "Department_DeptID" });
            DropColumn("dbo.Donations", "Department_DeptID");
        }
    }
}
