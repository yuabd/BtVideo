namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieLink", "LinkName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.MovieLink", "LinkUrl", c => c.String(nullable: false, maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MovieLink", "LinkUrl", c => c.String());
            DropColumn("dbo.MovieLink", "LinkName");
        }
    }
}
