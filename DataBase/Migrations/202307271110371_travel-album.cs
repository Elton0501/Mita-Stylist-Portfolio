namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class travelalbum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TravelAlbumDescs", "TravelAlbumId", "dbo.TravelAlbums");
            DropForeignKey("dbo.TravelAlbumImages", "TravelAlbumDescId", "dbo.TravelAlbumDescs");
            DropIndex("dbo.TravelAlbumDescs", new[] { "TravelAlbumId" });
            DropIndex("dbo.TravelAlbumImages", new[] { "TravelAlbumDescId" });
            AddColumn("dbo.TravelAlbums", "imgdefault", c => c.String());
            AddColumn("dbo.TravelAlbumImages", "Default", c => c.Boolean(nullable: false));
            AddColumn("dbo.TravelAlbumImages", "TravelAlbumId", c => c.Int(nullable: false));
            CreateIndex("dbo.TravelAlbumImages", "TravelAlbumId");
            AddForeignKey("dbo.TravelAlbumImages", "TravelAlbumId", "dbo.TravelAlbums", "Id", cascadeDelete: true);
            DropColumn("dbo.TravelAlbums", "BannerImage");
            DropColumn("dbo.TravelAlbums", "ThumbnailImage");
            DropColumn("dbo.TravelAlbums", "AlbumDesc");
            DropColumn("dbo.TravelAlbums", "isBigBanner");
            DropColumn("dbo.TravelAlbumImages", "TravelAlbumDescId");
            DropTable("dbo.TravelAlbumDescs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TravelAlbumDescs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ColumnNo = c.Int(nullable: false),
                        TravelAlbumId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TravelAlbumImages", "TravelAlbumDescId", c => c.Int(nullable: false));
            AddColumn("dbo.TravelAlbums", "isBigBanner", c => c.Boolean(nullable: false));
            AddColumn("dbo.TravelAlbums", "AlbumDesc", c => c.String());
            AddColumn("dbo.TravelAlbums", "ThumbnailImage", c => c.String());
            AddColumn("dbo.TravelAlbums", "BannerImage", c => c.String());
            DropForeignKey("dbo.TravelAlbumImages", "TravelAlbumId", "dbo.TravelAlbums");
            DropIndex("dbo.TravelAlbumImages", new[] { "TravelAlbumId" });
            DropColumn("dbo.TravelAlbumImages", "TravelAlbumId");
            DropColumn("dbo.TravelAlbumImages", "Default");
            DropColumn("dbo.TravelAlbums", "imgdefault");
            CreateIndex("dbo.TravelAlbumImages", "TravelAlbumDescId");
            CreateIndex("dbo.TravelAlbumDescs", "TravelAlbumId");
            AddForeignKey("dbo.TravelAlbumImages", "TravelAlbumDescId", "dbo.TravelAlbumDescs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TravelAlbumDescs", "TravelAlbumId", "dbo.TravelAlbums", "Id", cascadeDelete: true);
        }
    }
}
