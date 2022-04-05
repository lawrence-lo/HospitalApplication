namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userdepartment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Department", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Department", c => c.String());
        }
    }
}
