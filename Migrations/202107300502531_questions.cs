namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "QuestionDescription", c => c.String());
            DropColumn("dbo.Questions", "QuestionyDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "QuestionyDescription", c => c.String());
            DropColumn("dbo.Questions", "QuestionDescription");
        }
    }
}
