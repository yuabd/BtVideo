namespace BtVideo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SpiderTask",
                c => new
                    {
                        SpiderTaskID = c.Int(nullable: false, identity: true),
                        CurrentID = c.Int(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SpiderTaskID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SpiderTask");
        }
    }
}
