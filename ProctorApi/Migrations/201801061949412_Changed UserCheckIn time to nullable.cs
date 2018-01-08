namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserCheckIntimetonullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserCheckIns", "CheckInTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserCheckIns", "CheckInTime", c => c.DateTime(nullable: false));
        }
    }
}
