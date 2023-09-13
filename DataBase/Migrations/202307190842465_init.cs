namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlbumImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        Description = c.String(),
                        AlbumId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Albums", t => t.AlbumId, cascadeDelete: true)
                .Index(t => t.AlbumId);
            
            CreateTable(
                "dbo.Albums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThumbnailImage = c.String(),
                        Name = c.String(),
                        isFeatured = c.Boolean(nullable: false),
                        isBigBanner = c.Boolean(nullable: false),
                        CatId = c.Int(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CatId)
                .Index(t => t.CatId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Logo = c.String(),
                        PreferNo = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Service = c.String(nullable: false),
                        Message = c.String(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HomeBanners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(nullable: false),
                        Heading = c.String(nullable: false),
                        SubHeading = c.String(),
                        SubTitle = c.String(),
                        ButtonText = c.String(),
                        ButtonUrl = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Keys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        Description = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Newsletters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        Status = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        isMultipleUser = c.Boolean(nullable: false),
                        TypeId = c.String(),
                        AnchorLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Guid(nullable: false),
                        RoleName = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TravelAlbums",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BannerImage = c.String(),
                        ThumbnailImage = c.String(),
                        AlbumDate = c.DateTime(nullable: false),
                        AlbumDesc = c.String(),
                        isFeatured = c.Boolean(nullable: false),
                        isBigBanner = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TravelAlbums", t => t.TravelAlbumId, cascadeDelete: true)
                .Index(t => t.TravelAlbumId);
            
            CreateTable(
                "dbo.TravelAlbumImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        TravelAlbumDescId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TravelAlbumDescs", t => t.TravelAlbumDescId, cascadeDelete: true)
                .Index(t => t.TravelAlbumDescId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MobileNumber = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Varified = c.Boolean(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelAlbumImages", "TravelAlbumDescId", "dbo.TravelAlbumDescs");
            DropForeignKey("dbo.TravelAlbumDescs", "TravelAlbumId", "dbo.TravelAlbums");
            DropForeignKey("dbo.Albums", "CatId", "dbo.Categories");
            DropForeignKey("dbo.AlbumImages", "AlbumId", "dbo.Albums");
            DropIndex("dbo.TravelAlbumImages", new[] { "TravelAlbumDescId" });
            DropIndex("dbo.TravelAlbumDescs", new[] { "TravelAlbumId" });
            DropIndex("dbo.Albums", new[] { "CatId" });
            DropIndex("dbo.AlbumImages", new[] { "AlbumId" });
            DropTable("dbo.Users");
            DropTable("dbo.TravelAlbumImages");
            DropTable("dbo.TravelAlbumDescs");
            DropTable("dbo.TravelAlbums");
            DropTable("dbo.Roles");
            DropTable("dbo.Notifications");
            DropTable("dbo.Newsletters");
            DropTable("dbo.Keys");
            DropTable("dbo.HomeBanners");
            DropTable("dbo.Contacts");
            DropTable("dbo.Clients");
            DropTable("dbo.Categories");
            DropTable("dbo.Albums");
            DropTable("dbo.AlbumImages");
        }
    }
}
