namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedObjects : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CheckInInfoes", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CheckInInfoes", "Notes", c => c.Int(nullable: false));
        }
    }
}
