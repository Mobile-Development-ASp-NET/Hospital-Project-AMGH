namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class survey : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SurveysQuestions", newName: "SurveyQuestions");
            RenameColumn(table: "dbo.SurveyQuestions", name: "Surveys_SurveyID", newName: "Survey_SurveyID");
            RenameColumn(table: "dbo.SurveyQuestions", name: "Questions_QuestionID", newName: "Question_QuestionID");
            RenameIndex(table: "dbo.SurveyQuestions", name: "IX_Surveys_SurveyID", newName: "IX_Survey_SurveyID");
            RenameIndex(table: "dbo.SurveyQuestions", name: "IX_Questions_QuestionID", newName: "IX_Question_QuestionID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SurveyQuestions", name: "IX_Question_QuestionID", newName: "IX_Questions_QuestionID");
            RenameIndex(table: "dbo.SurveyQuestions", name: "IX_Survey_SurveyID", newName: "IX_Surveys_SurveyID");
            RenameColumn(table: "dbo.SurveyQuestions", name: "Question_QuestionID", newName: "Questions_QuestionID");
            RenameColumn(table: "dbo.SurveyQuestions", name: "Survey_SurveyID", newName: "Surveys_SurveyID");
            RenameTable(name: "dbo.SurveyQuestions", newName: "SurveysQuestions");
        }
    }
}
