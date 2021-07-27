namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Surveys1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionID = c.Int(nullable: false, identity: true),
                        QuestionTitle = c.String(),
                        QuestionyDescription = c.String(),
                    })
                .PrimaryKey(t => t.QuestionID);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        SurveyID = c.Int(nullable: false, identity: true),
                        SurveyTitle = c.String(),
                        SurveyDescription = c.String(),
                    })
                .PrimaryKey(t => t.SurveyID);
            
            CreateTable(
                "dbo.SurveysQuestions",
                c => new
                    {
                        Surveys_SurveyID = c.Int(nullable: false),
                        Questions_QuestionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Surveys_SurveyID, t.Questions_QuestionID })
                .ForeignKey("dbo.Surveys", t => t.Surveys_SurveyID, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Questions_QuestionID, cascadeDelete: true)
                .Index(t => t.Surveys_SurveyID)
                .Index(t => t.Questions_QuestionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SurveysQuestions", "Questions_QuestionID", "dbo.Questions");
            DropForeignKey("dbo.SurveysQuestions", "Surveys_SurveyID", "dbo.Surveys");
            DropIndex("dbo.SurveysQuestions", new[] { "Questions_QuestionID" });
            DropIndex("dbo.SurveysQuestions", new[] { "Surveys_SurveyID" });
            DropTable("dbo.SurveysQuestions");
            DropTable("dbo.Surveys");
            DropTable("dbo.Questions");
        }
    }
}
