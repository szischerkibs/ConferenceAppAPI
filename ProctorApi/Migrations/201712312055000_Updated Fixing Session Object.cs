namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedFixingSessionObject : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sessions", "SessionTime", c => c.DateTime());
            AlterColumn("dbo.Sessions", "SessionStartTime", c => c.DateTime());
            AlterColumn("dbo.Sessions", "SessionEndTime", c => c.DateTime());
            AlterColumn("dbo.Sessions", "ActualSessionStartTime", c => c.DateTime());
            AlterColumn("dbo.Sessions", "ActualSessionEndTime", c => c.DateTime());
            DropColumn("dbo.Sessions", "ProctorCheckInTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Sessions", "ProctorCheckInTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sessions", "ActualSessionEndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sessions", "ActualSessionStartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sessions", "SessionEndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sessions", "SessionStartTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Sessions", "SessionTime", c => c.DateTime(nullable: false));
        }
    }
}
