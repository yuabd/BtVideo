namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate15 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movie", "Magnet", c => c.String(maxLength: 800));
            AlterColumn("dbo.MovieLink", "Magnet", c => c.String(maxLength: 800));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MovieLink", "Magnet", c => c.String(maxLength: 500));
            AlterColumn("dbo.Movie", "Magnet", c => c.String(maxLength: 500));
        }
    }
}
