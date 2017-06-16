namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MovieComment", "CaptchaCode", c => c.String(maxLength: 6));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MovieComment", "CaptchaCode");
        }
    }
}
