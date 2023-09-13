namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class visitorcount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageVisitCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VisitPage = c.String(),
                        SessionID = c.String(),
                        VisitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebVisitCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SessionID = c.String(),
                        VisitDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebVisitCounts");
            DropTable("dbo.PageVisitCounts");
        }
    }
}
