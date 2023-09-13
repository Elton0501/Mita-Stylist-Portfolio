namespace DataBase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class albumlandscape : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Albums", "isLandscape", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Albums", "isLandscape");
        }
    }
}
