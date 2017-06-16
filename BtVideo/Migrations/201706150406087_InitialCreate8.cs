namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "DateUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.MovieLink", "Magnet", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieLink", "Magnet");
            DropColumn("dbo.Movie", "DateUpdate");
        }
    }
}
