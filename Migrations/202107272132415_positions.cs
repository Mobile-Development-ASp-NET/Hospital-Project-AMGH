namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class positions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionID = c.Int(nullable: false, identity: true),
                        PositionJob = c.String(),
                        PositionDescription = c.String(),
                        PositionPostedDate = c.DateTime(nullable: false),
                        ApplicationDeadLine = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PositionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Positions");
        }
    }
}
