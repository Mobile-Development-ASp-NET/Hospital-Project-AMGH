namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subscribed_user : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubscribedUserBlogs", "Blog_BlogID", "dbo.Blogs");
            DropForeignKey("dbo.SubscribedUserBlogs", "SubscribedUser_SubscribedUserID", "dbo.SubscribedUsers");
            DropIndex("dbo.SubscribedUserBlogs", new[] { "Blog_BlogID" });
            DropIndex("dbo.SubscribedUserBlogs", new[] { "SubscribedUser_SubscribedUserID" });
            DropTable("dbo.SubscribedUserBlogs");
            DropTable("dbo.SubscribedUsers");
            DropTable("dbo.Blogs");
        }
    }
}
