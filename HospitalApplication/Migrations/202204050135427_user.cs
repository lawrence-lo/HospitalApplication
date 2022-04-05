namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "GivenName", c => c.String());
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Department", c => c.String());
            AddColumn("dbo.AspNetUsers", "Position", c => c.String());
            AddColumn("dbo.AspNetUsers", "HireDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Salary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Salary");
            DropColumn("dbo.AspNetUsers", "HireDate");
            DropColumn("dbo.AspNetUsers", "Position");
            DropColumn("dbo.AspNetUsers", "Department");
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "GivenName");
            DropColumn("dbo.AspNetUsers", "LastName");
        }
    }
}
