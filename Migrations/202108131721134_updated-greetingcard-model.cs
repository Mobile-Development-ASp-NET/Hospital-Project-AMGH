namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedgreetingcardmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GreetingCards", "UserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.GreetingCards", "SenderFirstName", c => c.String(nullable: false));
            AlterColumn("dbo.GreetingCards", "SenderLastName", c => c.String(nullable: false));
            AlterColumn("dbo.GreetingCards", "CardMessage", c => c.String(nullable: false));
            CreateIndex("dbo.GreetingCards", "UserID");
            AddForeignKey("dbo.GreetingCards", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GreetingCards", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.GreetingCards", new[] { "UserID" });
            AlterColumn("dbo.GreetingCards", "CardMessage", c => c.String());
            AlterColumn("dbo.GreetingCards", "SenderLastName", c => c.String());
            AlterColumn("dbo.GreetingCards", "SenderFirstName", c => c.String());
            DropColumn("dbo.GreetingCards", "UserID");
        }
    }
}
