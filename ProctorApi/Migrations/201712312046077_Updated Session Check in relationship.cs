namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedSessionCheckinrelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserCheckInInfoes", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCheckInInfoes", "CheckInInfo_Id", "dbo.CheckInInfoes");
            DropForeignKey("dbo.CheckInInfoes", "Id", "dbo.Sessions");
            DropIndex("dbo.CheckInInfoes", new[] { "Id" });
            DropIndex("dbo.UserCheckInInfoes", new[] { "User_Id" });
            DropIndex("dbo.UserCheckInInfoes", new[] { "CheckInInfo_Id" });
            CreateTable(
                "dbo.UserCheckIns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionId = c.Int(nullable: false),
                        UserId = c.String(),
                        CheckInTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId);
            
            AddColumn("dbo.Sessions", "ProctorCheckInTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "ActualSessionStartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "ActualSessionEndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Sessions", "Attendees10", c => c.Int(nullable: false));
            AddColumn("dbo.Sessions", "Attendees50", c => c.Int(nullable: false));
            AddColumn("dbo.Sessions", "Notes", c => c.String());
            DropTable("dbo.CheckInInfoes");
            DropTable("dbo.UserCheckInInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserCheckInInfoes",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        CheckInInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.CheckInInfo_Id });
            
            CreateTable(
                "dbo.CheckInInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ProctorCheckInTime = c.DateTime(nullable: false),
                        SessionStartTime = c.DateTime(nullable: false),
                        SessionEndTime = c.DateTime(nullable: false),
                        Attendees10 = c.Int(nullable: false),
                        Attendees50 = c.Int(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.UserCheckIns", "SessionId", "dbo.Sessions");
            DropIndex("dbo.UserCheckIns", new[] { "SessionId" });
            DropColumn("dbo.Sessions", "Notes");
            DropColumn("dbo.Sessions", "Attendees50");
            DropColumn("dbo.Sessions", "Attendees10");
            DropColumn("dbo.Sessions", "ActualSessionEndTime");
            DropColumn("dbo.Sessions", "ActualSessionStartTime");
            DropColumn("dbo.Sessions", "ProctorCheckInTime");
            DropTable("dbo.UserCheckIns");
            CreateIndex("dbo.UserCheckInInfoes", "CheckInInfo_Id");
            CreateIndex("dbo.UserCheckInInfoes", "User_Id");
            CreateIndex("dbo.CheckInInfoes", "Id");
            AddForeignKey("dbo.CheckInInfoes", "Id", "dbo.Sessions", "Id");
            AddForeignKey("dbo.UserCheckInInfoes", "CheckInInfo_Id", "dbo.CheckInInfoes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.UserCheckInInfoes", "User_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
