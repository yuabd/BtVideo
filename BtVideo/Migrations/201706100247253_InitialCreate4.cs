namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "ImdbLink", c => c.String(maxLength: 200));
            AddColumn("dbo.Movie", "ImdbTitle", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "ImdbTitle");
            DropColumn("dbo.Movie", "ImdbLink");
        }
    }
}
