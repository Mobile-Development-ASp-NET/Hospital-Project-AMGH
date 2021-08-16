namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Question : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "QuestionTitle", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "QuestionTitle", c => c.String());
        }
    }
}
