namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class survey2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Surveys", "SurveyTitle", c => c.String(nullable: false));
            AlterColumn("dbo.Surveys", "SurveyDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Surveys", "SurveyDescription", c => c.String());
            AlterColumn("dbo.Surveys", "SurveyTitle", c => c.String());
        }
    }
}
