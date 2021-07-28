namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applications : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        ApplicationID = c.Int(nullable: false, identity: true),
                        ApplicationName = c.String(),
                        ApplicationDOB = c.DateTime(nullable: false),
                        ApplicationEmail = c.String(),
                        ApplicationCriminalRecord = c.Boolean(nullable: false),
                        ApplicationStatus = c.String(),
                    })
                .PrimaryKey(t => t.ApplicationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Applications");
        }
    }
}
