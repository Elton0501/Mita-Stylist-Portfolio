namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class landscape : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AlbumImages", "isLandscape", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AlbumImages", "isLandscape");
        }
    }
}
