namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationvalidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Applications", "ApplicationName", c => c.String(nullable: false));
            AlterColumn("dbo.Applications", "ApplicationEmail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applications", "ApplicationEmail", c => c.String());
            AlterColumn("dbo.Applications", "ApplicationName", c => c.String());
        }
    }
}
