namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Response : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Responses",
                c => new
                    {
                        ResponseID = c.Int(nullable: false, identity: true),
                        Answer = c.String(),
                        QuestionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseID)
                .ForeignKey("dbo.Questions", t => t.QuestionID, cascadeDelete: true)
                .Index(t => t.QuestionID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responses", "QuestionID", "dbo.Questions");
            DropIndex("dbo.Responses", new[] { "QuestionID" });
            DropTable("dbo.Responses");
        }
    }
}
