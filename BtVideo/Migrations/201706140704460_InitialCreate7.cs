namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movie", "PictureFile", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movie", "PictureFile", c => c.String(maxLength: 56));
        }
    }
}
