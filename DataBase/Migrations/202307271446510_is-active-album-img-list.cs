namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isactivealbumimglist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlbumImages", "isFeatured", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AlbumImages", "isFeatured");
        }
    }
}
