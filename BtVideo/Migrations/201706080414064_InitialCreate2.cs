namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MovieTag", "Blog_MovieID", "dbo.Movie");
            DropIndex("dbo.MovieTag", new[] { "Blog_MovieID" });
            RenameColumn(table: "dbo.MovieTag", name: "Blog_MovieID", newName: "MovieID");
            DropPrimaryKey("dbo.MovieTag");
            AlterColumn("dbo.MovieTag", "MovieID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.MovieTag", new[] { "MovieID", "Tag" });
            CreateIndex("dbo.MovieTag", "MovieID");
            AddForeignKey("dbo.MovieTag", "MovieID", "dbo.Movie", "MovieID", cascadeDelete: true);
            DropColumn("dbo.MovieTag", "BlogID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovieTag", "BlogID", c => c.Int(nullable: false));
            DropForeignKey("dbo.MovieTag", "MovieID", "dbo.Movie");
            DropIndex("dbo.MovieTag", new[] { "MovieID" });
            DropPrimaryKey("dbo.MovieTag");
            AlterColumn("dbo.MovieTag", "MovieID", c => c.Int());
            AddPrimaryKey("dbo.MovieTag", new[] { "BlogID", "Tag" });
            RenameColumn(table: "dbo.MovieTag", name: "MovieID", newName: "Blog_MovieID");
            CreateIndex("dbo.MovieTag", "Blog_MovieID");
            AddForeignKey("dbo.MovieTag", "Blog_MovieID", "dbo.Movie", "MovieID");
        }
    }
}
