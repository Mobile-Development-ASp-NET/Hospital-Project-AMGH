namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationvalidation2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applications", "ApplicationStatus", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applications", "ApplicationStatus", c => c.String());
        }
    }
}
