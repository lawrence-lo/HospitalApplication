namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jobdepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "DeptID", c => c.Int(nullable: false));
            CreateIndex("dbo.Jobs", "DeptID");
            AddForeignKey("dbo.Jobs", "DeptID", "dbo.Departments", "DeptID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "DeptID", "dbo.Departments");
            DropIndex("dbo.Jobs", new[] { "DeptID" });
            DropColumn("dbo.Jobs", "DeptID");
        }
    }
}
