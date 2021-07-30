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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "DrId", "dbo.DoctorDetails");
            DropForeignKey("dbo.Feedbacks", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Feedbacks", new[] { "DrId" });
            DropIndex("dbo.Feedbacks", new[] { "UserId" });
            DropTable("dbo.Feedbacks");
        }
    }
}
