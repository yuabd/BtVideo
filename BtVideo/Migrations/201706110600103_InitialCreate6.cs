namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate6 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MovieComment", "CaptchaCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MovieComment", "CaptchaCode", c => c.String(maxLength: 6));
        }
    }
}
