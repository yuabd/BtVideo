namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate16 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movie", "Magnet", c => c.String(maxLength: 1000));
            AlterColumn("dbo.MovieLink", "Magnet", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MovieLink", "Magnet", c => c.String(maxLength: 800));
            AlterColumn("dbo.Movie", "Magnet", c => c.String(maxLength: 800));
        }
    }
}
