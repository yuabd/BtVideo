namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate12 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MovieTag");
            AlterColumn("dbo.MovieTag", "Tag", c => c.String(nullable: false, maxLength: 50));
            AddPrimaryKey("dbo.MovieTag", new[] { "MovieID", "Tag" });
            DropColumn("dbo.Movie", "Tags");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movie", "Tags", c => c.String());
            DropPrimaryKey("dbo.MovieTag");
            AlterColumn("dbo.MovieTag", "Tag", c => c.String(nullable: false, maxLength: 20));
            AddPrimaryKey("dbo.MovieTag", new[] { "MovieID", "Tag" });
        }
    }
}
