namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate14 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HotKeyword",
                c => new
                    {
                        Keyword = c.String(nullable: false, maxLength: 128),
                        Count = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Keyword);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HotKeyword");
        }
    }
}
