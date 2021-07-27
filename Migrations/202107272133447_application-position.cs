namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationposition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "PositionID", c => c.Int(nullable: false));
            CreateIndex("dbo.Applications", "PositionID");
            AddForeignKey("dbo.Applications", "PositionID", "dbo.Positions", "PositionID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Applications", "PositionID", "dbo.Positions");
            DropIndex("dbo.Applications", new[] { "PositionID" });
            DropColumn("dbo.Applications", "PositionID");
        }
    }
}
