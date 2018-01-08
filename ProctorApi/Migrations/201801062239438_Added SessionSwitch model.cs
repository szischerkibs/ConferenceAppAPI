namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSessionSwitchmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SessionSwitches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionId = c.Int(nullable: false),
                        ForSessionId = c.Int(),
                        OfferedBy = c.String(),
                        OfferedTo = c.String(),
                        Status = c.Int(nullable: false),
                        OfferedOn = c.DateTime(),
                        StatusChangedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SessionSwitches");
        }
    }
}
