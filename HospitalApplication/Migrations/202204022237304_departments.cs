namespace HospitalApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DeptID = c.Int(nullable: false, identity: true),
                        DeptName = c.String(),
                        DeptLocation = c.String(),
                        DeptDescription = c.String(),
                    })
                .PrimaryKey(t => t.DeptID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Departments");
        }
    }
}
