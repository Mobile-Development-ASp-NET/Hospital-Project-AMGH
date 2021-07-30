namespace Hospital_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class greetingcard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admissions",
                c => new
                    {
                        AdmissionId = c.Int(nullable: false, identity: true),
                        Room = c.String(),
                        Bed = c.String(),
                        DrId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AdmissionId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.DoctorDetails", t => t.DrId, cascadeDelete: true)
                .Index(t => t.DrId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GreetingCards",
                c => new
                    {
                        CardId = c.Int(nullable: false, identity: true),
                        SenderFirstName = c.String(),
                        SenderLastName = c.String(),
                        CardType = c.String(),
                        CardHasPic = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                        AdmissionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CardId)
                .ForeignKey("dbo.Admissions", t => t.AdmissionId, cascadeDelete: true)
                .Index(t => t.AdmissionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GreetingCards", "AdmissionId", "dbo.Admissions");
            DropForeignKey("dbo.Admissions", "DrId", "dbo.DoctorDetails");
            DropForeignKey("dbo.Admissions", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.GreetingCards", new[] { "AdmissionId" });
            DropIndex("dbo.Admissions", new[] { "UserId" });
            DropIndex("dbo.Admissions", new[] { "DrId" });
            DropTable("dbo.GreetingCards");
            DropTable("dbo.Admissions");
        }
    }
}
