namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class survey1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Positions", "PositionJob", c => c.String(nullable: false));
            AlterColumn("dbo.Positions", "PositionDescription", c => c.String(nullable: false));
            AlterColumn("dbo.Applications", "ApplicationName", c => c.String(nullable: false));
            AlterColumn("dbo.Applications", "ApplicationEmail", c => c.String(nullable: false));
            AlterColumn("dbo.Applications", "ApplicationStatus", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Applications", "ApplicationStatus", c => c.String());
            AlterColumn("dbo.Applications", "ApplicationEmail", c => c.String());
            AlterColumn("dbo.Applications", "ApplicationName", c => c.String());
            AlterColumn("dbo.Positions", "PositionDescription", c => c.String());
            AlterColumn("dbo.Positions", "PositionJob", c => c.String());
        }
    }
}
