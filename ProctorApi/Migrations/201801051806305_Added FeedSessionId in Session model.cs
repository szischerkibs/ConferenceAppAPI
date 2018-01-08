namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFeedSessionIdinSessionmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "FeedSessionId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "FeedSessionId");
        }
    }
}
