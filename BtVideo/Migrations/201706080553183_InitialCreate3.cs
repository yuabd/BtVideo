namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movie", "Grade", c => c.Double(nullable: false));
            AddColumn("dbo.Movie", "Magnet", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movie", "Magnet");
            DropColumn("dbo.Movie", "Grade");
        }
    }
}
