namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MovieLink", "Magnet", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MovieLink", "Magnet", c => c.String(maxLength: 200));
        }
    }
}
