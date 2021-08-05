namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Positions", "DepartmentID", c => c.Int(nullable: false));
            CreateIndex("dbo.Positions", "DepartmentID");
            AddForeignKey("dbo.Positions", "DepartmentID", "dbo.Departments", "DepartmentID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Positions", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Positions", new[] { "DepartmentID" });
            DropColumn("dbo.Positions", "DepartmentID");
        }
    }
}
