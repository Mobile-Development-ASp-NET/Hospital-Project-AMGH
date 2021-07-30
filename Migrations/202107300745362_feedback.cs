namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class feedback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        FeedbackId = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        DrId = c.Int(nullable: false),
                        FeedbackContent = c.String(),
                        FeedbackDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FeedbackId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.DoctorDetails", t => t.DrId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.DrId);
            
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        BlogID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.BlogID);
            
            CreateTable(
                "dbo.SubscribedUsers",
                c => new
                    {
                        SubscribedUserID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.SubscribedUserID);
            
            CreateTable(
                "dbo.SubscribedUserBlogs",
                c => new
                    {
                        SubscribedUser_SubscribedUserID = c.Int(nullable: false),
                        Blog_BlogID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubscribedUser_SubscribedUserID, t.Blog_BlogID })
                .ForeignKey("dbo.SubscribedUsers", t => t.SubscribedUser_SubscribedUserID, cascadeDelete: true)
                .ForeignKey("dbo.Blogs", t => t.Blog_BlogID, cascadeDelete: true)
                .Index(t => t.SubscribedUser_SubscribedUserID)
                .Index(t => t.Blog_BlogID);
            
            AddColumn("dbo.Questions", "QuestionyDescription", c => c.String());
            DropColumn("dbo.Questions", "QuestionDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "QuestionDescription", c => c.String());
            DropForeignKey("dbo.SubscribedUserBlogs", "Blog_BlogID", "dbo.Blogs");
            DropForeignKey("dbo.SubscribedUserBlogs", "SubscribedUser_SubscribedUserID", "dbo.SubscribedUsers");
            DropForeignKey("dbo.Feedbacks", "DrId", "dbo.DoctorDetails");
            DropForeignKey("dbo.Feedbacks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.SubscribedUserBlogs", new[] { "Blog_BlogID" });
            DropIndex("dbo.SubscribedUserBlogs", new[] { "SubscribedUser_SubscribedUserID" });
            DropIndex("dbo.Feedbacks", new[] { "DrId" });
            DropIndex("dbo.Feedbacks", new[] { "UserId" });
            DropColumn("dbo.Questions", "QuestionyDescription");
            DropTable("dbo.SubscribedUserBlogs");
            DropTable("dbo.SubscribedUsers");
            DropTable("dbo.Blogs");
            DropTable("dbo.Feedbacks");
        }
    }
}
