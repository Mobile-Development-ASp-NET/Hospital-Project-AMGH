namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Survey : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Surveys", "SurveyDescription", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Surveys", "SurveyDescription", c => c.String(nullable: false));
        }
    }
}
