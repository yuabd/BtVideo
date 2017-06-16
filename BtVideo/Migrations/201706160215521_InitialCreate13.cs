namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movie", "MetaDescription", c => c.String(maxLength: 500));
            AlterColumn("dbo.Movie", "MetaKeywords", c => c.String(maxLength: 300));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movie", "MetaKeywords", c => c.String(maxLength: 100));
            AlterColumn("dbo.Movie", "MetaDescription", c => c.String(maxLength: 300));
        }
    }
}
