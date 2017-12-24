namespace ProctorApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Gravatar", c => c.String());
            AddColumn("dbo.AspNetUsers", "CellNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CellNumber");
            DropColumn("dbo.AspNetUsers", "Gravatar");
        }
    }
}
