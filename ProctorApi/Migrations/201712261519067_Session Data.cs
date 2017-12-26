namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SessionData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionTime = c.DateTime(nullable: false),
                        SessionStartTime = c.DateTime(nullable: false),
                        SessionEndTime = c.DateTime(nullable: false),
                        Title = c.String(),
                        Abstract = c.String(),
                        SessionType = c.String(),
                        Category = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Speakers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Biography = c.String(),
                        GravatarUrl = c.String(),
                        TwitterLink = c.String(),
                        GitHubLink = c.String(),
                        LinkedInProfile = c.String(),
                        BlogUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId);
            
            CreateTable(
                "dbo.SpeakerSessions",
                c => new
                    {
                        Speaker_Id = c.String(nullable: false, maxLength: 128),
                        Session_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Speaker_Id, t.Session_Id })
                .ForeignKey("dbo.Speakers", t => t.Speaker_Id, cascadeDelete: true)
                .ForeignKey("dbo.Sessions", t => t.Session_Id, cascadeDelete: true)
                .Index(t => t.Speaker_Id)
                .Index(t => t.Session_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tags", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.SpeakerSessions", "Session_Id", "dbo.Sessions");
            DropForeignKey("dbo.SpeakerSessions", "Speaker_Id", "dbo.Speakers");
            DropForeignKey("dbo.Rooms", "SessionId", "dbo.Sessions");
            DropIndex("dbo.SpeakerSessions", new[] { "Session_Id" });
            DropIndex("dbo.SpeakerSessions", new[] { "Speaker_Id" });
            DropIndex("dbo.Tags", new[] { "SessionId" });
            DropIndex("dbo.Rooms", new[] { "SessionId" });
            DropTable("dbo.SpeakerSessions");
            DropTable("dbo.Tags");
            DropTable("dbo.Speakers");
            DropTable("dbo.Sessions");
            DropTable("dbo.Rooms");
        }
    }
}
