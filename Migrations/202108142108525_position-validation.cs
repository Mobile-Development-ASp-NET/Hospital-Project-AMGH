namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class positionvalidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Positions", "PositionJob", c => c.String(nullable: false));
            AlterColumn("dbo.Positions", "PositionDescription", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Positions", "PositionDescription", c => c.String());
            AlterColumn("dbo.Positions", "PositionJob", c => c.String());
        }
    }
}
