namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DoctorDetails",
                c => new
                    {
                        DrId = c.Int(nullable: false, identity: true),
                        DrFname = c.String(),
                        DrLname = c.String(),
                        DrEmail = c.String(),
                        DrBio = c.String(),
                        DrStudies = c.String(),
                        DrPosition = c.String(),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DrId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DoctorDetails", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.DoctorDetails", new[] { "DepartmentId" });
            DropTable("dbo.DoctorDetails");
        }
    }
}
