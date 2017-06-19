namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate17 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MovieLink", "LinkName", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MovieLink", "LinkName", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
