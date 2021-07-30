namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cardmessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GreetingCards", "CardMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GreetingCards", "CardMessage");
        }
    }
}
