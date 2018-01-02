namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CheckInInfo : DbMigration
    {
        public override void Up()
        {
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
                        Notes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.UserCheckInInfoes",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        CheckInInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.CheckInInfo_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.CheckInInfoes", t => t.CheckInInfo_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.CheckInInfo_Id);
            
            CreateTable(
                "dbo.SessionUsers",
                c => new
                    {
                        Session_Id = c.Int(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Session_Id, t.User_Id })
                .ForeignKey("dbo.Sessions", t => t.Session_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Session_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Sessions", "VolunteersRequired", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckInInfoes", "Id", "dbo.Sessions");
            DropForeignKey("dbo.SessionUsers", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SessionUsers", "Session_Id", "dbo.Sessions");
            DropForeignKey("dbo.UserCheckInInfoes", "CheckInInfo_Id", "dbo.CheckInInfoes");
            DropForeignKey("dbo.UserCheckInInfoes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.SessionUsers", new[] { "User_Id" });
            DropIndex("dbo.SessionUsers", new[] { "Session_Id" });
            DropIndex("dbo.UserCheckInInfoes", new[] { "CheckInInfo_Id" });
            DropIndex("dbo.UserCheckInInfoes", new[] { "User_Id" });
            DropIndex("dbo.CheckInInfoes", new[] { "Id" });
            DropColumn("dbo.Sessions", "VolunteersRequired");
            DropTable("dbo.SessionUsers");
            DropTable("dbo.UserCheckInInfoes");
            DropTable("dbo.CheckInInfoes");
        }
    }
}
